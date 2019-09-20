---
page_type: sample
languages:
- csharp
- powershell
products:
- azure
description: "This project implements a to-do service using ASP.NET Core. It demonstrates how to use the repository pattern with Azure Cosmos DB SQL API."
urlFragment: repository-pattern-with-azure-cosmos-db-sql-api
---

# Repository Pattern with Azure Cosmos DB SQL API

This project implements a sample to-do service using ASP.NET core. It demonstrates how to use the repository pattern with Azure Cosmos DB SQL API. It also demonstrates other best practices for Azure hosted ASP.NET core web applications, such as logging and telemetry with Application Insights and key/secret management via Azure Key Vault.

### Repository Pattern
This repository is based on the [repository design pattern](https://docs.microsoft.com/en-us/aspnet/core/fundamentals/repository-pattern?view=aspnetcore-2.1) which isolates data access behind interface abstractions. Connecting to the database and manipulating data storage objects is performed through methods provided by the interface's implementation. Consequently, there is no need to call code to deal with database concerns, such as connections, commands, and readers.

Azure Cosmos DB utilizes [partition keys](https://docs.microsoft.com/en-us/azure/cosmos-db/partition-data) to enable quick look ups in unlimited scaled databases. This repository implements the repository design pattern to support partition keys for Cosmos DB. For details on the execution, please see the code within `./TodoService.Infrastructure/Data`.

### Benefits of the Partitioned Repository Pattern Implementation

* Easy reuse of the database access code, because the database communications is centralized in a single place.
* The business domain can be unit tested independent off the database layer.
* Integrated partition key support for large scale Cosmos DB projects.

## Getting Started

### Deploy Azure Resources Using a PowerShell Script
A script for deploying the necessary Cosmos DB resources is located in the **AzureResources.DeploymentScript** folder. The script will check the existence of a resource group, a Cosmos DB account, a Cosmos DB database, and one or more Cosmos DB collections with partition keys. If the defined resources do not exist they will be created by the script.

The following Parameters are inputs to the script:

- **resourceGroup**: where the Cosmos DB instance be created
- **location**: where the ResourceGroup and Cosmos DB will be hosted
- **cosmosAccount**: Cosmos DB account name
- **cosmosDatabase**: Cosmos DB database name
- **cosmosCollections**: Cosmos DB collection names separated by comma delimiter i.e col1,col2,col3
- **cosmosCollectionsPartitionKeys**: Cosmos DB collection partition key names separated by comma delimiter i.e PK1,PK2,PK3

### Prerequisites
 - [ASP.NET Core SDK v2.1.300](https://www.microsoft.com/net/download/thank-you/dotnet-sdk-2.1.300-windows-x64-installer)
 - [ASP.NET Core Runtime 2.1.0](https://www.microsoft.com/net/download/thank-you/dotnet-runtime-2.1.0-windows-hosting-bundle-installer)
 - [Visual Studio 2017 15.7 or newer](https://docs.microsoft.com/en-us/visualstudio/install/update-visual-studio)

### Build and Test
The configuration for this solution depends on secrets, either stored locally or in Azure Key Vault. This is to prevent sensitive information from being stored in a public manner. Please see [SECRET_MANAGEMENT.md](./SECRET_MANAGEMENT.md) for more information on setting up the secret management system.

### Manually Deploying the Solution
The solution can be run locally for development purposes. If you are looking to deploy the solution to an Azure Web App, follow the steps in [MANUAL_DEPLOYMENT.md](./MANUAL_DEPLOYMENT.md)


## Resources

* Repository Design Pattern: https://docs.microsoft.com/en-us/aspnet/core/fundamentals/repository-pattern?view=aspnetcore-2.1
* Azure Free Account : https://azure.microsoft.com/en-us/free/
* Azure App Service : https://azure.microsoft.com/en-us/services/app-service/
* Azure Cosmos DB : https://azure.microsoft.com/en-us/services/cosmos-db/
* Swagger : https://swagger.io/
