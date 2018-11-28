# Application Configuration Settings 
This file describes the application configuration that the TodoService.Api project uses.

The complete logical settings document is listed below:

```JSON
{
  "ApplicationInsights": {
    "InstrumentationKey": "<GUID>"
  },
  "ConnectionStrings": {
    "ConnectionMode": "<Azure | Emulator>",
    "Azure": {
      "AuthKey": "<AUTH_KEY>",
      "ServiceEndpoint": "<COSMOS_DB_ENDPOINT>"
    },
    "Emulator": {
      "AuthKey": "<AUTH_KEY>",
      "ServiceEndpoint": "<COSMOS_DB_EMULATOR_ENDPOINT>"
    }
  },
  "CosmosDb": {
    "DatabaseName": "<COSMOS_DB_NAME>",
    "CollectionNames": [
      {"Name" : "<COLLECTION_NAM>",
	   "PartitionKey" : "<Partition Key Name>"
	  }
    ]
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning"
    }
  },
  "Secrets": {
    "Mode": "<UseMsi | UseKeyVault | UseLocalSecretStore>",
    "KeyVaultUri": "<KEY_VAULT_URI>",
    "ClientId": "<APP_CLIENT_ID>",
    "ClientSecret": "<APP_CLIENT_SECRET>"
  }
}
```
Even though the settings above define the runtime configuration objects of the application, they are not provided by the same file. 

It is not advisable to have settings that contain secrets saved in the appSettings.json file that is checked into source control.

Some configuration settings are removed from the appSettings.json file and moved to a secret store. This is documented in [SECRET_MANAGEMENT.md](./SECRET_MANAGEMENT.md).

At runtime, the configuration system will read several locations and create a memory representation of the document shown above.

This could mean that the same setting is defined in several locations. If that is the case, the order the files are read and the settings overriden is:

1. appSettings.json
1. appSettings.Development.json
1. Secret Store

In particular, the settings that should be moved from appSettings.json (or appSettings.Development.json) to the secret store are:

* ApplicationInsights:InstrumentationKey
* ConnectionStrings:Azure:AuthKey
* ConnectionStrings:Emulator:AuthKey
* Secrets:ClientId
* Secrets:ClientSecret
