locals {
  sanitized_project = lower(replace(var.project_name, "[^0-9a-z-]", ""))
  base_name         = substr("${local.sanitized_project}-${var.environment}", 0, 40)
  backend_app_name  = "${local.base_name}-backend"
  frontend_app_name = "${local.base_name}-frontend"
  tags = merge({
    project     = var.project_name
    environment = var.environment
  }, var.tags)
}

resource "random_string" "suffix" {
  length  = 4
  upper   = false
  lower   = true
  numeric = true
  special = false
}

resource "azurerm_resource_group" "main" {
  name     = "rg-${local.base_name}"
  location = var.location
  tags     = local.tags
}

resource "azurerm_log_analytics_workspace" "main" {
  name                = substr(replace("${local.base_name}-log", "-", ""), 0, 63)
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  sku                 = "PerGB2018"
  retention_in_days   = 30
  tags                = local.tags
}

resource "azurerm_container_app_environment" "main" {
  name                       = "${local.base_name}-cae"
  resource_group_name        = azurerm_resource_group.main.name
  location                   = azurerm_resource_group.main.location
  log_analytics_workspace_id = azurerm_log_analytics_workspace.main.id
  tags                       = local.tags
}

resource "azurerm_container_registry" "main" {
  name                          = substr("${replace(local.sanitized_project, "-", "")}${var.environment}${random_string.suffix.result}", 0, 50)
  resource_group_name           = azurerm_resource_group.main.name
  location                      = azurerm_resource_group.main.location
  sku                           = "Standard"
  admin_enabled                 = true
  public_network_access_enabled = true
  tags                          = local.tags
}

locals {
  mysql_server_name = substr("${replace(local.sanitized_project, "-", "")}6${var.environment}${random_string.suffix.result}", 0, 40)
}

resource "azurerm_mysql_flexible_server" "main" {
  name                   = local.mysql_server_name
  resource_group_name    = azurerm_resource_group.main.name
  location               = azurerm_resource_group.main.location
  administrator_login    = var.mysql_admin_username
  administrator_password = var.mysql_admin_password
  sku_name               = var.mysql_sku_name
  version                = var.mysql_version
  storage {
    size_gb = var.mysql_storage_gb
  }
  backup_retention_days        = var.mysql_backup_retention_days
  geo_redundant_backup_enabled = false
  tags                         = local.tags
}

resource "azurerm_mysql_flexible_database" "main" {
  name                = var.mysql_database_name
  resource_group_name = azurerm_resource_group.main.name
  server_name         = azurerm_mysql_flexible_server.main.name
  charset             = "utf8mb4"
  collation           = "utf8mb4_unicode_ci"
}

resource "azurerm_mysql_flexible_server_firewall_rule" "azure_services" {
  count               = var.mysql_public_access && var.mysql_allow_azure_services ? 1 : 0
  name                = "AllowAzureServices"
  resource_group_name = azurerm_resource_group.main.name
  server_name         = azurerm_mysql_flexible_server.main.name
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "0.0.0.0"
}

resource "azurerm_mysql_flexible_server_firewall_rule" "custom" {
  for_each            = var.mysql_public_access ? { for rule in var.mysql_firewall_rules : rule.name => rule } : {}
  name                = each.value.name
  resource_group_name = azurerm_resource_group.main.name
  server_name         = azurerm_mysql_flexible_server.main.name
  start_ip_address    = each.value.start_ip
  end_ip_address      = each.value.end_ip
}

locals {
  backend_image  = var.backend_use_placeholder ? "mcr.microsoft.com/azuredocs/containerapps-helloworld:latest" : "${azurerm_container_registry.main.login_server}/${var.backend_image_name}:${var.backend_image_tag}"
  frontend_image = var.frontend_use_placeholder ? "mcr.microsoft.com/azuredocs/containerapps-helloworld:latest" : "${azurerm_container_registry.main.login_server}/${var.frontend_image_name}:${var.frontend_image_tag}"
  mysql_user     = var.mysql_admin_username
  mysql_conn     = "Server=${azurerm_mysql_flexible_server.main.fqdn};Database=${var.mysql_database_name};User Id=${local.mysql_user};Password=${var.mysql_admin_password};Ssl Mode=Required;"
}

locals {
  backend_fqdn  = "${local.backend_app_name}.${azurerm_container_app_environment.main.default_domain}"
  frontend_fqdn = "${local.frontend_app_name}.${azurerm_container_app_environment.main.default_domain}"
}

