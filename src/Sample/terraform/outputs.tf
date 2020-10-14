output "webclient_application_id" {
  value = azuread_application.my_webclient.application_id
}

output "webclient_tenant_id" {
  value = data.azuread_client_config.current.tenant_id
}

output "webclient_application_secret" {
  value = azuread_application_password.my_webclient_secret.value
}

output "list_webclient_user_secret_command" {
  value = format("dotnet user-secrets list --id %s", var.webclient_user_secret_id)
}

output webclient_replyurl {
  value = var.webclient_replyurl
}

output "webapi1_application_id" {
  value = azuread_application.my_webapi1.application_id
}

output "webapi1_tenant_id" {
  value = data.azuread_client_config.current.tenant_id
}

output "list_webapi1_user_secret_command" {
  value = format("dotnet user-secrets list --id %s", var.webapi1_user_secret_id)
}

output webapi1_replyurl {
  value = var.webapi1_replyurl
}

output "webapi2_application_id" {
  value = azuread_application.my_webapi2.application_id
}

output "webapi2_tenant_id" {
  value = data.azuread_client_config.current.tenant_id
}

output "list_webapi2_user_secret_command" {
  value = format("dotnet user-secrets list --id %s", var.webapi2_user_secret_id)
}

output webapi2_replyurl {
  value = var.webapi2_replyurl
}

output webapi1_scope {
  value = "api://${azuread_application.my_webapi1.application_id}/.default"
}

output webapi2_scope {
  value = "api://${azuread_application.my_webapi2.application_id}/.default"
}

output NOTES {
  value = "accessTokenAcceptedVersion is not exposed through an API.  This means that you must manually set the value to '2' in the application registration manifest.  Also, update knownclientapplications"
}
