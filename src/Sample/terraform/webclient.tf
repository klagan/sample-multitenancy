resource "azuread_application" "my_webclient" {
  name                       = var.webclient_name
  homepage                   = var.webclient_homepage
  reply_urls                 = var.webclient_replyurl
  available_to_other_tenants = true
  oauth2_allow_implicit_flow = false
  type                       = "webapp/api"

  required_resource_access {
    resource_app_id = azuread_application.my_webapi1.application_id
    resource_access {
      id   = tolist(azuread_application.my_webapi1.oauth2_permissions)[0].id
      type = "Scope"
    }
  }

  required_resource_access {
    # ms graph api
    resource_app_id = "00000003-0000-0000-c000-000000000000"

    # email
    resource_access {
      id   = "64a6cdd6-aab1-4aaf-94b8-3cc8405e90d0"
      type = "Scope"
    }

    # offline access
    resource_access {
      id   = "7427e0e9-2fba-42fe-b0c0-848c9e6a8182"
      type = "Scope"
    }

    # openid
    resource_access {
      id   = "37f7f235-527c-4136-accd-4a02d197296e"
      type = "Scope"
    }

    # profile
    resource_access {
      id   = "14dad69e-099b-42c9-810b-d002981feec1"
      type = "Scope"
    }

    # user.read
    resource_access {
      id   = "e1fe6dd8-ba31-4d61-89e7-88639da4683d"
      type = "Scope"
    }
  }

  depends_on = [azuread_application.my_webapi1]
}

resource "azuread_service_principal" "my_webclient_service_principal" {
  application_id               = azuread_application.my_webclient.application_id
  app_role_assignment_required = false

  depends_on = [azuread_application.my_webclient]
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
