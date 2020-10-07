# Multi-tenancy sample

This sample is a web project that allows sign in from multiple tenants

## Work in Progress

- [X] incorporate api permission in terraform script
- [X] add documentation on providing consent
- [X] use accesstoken to call webapi
- [ ] register webapi with a different tenant (enterprise application)
- [ ] restrict tenant user access to different tenant owned web apis
- [ ] rearrange terraform script to prevent needing to run twice (something messed up in terraform deployment sequence?)
- [ ] add app api id into `knownClientApplications` section of application registration manifest [(here?)](https://docs.microsoft.com/en-us/cli/azure/ad/app?view=azure-cli-latest#az-ad-app-update)
- [ ] provisioning application [(link)](https://docs.microsoft.com/en-us/azure/active-directory/app-provisioning/user-provisioning)
- [ ] set the homepage
- [ ] work on "my notes" section.  it should exist and all notes should be worked into system through automation and/or code
- [ ] add another webapi for different tenant
- [ ] move webapi config out of .json and into a sample inmemory source
- [ ] accessTokenAcceptedVersion => 2 in application manifest (webapp and web api)
- [ ] put secrets into keyvault

## Getting started

### Terraform your environment
Terraform is used to build the environment and set the local user secrets.  Unfortunately, because I cant solve a quirk in multiline commands on MacOS I have to create a `local-exec` for each command I want to run.

### Grant administrator consent
The environment has now been setup and the local environment has been configured using `user secrets`.  We now need to register consent for the application in the tenant.

The application relies on resources in the `API permissions` section of `Application Registrations`.  These need to be consented to by an administrator (or user) in the `Enterprise Application` configuration.

```text
Application Registration --> Managed Application --> Permissions
```
## Run web project
The web project has two `ServiceCollectionExtension` methods.  One sets up authentication using the traditional methods and the other uses the new `Microsoft.Identity.Web` MSAL preview packages.

## Troubleshooting

|Error|Remedy|
|-|-|
|Random issues with `appSettings.json` configuration| This could indicate an issue with the user secrets not being loaded which is because the `ASPNETCORE_ENVIRONMENT` environment variable is not set to `DEVELOPMENT`.  User secrets are not loaded in environments other than development.  This is built into the framework.|
|`MsalUiRequiredException: AADSTS65001: The user or administrator has not consented to use the application with ID`|The application relies on resources in the `API permissions` section of `Application Registrations`.  These need to be consented to by an administrator (or user) in the `Enterprise Application` configuration. (```Application Registration --> Managed Application --> Permissions)```|
|`MsalUiRequiredException: No account or login hint was passed to the AcquireTokenSilent call.`|Try clearing the cookies and trying again.  This message could indicate you are using a stale cookie when changes have been made to authn.  Clearing the cookies and logging in again to generate a new cookie may highlight the true error or fix the problem.|

## My Notes

Add `Microsoft.Identity.Web` and `Microsoft.Identity.Web.UI` packages to handle authentication and challenge screens respectively

Change the `area` in the `_LoginPartial` partial views from `AzureAd` to `MicrosoftIdentity`.  The `Microsoft.Identity.Web.UI` package is responsible for the challenge screens and uses the `MicrosoftIdentity` MVC area for the login and logout pages.

#### Pipeline (middleware)

[Source](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/middleware/?view=aspnetcore-3.1)

This is set up in the `Startup.cs` in this order:

exceptionHandler -> httpsRedirection -> staticFiles -> routing -> authN -> authZ -> myMiddleware -> endpoints (controller etc.)

This order is also used in reverse on the way back unless a short circuit has been introduced

#### What this example currently does

- uses`terraform` to create the cloud resources
- uses`donet user-secrets` to set the client side configuration values in a secret store
- spins up a protected multi tenant web api 
- spins up a protected multi tenant web app
- allows you to login into web app and subsequently call the underlying web service with an `on behalf of` call

#### Tear down 

1. delete enterprise apps from client tenants
2. check service principals on client tenants
3. tear down terraform/home tenant resources

#### Terraform environment

1. run terraform
2. home tenant: knownClientApplications in webapi manifest must include appId of webclient
3. login to application with home tenant credentials
4. consent permissions on appreg -> managed app -> permissions
5. add enterprise application to client tenant : `az ad sp create --id <home tenant webapi appId>` and `az ad sp create --id <home tenant client application appId>` (make sure any old ones are deleted)
6. consent permission in client tenant enterprise applications -> permissions

#### Docker
override files dont override everything.  Use `docker-compose config` to see what the effective result it. eg: ports are concatenated not overriden
