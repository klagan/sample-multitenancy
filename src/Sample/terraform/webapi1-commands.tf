

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
    command = "dotnet user-secrets set --id ${var.webapi1_user_secret_id} AzureAd:ClientId ${azuread_application.my_webapi1.application_id}"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.clear_webapi1_user_secrets]
}

//resource "null_resource" "set_webapi1_tenantid" {
//  provisioner "local-exec" {
//    command = "dotnet user-secrets set --id ${var.webapi1_user_secret_id} AzureAd:TenantId ${data.azuread_client_config.current.tenant_id}"
//    interpreter = ["/bin/bash", "-c"]
//  }
//
//  triggers = {
//    always_run = "${timestamp()}"
//  }
//
//  depends_on = [null_resource.set_webapi1_clientid]
//}

resource "null_resource" "set_webapi1_tenantid" {
  provisioner "local-exec" {
    command = "dotnet user-secrets set --id ${var.webapi1_user_secret_id} AzureAd:TenantId common"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.set_webapi1_clientid]
}