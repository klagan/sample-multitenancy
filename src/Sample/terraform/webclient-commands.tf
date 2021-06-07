resource "null_resource" "clear_webclient_user_secrets" {
  provisioner "local-exec" {
    command = "dotnet user-secrets clear --id ${var.webclient_user_secret_id}"
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [azuread_application.my_webclient]
}

resource "null_resource" "set_webclient_clientid" {
  provisioner "local-exec" {
    command     = "dotnet user-secrets set --id ${var.webclient_user_secret_id} AzureAd:ClientId ${chomp(azuread_application.my_webclient.application_id)}"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.clear_webclient_user_secrets]
}

resource "null_resource" "set_webclient_clientsecret" {
  provisioner "local-exec" {
    command     = "dotnet user-secrets set --id ${var.webclient_user_secret_id} AzureAd:ClientSecret ${azuread_application_password.my_webclient_secret.value}"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.set_webclient_clientid]
}

resource "null_resource" "set_webclient_webapi1_clientid" {
  provisioner "local-exec" {
    command     = "dotnet user-secrets set --id ${var.webclient_user_secret_id} WebApi1:ClientId ${azuread_application.my_webapi1.application_id}"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.set_webclient_clientsecret]
}

resource "null_resource" "set_webclient_webapi1_baseaddress" {
  provisioner "local-exec" {
    command     = "dotnet user-secrets set --id ${var.webclient_user_secret_id} WebApi1:BaseAddress ${var.webapi1_baseaddress}"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.set_webclient_webapi1_clientid]
}

#resource "null_resource" "set_webclient_token_version_2" {
#  provisioner "local-exec" {
#    interpreter = ["/bin/bash", "-c"]
#    command     = "sleep 5 && az rest --method PATCH --uri https://graph.microsoft.com/v1.0/applications/${azuread_application.my_webclient.object_id} --body '{\"api\":{\"requestedAccessTokenVersion\":2}}'"
#  }

#  depends_on = [azuread_service_principal.my_webclient_service_principal, azuread_application.my_webclient]
#}