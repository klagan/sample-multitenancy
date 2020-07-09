resource "azuread_application" "my_webclient" {
  name                       = var.webclient_name
  homepage                   = var.webclient_homepage
  reply_urls                 = [var.webclient_replyurl]
  available_to_other_tenants = true
  oauth2_allow_implicit_flow = true
  type                       = "webapp/api"
}

resource "azuread_service_principal" "my_webclient_service_principal" {
  application_id               = azuread_application.my_webclient.application_id
  app_role_assignment_required = false
}

data "azuread_client_config" "current" {
}

resource "azuread_application_password" "my_webclient_secret" {
  application_object_id = azuread_application.my_webclient.object_id
  value                 = uuid()
  end_date              = "2099-01-01T01:02:03Z"
  lifecycle {
    ignore_changes = [
      value,
    ]
  }
  depends_on = [azuread_application.my_webclient]
}

resource "null_resource" "clear_webclient_user_secrets" {
  provisioner "local-exec" {
    command = "dotnet user-secrets clear --id ${var.webclient_user_secret_id}"
  }

  triggers = {
    always_run = "${timestamp()}"
  }

  depends_on = [azuread_application.my_webclient]
}

resource "null_resource" "set_webclient_clientid" {
  provisioner "local-exec" {
    command = "dotnet user-secrets set --id ${var.webclient_user_secret_id} AzureAd:ClientId ${chomp(azuread_application.my_webclient.application_id)}"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = "${timestamp()}"
  }

  depends_on = [null_resource.clear_webclient_user_secrets]
}

resource "null_resource" "set_webclient_clientsecret" {
  provisioner "local-exec" {
    command = "dotnet user-secrets set --id ${var.webclient_user_secret_id} AzureAd:ClientSecret ${azuread_application_password.my_webclient_secret.value}"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = "${timestamp()}"
  }

  depends_on = [null_resource.set_webclient_clientid]
}

resource "null_resource" "list_webclient_user_secrets" {
  provisioner "local-exec" {
    command = "dotnet user-secrets list --id ${var.webclient_user_secret_id}"
  }

  triggers = {
    always_run = "${timestamp()}"
  }

  depends_on = [null_resource.set_webclient_clientsecret]
}
