### web client
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

resource "null_resource" "set_webclient_tenantid" {
  provisioner "local-exec" {
    command     = "dotnet user-secrets set --id ${var.webclient_user_secret_id} AzureAd:TenantId common"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.set_webclient_clientid]
}

resource "null_resource" "set_webclient_clientsecret" {
  provisioner "local-exec" {
    command     = "dotnet user-secrets set --id ${var.webclient_user_secret_id} AzureAd:ClientSecret ${azuread_application_password.my_webclient_secret.value}"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.set_webclient_tenantid]
}

resource "null_resource" "set_webclient_identifier_uri" {
  provisioner "local-exec" {
    command     = "az ad app update --id ${azuread_application.my_webclient.application_id} --identifier-uris=\"api://${azuread_application.my_webclient.application_id}\""
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.set_webclient_webapi1_clientid]
}


### webapi 1
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

resource "null_resource" "set_webclient_webapi1_scope" {
  provisioner "local-exec" {
    command     = "dotnet user-secrets set --id ${var.webclient_user_secret_id} WebApi1:PermissionScope \"api://${azuread_application.my_webapi1.application_id}/.default\""
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.set_webclient_identifier_uri]
}



### webapi 2
resource "null_resource" "set_webclient_webapi2_clientid" {
  provisioner "local-exec" {
    command     = "dotnet user-secrets set --id ${var.webclient_user_secret_id} WebApi2:ClientId ${azuread_application.my_webapi2.application_id}"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.set_webclient_clientsecret]
}

resource "null_resource" "set_webclient_webapi2_baseaddress" {
  provisioner "local-exec" {
    command     = "dotnet user-secrets set --id ${var.webclient_user_secret_id} WebApi2:BaseAddress ${var.webapi2_baseaddress}"
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.set_webclient_webapi2_clientid]
}

resource "null_resource" "set_webclient_webapi2_scope" {
  provisioner "local-exec" {
    command     = "dotnet user-secrets set --id ${var.webclient_user_secret_id} WebApi2:PermissionScope \"api://${azuread_application.my_webapi2.application_id}/.default\""
    interpreter = ["/bin/bash", "-c"]
  }

  triggers = {
    always_run = timestamp()
  }

  depends_on = [null_resource.set_webclient_identifier_uri]
}
