# Multi-tenancy sample

This sample is a web project that allows sign in from multiple tenants

## Getting started

### Terraform your environment
Terraform is used to build the environment and set the local user secrets.  Unfortunately, because I cant solve a quirk in multiline commands on MacOS I have to create a `local-exec` for each command I want to run.

The environment has now been setup anf the local environment has been configured using `user secrets`.  We now need to register consent for the application in the tenant.

The application relies on resources in the `API permissions` section of `Application Registrations`.  These need to be consented to by an administrator (or user) in the `Enterprise Application` configuration.

```text
Application Registration --> Managed Application --> Permissions
```
## Run web project
The web project has two `ServiceCollectionExtension` methods.  One sets up authentication using the traditional methods and the other uses the new `Microsoft.Identity.Web` MSAL preview packages.


#### Notes

Add `Microsoft.Identity.Web` and `Microsoft.Identity.Web.UI` packages to handle authentication and challenge screens respectively

Change the `area` in the `_LoginPartial` partial views from `AzureAd` to `MicrosoftIdentity`.  The `Microsoft.Identity.Web.UI` package is responsible for the challenge screens and uses the `MicrosoftIdentity` MVC area for the login and logout pages.

#### Troubleshooting

|Error|Remedy|
|-|-|
|`MsalUiRequiredException: AADSTS65001: The user or administrator has not consented to use the application with ID`|The application relies on resources in the `API permissions` section of `Application Registrations`.  These need to be consented to by an administrator (or user) in the `Enterprise Application` configuration.
Follow: `Application Registration` --> `Managed Application` --> Permissions|
|`MsalUiRequiredException: No account or login hint was passed to the AcquireTokenSilent call.`|Try clearing the cookies and trying again.  This message could indicate you are using a stale cookie when changes have been made to authn.  Clearing the cookies and logging in again to generate a new cookie may highlight the true error or fix the problem.|