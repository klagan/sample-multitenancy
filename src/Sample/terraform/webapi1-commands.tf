
resource "null_resource" "clear_webapi1_user_secrets" {
  provisioner "local-exec" {
    command = "dotnet user-secrets clear --id ${var.webapi1_user_secret_id}"
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [azuread_application.my_webapi1]
}

resource "null_resource" "set_webapi1_clientid" {
  provisioner "local-exec" {
    command     = "dotnet user-secrets set --id ${var.webapi1_user_secret_id} AzureAd:ClientId ${azuread_application.my_webapi1.application_id}"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.clear_webapi1_user_secrets]
}

#resource "null_resource" "set_webapi_token_version_2" {
#  provisioner "local-exec" {
#    interpreter = ["/bin/bash", "-c"]
#    command     = "sleep 5 && az rest --method PATCH --uri https://graph.microsoft.com/v1.0/applications/${azuread_application.my_webapi1.object_id} --body '{\"api\":{\"requestedAccessTokenVersion\":2}}'"
#  }

#  depends_on = [azuread_application.my_webapi1, azuread_service_principal.my_webapi1_service_principal]
#}

resource "null_resource" "set_webapi1_tenantid" {
  provisioner "local-exec" {
    command     = "dotnet user-secrets set --id ${var.webapi1_user_secret_id} AzureAd:TenantId common"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.set_webapi1_clientid]
}