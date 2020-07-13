# terraform {
#   backend "azurerm" {}
# }

provider "azurerm" {
  version = "~> 2.1"
  features {}
}

provider "azuread" {
  version = "~> 0.7"
}