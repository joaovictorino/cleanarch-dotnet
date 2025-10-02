variable "subscription_id" {
  description = "Azure subscription where the resources will be created."
  type        = string
}

variable "location" {
  description = "Azure region for the deployment (for example, brazilsouth)."
  type        = string
  default     = "brazilsouth"
}

variable "environment" {
  description = "Short environment name (for example, dev, staging, prod)."
  type        = string
  default     = "dev"
}

variable "project_name" {
  description = "Base name used when creating Azure resources."
  type        = string
  default     = "cleanarch"
}

variable "tags" {
  description = "Common resource tags."
  type        = map(string)
  default     = {}
}

variable "mysql_admin_username" {
  description = "Administrator username for the MySQL Flexible Server (without the server suffix)."
  type        = string
  default     = "dbadmin"
}

variable "mysql_admin_password" {
  description = "Administrator password for the MySQL Flexible Server."
  type        = string
  sensitive   = true
}

variable "mysql_sku_name" {
  description = "SKU for the MySQL Flexible Server (for example, B_Standard_B1ms, GP_Standard_D2ds_v4)."
  type        = string
  default     = "B_Standard_B1ms"
}

variable "mysql_storage_gb" {
  description = "Storage capacity for the MySQL Flexible Server (in GB)."
  type        = number
  default     = 32
}

variable "mysql_backup_retention_days" {
  description = "Number of days to retain backups for the MySQL Flexible Server."
  type        = number
  default     = 7
}

variable "mysql_version" {
  description = "Version of MySQL to deploy."
  type        = string
  default     = "8.0.21"
}

variable "mysql_database_name" {
  description = "Name of the application database to create."
  type        = string
  default     = "appdb"
}

variable "mysql_public_access" {
  description = "Allow public network access to the MySQL Flexible Server. If false, you must provide VNet integration manually."
  type        = bool
  default     = true
}

variable "mysql_allow_azure_services" {
  description = "Add a firewall rule that allows other Azure services to reach the MySQL Flexible Server."
  type        = bool
  default     = true
}

variable "mysql_firewall_rules" {
  description = "Optional list of firewall rules (name/start/end) for the MySQL Flexible Server when public access is enabled."
  type = list(object({
    name     = string
    start_ip = string
    end_ip   = string
  }))
  default = []
}

variable "backend_image_name" {
  description = "Name of the backend image stored in the Azure Container Registry."
  type        = string
  default     = "backend"
}

variable "backend_image_tag" {
  description = "Tag of the backend image stored in the Azure Container Registry."
  type        = string
  default     = "latest"
}

variable "backend_container_port" {
  description = "Container port exposed by the backend application."
  type        = number
  default     = 8080
}

variable "backend_min_replicas" {
  description = "Minimum number of backend replicas."
  type        = number
  default     = 1
}

variable "backend_max_replicas" {
  description = "Maximum number of backend replicas."
  type        = number
  default     = 5
}

variable "backend_cpu" {
  description = "CPU requested by the backend container (in vCPU)."
  type        = number
  default     = 0.5
}

variable "backend_memory" {
  description = "Memory requested by the backend container (in GiB)."
  type        = number
  default     = 1.0
}

variable "frontend_image_name" {
  description = "Name of the frontend image stored in the Azure Container Registry."
  type        = string
  default     = "frontend"
}

variable "frontend_image_tag" {
  description = "Tag of the frontend image stored in the Azure Container Registry."
  type        = string
  default     = "latest"
}

variable "frontend_container_port" {
  description = "Container port exposed by the frontend application."
  type        = number
  default     = 3000
}

variable "frontend_min_replicas" {
  description = "Minimum number of frontend replicas."
  type        = number
  default     = 1
}

variable "frontend_max_replicas" {
  description = "Maximum number of frontend replicas."
  type        = number
  default     = 5
}

variable "frontend_cpu" {
  description = "CPU requested by the frontend container (in vCPU)."
  type        = number
  default     = 0.25
}

variable "frontend_memory" {
  description = "Memory requested by the frontend container (in GiB)."
  type        = number
  default     = 0.5
}

variable "github_repository" {
  description = "Optional owner/repo identifier used with the gh CLI. Leave empty to target the current repository."
  type        = string
  default     = ""
}

variable "backend_local_image" {
  description = "Local Docker image reference (name:tag) that should be pushed as the backend image."
  type        = string
  default     = "backend:latest"
}

variable "frontend_local_image" {
  description = "Local Docker image reference (name:tag) that should be pushed as the frontend image."
  type        = string
  default     = "frontend:latest"
}
