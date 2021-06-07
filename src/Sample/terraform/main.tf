data "azuread_client_config" "current" {
}

#module kam {
#source = "./commands"

#webapi1_user_secret_id = var.webapi1_user_secret_id
#webapi1_application_id = azuread_application.my_webapi1.application_id
#webapi1_object_id = azuread_application.my_webapi1.object_id
#}