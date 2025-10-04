output "backend_url" {
  description = "Internal URL of the backend container app."
  value       = "https://${local.backend_fqdn}"
}

output "frontend_url" {
  description = "Public URL of the frontend container app."
  value       = "https://${local.frontend_fqdn}"
}
