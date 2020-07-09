output "webclient_application_id" {
  value = azuread_application.my_webclient.application_id
}

output "webclient_tenant_id" {
  value = data.azuread_client_config.current.tenant_id
}

output "webclient_user_secret_id" {
  value = var.webclient_user_secret_id
}

output webclient_replyurl {
  value = var.webclient_replyurl
}