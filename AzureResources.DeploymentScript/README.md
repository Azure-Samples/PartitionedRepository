# Auto Create CosmosDB Collection(s) Script

CosmosDB Collections script is used to check if the collections already exist otherwise the script will create it.
The following resources will be created if it does not exists

- Resource Group
- Cosmos Account
- Cosmos Database
- Cosmos Collections


## Required Paramaters
The following parameters are mandatory parameters:

In order to run the script successfully the following are condition should be met

- **resourceGroup**: where the CosmosDB be created
- **location**: where the ResourceGroup and CosmosDB will be hoste
- **cosmosAccount**: CosmosDB account name
- **cosmosDatabase**: ComosDB database name
- **cosmosCollections**: CosmosDB collection names separated by comma delimiater i.e col1,col2,col3
- **cosmosCollectionsPartitionKeys**: CosmosDB collection PK names separated by comma delimiater i.e PK1,PK2,PK3

## Running the script

The script will be executed in the following format

    

    ./CosmosDeployPS.ps1 -resourceGroup "<myresourcegroup-name>" -location "<mylocation-name>" -cosmosAccount "<mycosmosDBAccount-name>" -cosmosDatabase "<database name>" -cosmosCollections "<cosmos collections name>" - cosmosCollectionsPartitionKeys "<cosmos collections PK names>"
    

In a successful case, we will get the following result


    Scipt is completed successfully.


and by reading $lastexistcode will give 0

       echo $lastexitcode
       0
    
    


    
