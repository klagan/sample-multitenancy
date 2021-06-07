#output "webclient_application_id" {
#  value = azuread_application.my_webclient.application_id
#}

#output "webclient_tenant_id" {
#  value = data.azuread_client_config.current.tenant_id
#}

output "webclient_object_id" {
  value = azuread_application.my_webclient.object_id
}

#output webclient_secret {
#  value = azuread_application_password.my_webclient_secret
#  sensitive = true
#}

output "list_webclient_user_secret_command" {
  value = format("dotnet user-secrets list --id %s", var.webclient_user_secret_id)
}

#output webclient_replyurl {
#  value = var.webclient_replyurl
#}

#output "webapi1_application_id" {
#  value = azuread_application.my_webapi1.application_id
#}

#output "webapi1_tenant_id" {
#  value = data.azuread_client_config.current.tenant_id
#}

output "list_webapi1_user_secret_command" {
  value = format("dotnet user-secrets list --id %s", var.webapi1_user_secret_id)
}

#output webapi1_replyurl {
#  value = var.webapi1_replyurl
#}


output "webapi1_object_id" {
  value = azuread_application.my_webapi1.object_id
}
