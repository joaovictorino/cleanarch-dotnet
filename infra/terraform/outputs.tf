output "resource_group_name" {
  description = "Name of the resource group created for the deployment."
  value       = azurerm_resource_group.main.name
}

output "container_registry_login_server" {
  description = "Login server of the Azure Container Registry where images must be pushed."
  value       = azurerm_container_registry.main.login_server
}

output "container_registry_admin_username" {
  description = "Admin user for the Azure Container Registry."
  value       = azurerm_container_registry.main.admin_username
}

output "container_registry_admin_password" {
  description = "Admin password for the Azure Container Registry."
  value       = azurerm_container_registry.main.admin_password
  sensitive   = true
}

output "backend_url" {
  description = "Internal URL of the backend container app."
  value       = "https://${azurerm_container_app.backend.latest_revision_fqdn}"
}

output "frontend_url" {
  description = "Public URL of the frontend container app."
  value       = "https://${azurerm_container_app.frontend.latest_revision_fqdn}"
}

output "mysql_fqdn" {
  description = "Fully qualified domain name for the MySQL Flexible Server."
  value       = azurerm_mysql_flexible_server.main.fqdn
}

output "mysql_connection_user" {
  description = "User name to connect to MySQL."
  value       = local.mysql_user
}

output "mysql_connection_string" {
  description = "Connection string used by the backend."
  value       = local.mysql_conn
  sensitive   = true
}
