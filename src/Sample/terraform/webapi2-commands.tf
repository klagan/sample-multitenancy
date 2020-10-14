resource "null_resource" "set_webapi2_user_secrets" {
  provisioner "local-exec" {
    command = <<EOF
              dotnet user-secrets clear --id ${var.webapi2_user_secret_id}
              dotnet user-secrets set --id ${var.webapi2_user_secret_id} AzureAd:ClientId ${azuread_application.my_webapi2.application_id}
              dotnet user-secrets set --id ${var.webapi2_user_secret_id} AzureAd:TenantId common
              az ad app update --id ${azuread_application.my_webapi2.application_id} --identifier-uris=api://${azuread_application.my_webapi2.application_id}
              EOF
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [azuread_application.my_webapi2]
}