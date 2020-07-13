resource "azuread_application" "my_webapi1" {
  name                       = var.webapi1_name
  homepage                   = var.webapi1_homepage
  reply_urls                 = [var.webapi1_replyurl]
  available_to_other_tenants = true
  oauth2_allow_implicit_flow = true
  type                       = "webapp/api"
}

resource "azuread_service_principal" "my_webapi1_service_principal" {
  application_id               = azuread_application.my_webapi1.application_id
  app_role_assignment_required = false
}

resource "null_resource" "clear_webapi1_user_secrets" {
  provisioner "local-exec" {
    command = "dotnet user-secrets clear --id ${var.webapi1_user_secret_id}"
  }

  triggers = {
    always_run = "${timestamp()}"
  }

  depends_on = [azuread_application.my_webapi1]
}

resource "null_resource" "set_webapi1_clientid" {
  provisioner "local-exec" {
    command = "dotnet user-secrets set --id ${var.webapi1_user_secret_id} AzureAd:ClientId ${azuread_application.my_webapi1.application_id}"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = "${timestamp()}"
  }

  depends_on = [null_resource.clear_webapi1_user_secrets]
}

resource "null_resource" "set_webapi1_tenantid" {
  provisioner "local-exec" {
    command = "dotnet user-secrets set --id ${var.webapi1_user_secret_id} AzureAd:TenantId ${data.azuread_client_config.current.tenant_id}"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = "${timestamp()}"
  }

  depends_on = [null_resource.set_webapi1_clientid]
}

resource "null_resource" "list_webapi1_user_secrets" {
  provisioner "local-exec" {
    command = "dotnet user-secrets list --id ${var.webapi1_user_secret_id}"
  }

  triggers = {
    always_run = "${timestamp()}"
  }

  depends_on = [null_resource.set_webapi1_tenantid]
}
