resource "azuread_application" "my_webapi2" {
  name                       = var.webapi2_name
  homepage                   = var.webapi2_homepage
  reply_urls                 = var.webapi2_replyurl
  available_to_other_tenants = true
  oauth2_allow_implicit_flow = false
  type                       = "webapp/api"
}

resource "azuread_service_principal" "my_webapi2_service_principal" {
  application_id               = azuread_application.my_webapi2.application_id
  app_role_assignment_required = false

  depends_on = [azuread_application.my_webapi2]
}