resource "azuread_application" "my_webapi1" {
  name                       = var.webapi1_name
  homepage                   = local.wa1_homepage
  reply_urls                 = var.webapi1_replyurl
  available_to_other_tenants = true
  oauth2_allow_implicit_flow = false
  prevent_duplicate_names    = false
  type                       = "webapp/api"
}

resource "azuread_service_principal" "my_webapi1_service_principal" {
  application_id               = azuread_application.my_webapi1.application_id
  app_role_assignment_required = false

  depends_on = [azuread_application.my_webapi1]
}