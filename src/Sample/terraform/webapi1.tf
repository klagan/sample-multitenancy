resource "azuread_application" "my_webapi1" {
  name                       = var.webapi1_name
  homepage                   = var.webapi1_homepage
  reply_urls                 = var.webapi1_replyurl
  identifier_uris            = ["api://kaml/my-webapi1"]
  available_to_other_tenants = false
  oauth2_allow_implicit_flow = false
  type                       = "webapp/api"
}

resource "azuread_service_principal" "my_webapi1_service_principal" {
  application_id               = azuread_application.my_webapi1.application_id
  app_role_assignment_required = false
}