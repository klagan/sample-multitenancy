output "set_environment_command" {
 value = <<EOF
export \
MultiTenant_Client_ASPNETCORE_URLS=http://+:8080 \
MultiTenant_Client_ClientId=${azuread_application.my_webclient.application_id} \
MultiTenant_Client_ClientSecret=${azuread_application_password.my_webclient_secret.value} \
MultiTenant_Client_TenantId=common \
MultiTenant_Client_DOTNET_USE_POLLING_FILE_WATCHER=true \
MultiTenant_Client_ASPNETCORE_ENVIRONMENT=Development \
MultiTenant_WebApi_ASPNETCORE_URLS=http://+:8080 \
MultiTenant_WebApi_ClientId=${azuread_application.my_webapi1.application_id} \
MultiTenant_WebApi_TenantId=common \
MultiTenant_WebApi_Domain=laganlabs.it \
MultiTenant_WebApi_BaseAddress=${var.webapi1_baseaddress} \
MultiTenant_WebApi_PermissionScope="api://${azuread_application.my_webapi1.application_id}/.default" \
MultiTenant_WebApi_ASPNETCORE_ENVIRONMENT=Development
EOF
}