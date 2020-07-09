# Multi-tenancy sample

This sample is a web project that allows sign in from multiple tenants

## Getting started

### Terraform your environment
Terraform is used to build the environment and set the local user secrets.  Unfortunately, because I cant solve a quirk in multiline commands on MacOS I have to create a `local-exec` for each command I want to run.

## Run web project
The web project has two `ServiceCollectionExtension` methods.  One sets up authentication using the traditional methods and the other uses the new `Microsoft.Identity.Web` MSAL preview packages.


#### Notes

Add `Microsoft.Identity.Web` and `Microsoft.Identity.Web.UI` packages to handle authentication and challenge screens respectively

Change the `area` in the `_LoginPartial` partial views from `AzureAd` to `MicrosoftIdentity`.  The `Microsoft.Identity.Web.UI` package is responsible for the challenge screens and uses the `MicrosoftIdentity` MVC area for the login and logout pages.