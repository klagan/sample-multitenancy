terraform {
  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 2.1"
    }
    azuread = {
      source  = "hashicorp/azuread"
      version = "~> 1.1.1"
    }
    null = {
      source  = "hashicorp/null"
      version = "~> 3.1.0"
    }
  }
}
provider "azurerm" {
  features {}
}