resource "azurerm_container_app" "backend" {
  name                         = local.backend_app_name
  resource_group_name          = azurerm_resource_group.main.name
  container_app_environment_id = azurerm_container_app_environment.main.id
  revision_mode                = "Single"

  identity {
    type = "SystemAssigned"
  }

  secret {
    name  = "mysql-connection"
    value = local.mysql_conn
  }

  secret {
    name  = "acr-pwd"
    value = azurerm_container_registry.main.admin_password
  }

  registry {
    server               = azurerm_container_registry.main.login_server
    username             = azurerm_container_registry.main.admin_username
    password_secret_name = "acr-pwd"
  }

  template {
    min_replicas = var.backend_min_replicas
    max_replicas = var.backend_max_replicas

    container {
      name   = "backend"
      image  = local.backend_image
      cpu    = var.backend_cpu
      memory = "${var.backend_memory}Gi"
      env {
        name        = "ConnectionStrings__DefaultConnection"
        secret_name = "mysql-connection"
      }
      env {
        name  = "ASPNETCORE_URLS"
        value = "http://0.0.0.0:${var.backend_container_port}"
      }
      env {
        name  = "Cors__AllowedOrigins"
        value = "https://${local.frontend_fqdn}"
      }
    }
  }

  ingress {
    target_port      = var.backend_container_port
    transport        = "auto"
    external_enabled = true
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }

  tags = local.tags

  lifecycle {
    ignore_changes = [
      template[0].container[0].image,
      template[0].revision_suffix
    ]
  }

  depends_on = [
    azurerm_mysql_flexible_database.main,
    #    null_resource.push_backend_image
  ]
}

resource "azurerm_container_app" "frontend" {
  name                         = local.frontend_app_name
  resource_group_name          = azurerm_resource_group.main.name
  container_app_environment_id = azurerm_container_app_environment.main.id
  revision_mode                = "Single"

  identity {
    type = "SystemAssigned"
  }

  secret {
    name  = "acr-pwd"
    value = azurerm_container_registry.main.admin_password
  }

  registry {
    server               = azurerm_container_registry.main.login_server
    username             = azurerm_container_registry.main.admin_username
    password_secret_name = "acr-pwd"
  }

  template {
    min_replicas = var.frontend_min_replicas
    max_replicas = var.frontend_max_replicas

    container {
      name   = "frontend"
      image  = local.frontend_image
      cpu    = var.frontend_cpu
      memory = "${var.frontend_memory}Gi"
      env {
        name  = "NEXT_PUBLIC_API_BASE_URL"
        value = "https://${local.backend_fqdn}"
      }
      env {
        name  = "API_BASE_URL"
        value = "https://${local.backend_fqdn}"
      }
      env {
        name  = "PORT"
        value = tostring(var.frontend_container_port)
      }
    }
  }

  ingress {
    target_port      = var.frontend_container_port
    transport        = "auto"
    external_enabled = true
    traffic_weight {
      percentage      = 100
      latest_revision = true
    }
  }

  tags = local.tags

  lifecycle {
    ignore_changes = [
      template[0].container[0].image,
      template[0].revision_suffix
    ]
  }

  depends_on = [
    #    null_resource.push_frontend_image,
    azurerm_mysql_flexible_database.main
  ]
}

