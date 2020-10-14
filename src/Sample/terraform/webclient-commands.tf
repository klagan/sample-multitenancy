resource "null_resource" "set_webclient_user_secrets" {
  provisioner "local-exec" {
    command = <<EOF
              dotnet user-secrets clear --id ${var.webclient_user_secret_id}
              dotnet user-secrets set --id ${var.webclient_user_secret_id} AzureAd:ClientId ${chomp(azuread_application.my_webclient.application_id)}
              dotnet user-secrets set --id ${var.webclient_user_secret_id} AzureAd:TenantId common
              dotnet user-secrets set --id ${var.webclient_user_secret_id} AzureAd:ClientSecret ${azuread_application_password.my_webclient_secret.value}
              az ad app update --id ${azuread_application.my_webclient.application_id} --identifier-uris=api://${azuread_application.my_webclient.application_id}
              dotnet user-secrets set --id ${var.webclient_user_secret_id} WebApi1:ClientId ${azuread_application.my_webapi1.application_id}
              dotnet user-secrets set --id ${var.webclient_user_secret_id} WebApi1:BaseAddress ${var.webapi1_baseaddress}
              dotnet user-secrets set --id ${var.webclient_user_secret_id} WebApi1:PermissionScope \"api://${azuread_application.my_webapi1.application_id}/.default\"
              dotnet user-secrets set --id ${var.webclient_user_secret_id} WebApi2:ClientId ${azuread_application.my_webapi2.application_id}
              dotnet user-secrets set --id ${var.webclient_user_secret_id} WebApi2:BaseAddress ${var.webapi2_baseaddress}
              dotnet user-secrets set --id ${var.webclient_user_secret_id} WebApi2:PermissionScope \"api://${azuread_application.my_webapi2.application_id}/.default\"
              EOF
    interpreter = ["/bin/bash", "-c"]    
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [azuread_application.my_webclient]
}