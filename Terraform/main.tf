provider "azurerm"{
    features {}
}

resource "azurerm_resource_group" "example" {
  name     = "rg-personal-dev-cus-eventhubtraining"
  location = "centralus"

  tags = {
      environment ="Development"
  }
}

resource "azurerm_eventhub_namespace" "example" {
  name                = "eventhubtraining"
  location            = azurerm_resource_group.example.location
  resource_group_name = azurerm_resource_group.example.name
  sku                 = "Standard"
  capacity            = 2

  tags = {
    environment = "Development"
  }
}

resource "azurerm_storage_account" "example" {
  name                     = "sapersonaldevcuseventhub"
  resource_group_name      = azurerm_resource_group.example.name
  location                 = azurerm_resource_group.example.location
  account_tier             = "Standard"
  account_replication_type = "GRS"

  tags = {
    environment = "Development"
  }
}