resource "null_resource" "github_acr_variables" {
  triggers = {
    login_server  = azurerm_container_registry.main.login_server
    username      = azurerm_container_registry.main.admin_username
    password_hash = sha256(azurerm_container_registry.main.admin_password)
    repository    = var.github_repository
  }

  provisioner "local-exec" {
    interpreter = ["bash", "-c"]
    environment = {
      ACR_LOGIN_SERVER             = azurerm_container_registry.main.login_server
      ACR_USERNAME                 = azurerm_container_registry.main.admin_username
      ACR_PASSWORD                 = azurerm_container_registry.main.admin_password
      GH_REPOSITORY                = var.github_repository
      AZURE_RESOURCE_GROUP         = azurerm_resource_group.main.name
      AZURE_BACKEND_CONTAINER_APP  = azurerm_container_app.backend.name
      AZURE_FRONTEND_CONTAINER_APP = azurerm_container_app.frontend.name
      AZURE_SUBSCRIPTION_ID        = var.subscription_id
      RESOURCE_GROUP               = azurerm_resource_group.main.name
      BACKEND_CONTAINER_APP        = azurerm_container_app.backend.name
      FRONTEND_CONTAINER_APP       = azurerm_container_app.frontend.name
    }

    command = <<-EOT
      set -euo pipefail

      if ! command -v gh >/dev/null 2>&1; then
        echo "GitHub CLI (gh) not found in PATH" >&2
        exit 1
      fi

      if ! gh auth status >/dev/null 2>&1; then
        echo "GitHub CLI is not authenticated. Run 'gh auth login'." >&2
        exit 1
      fi

      if [ -n "$${GH_REPOSITORY:-}" ]; then
        gh secret set ACR_LOGIN_SERVER --repo "$${GH_REPOSITORY}" --body "$${ACR_LOGIN_SERVER}"
        gh secret set ACR_USERNAME --repo "$${GH_REPOSITORY}" --body "$${ACR_USERNAME}"
        gh secret set ACR_PASSWORD --repo "$${GH_REPOSITORY}" --body "$${ACR_PASSWORD}"
        gh variable set RESOURCE_GROUP --repo "$${GH_REPOSITORY}" --body "$${RESOURCE_GROUP}"
        gh variable set BACKEND_CONTAINER_APP --repo "$${GH_REPOSITORY}" --body "$${BACKEND_CONTAINER_APP}"
        gh variable set FRONTEND_CONTAINER_APP --repo "$${GH_REPOSITORY}" --body "$${FRONTEND_CONTAINER_APP}"
      fi
    EOT
  }

  depends_on = [
    azurerm_container_registry.main
  ]
}

#resource "null_resource" "push_backend_image" {
#  triggers = {
#    local_image   = var.backend_local_image
#    remote_image  = local.backend_image
#    password_hash = sha256(azurerm_container_registry.main.admin_password)
#  }
#
#  provisioner "local-exec" {
#    interpreter = ["bash", "-c"]
#    environment = {
#      ACR_LOGIN_SERVER = azurerm_container_registry.main.login_server
#      ACR_USERNAME     = azurerm_container_registry.main.admin_username
#      ACR_PASSWORD     = azurerm_container_registry.main.admin_password
#      LOCAL_IMAGE      = var.backend_local_image
#      REMOTE_IMAGE     = local.backend_image
#    }
#
#    command = <<-EOT
#      set -euo pipefail
#
#      if ! command -v docker >/dev/null 2>&1; then
#        echo "Docker CLI not found in PATH." >&2
#        exit 1
#      fi
#
#      if ! docker image inspect "$${LOCAL_IMAGE}" >/dev/null 2>&1; then
#        echo "Local Docker image '$${LOCAL_IMAGE}' not found. Build it before applying Terraform." >&2
#        exit 1
#      fi
#
#      echo "$${ACR_PASSWORD}" | docker login "$${ACR_LOGIN_SERVER}" -u "$${ACR_USERNAME}" --password-stdin
#      docker tag "$${LOCAL_IMAGE}" "$${REMOTE_IMAGE}"
#      docker push "$${REMOTE_IMAGE}"
#    EOT
#  }
#
#  depends_on = [
#    azurerm_container_registry.main
#  ]
#}
#
#resource "null_resource" "push_frontend_image" {
#  triggers = {
#    local_image   = var.frontend_local_image
#    remote_image  = local.frontend_image
#    password_hash = sha256(azurerm_container_registry.main.admin_password)
#  }
#
#  provisioner "local-exec" {
#    interpreter = ["bash", "-c"]
#    environment = {
#      ACR_LOGIN_SERVER = azurerm_container_registry.main.login_server
#      ACR_USERNAME     = azurerm_container_registry.main.admin_username
#      ACR_PASSWORD     = azurerm_container_registry.main.admin_password
#      LOCAL_IMAGE      = var.frontend_local_image
#      REMOTE_IMAGE     = local.frontend_image
#    }
#
#    command = <<-EOT
#      set -euo pipefail
#
#      if ! command -v docker >/dev/null 2>&1; then
#        echo "Docker CLI not found in PATH." >&2
#        exit 1
#      fi
#
#      if ! docker image inspect "$${LOCAL_IMAGE}" >/dev/null 2>&1; then
#        echo "Local Docker image '$${LOCAL_IMAGE}' not found. Build it before applying Terraform." >&2
#        exit 1
#      fi
#
#      echo "$${ACR_PASSWORD}" | docker login "$${ACR_LOGIN_SERVER}" -u "$${ACR_USERNAME}" --password-stdin
#      docker tag "$${LOCAL_IMAGE}" "$${REMOTE_IMAGE}"
#      docker push "$${REMOTE_IMAGE}"
#    EOT
#  }
#
#  depends_on = [
#    azurerm_container_registry.main
#  ]
#}
