# Subway Remote Order Cosmos DB Collections Creation script

# The script will create Cosmos Account / Database and Collections If it does not exists
#
# The script will required the following Parameters
# 1- ResourceGroup: where the CosmosDB be created
# 2- Location: where the ResourceGroup and CosmosDB will be hosted
# 3- Cosmos Account: CosmosDB account name
# 4- Cosmos Database: ComosDB database name
# 5- Cosmos Collections: CosmosDB collection names separated by comma delimiater i.e col1,col2,col3

param(
      [Parameter(Mandatory=$True)]
      [string]$resourceGroup = $(throw "-resourceGroup is required."),
      [Parameter(Mandatory=$True)]      
      [string]$location  = $(throw "-location is required."),
      [Parameter(Mandatory=$True)]      
      [string]$cosmosAccount  = $(throw "-cosmosAccount is required."),
      [Parameter(Mandatory=$True)]      
      [string]$cosmosDatabase  = $(throw "-cosmosDatabase is required."),
      [Parameter(Mandatory=$True)]      
      [string]$cosmosCollections  = $(throw "-cosmosCollections is required.")
      [Parameter(Mandatory=$True)]      
      [string]$cosmosCollectionsPK  = $(throw "-cosmosCollectionsPK is required.")
)


echo "Logging into Azure Account"
#Connect-AzureRmAccount

echo "Install CosmosDB Modules"
Install-Module -Name CosmosDB

echo "Check if resourceGroup $resourceGroup exists"
Get-AzureRmResourceGroup -Name $resourceGroup -ErrorVariable notPresent -ErrorAction SilentlyContinue
if ($notPresent)
{
    echo "ResourceGroup $resourceGroup at location $location does not exists. Create New Resource Group"
    
    New-AzureRmResourceGroup -Name $resourceGroup -Location $location
}
else
{
   echo "ResourceGroup $resourceGroup exists"
}

echo "Check if CosmosDb Account $cosmosAccount exists"
Get-CosmosDbAccount -Name $cosmosAccount -ResourceGroup $resourceGroup -ErrorVariable notPresent -ErrorAction SilentlyContinue
if ($notPresent)
{
    echo "CosmosDBAccount $cosmosAccount does not. Create new account"
    
    New-CosmosDbAccount -Name $cosmosAccount -ResourceGroup $resourceGroup -Location $location
}
else
{
   echo "CosmosDbAccount $cosmosAccount exists"
}


echo "Retrieve Cosmos primary key and create Cosmos context object"
$cosmosDbContext = New-CosmosDbContext -Account $cosmosAccount  -ResourceGroup $resourceGroup -MasterKeyType 'PrimaryMasterKey'



echo "Check if CosmosDb Database $cosmosDatabase exists"
Get-CosmosDbDatabase -Context $cosmosDbContext -Id $cosmosDatabase -ErrorVariable notPresent -ErrorAction SilentlyContinue
if ($notPresent)
{
    echo "Create new CosmosDb Database $cosmosDatabase"
    New-CosmosDbDatabase -Context $cosmosDbContext -Id $cosmosDatabase
}
else
{
   echo "CosmosDbDatabase $cosmosDatabase exists"
}


echo "Retrieve Cosmos primary key and create Cosmos context object for database $cosmosDatabase"
$cosmosDbContext = New-CosmosDbContext -Account $cosmosAccount -Database $cosmosDatabase -ResourceGroup $resourceGroup -MasterKeyType 'PrimaryMasterKey'

echo "Covert Collections string into array"
$collectionslist = $cosmosCollections.split(",");

echo "Covert Collections string into array"
$pklist = $cosmosCollectionsPK.split(",");

$i=0;
foreach($collection in $collectionslist){ 

    echo "Check if CosmosDb Collection $collection exists"
    Get-CosmosDbCollection -Context $cosmosDbContext -Id $collection -ErrorVariable notPresent -ErrorAction SilentlyContinue 
    if ($notPresent)
    {
        echo "Create new CosmosDb collection $collection"
        New-CosmosDbCollection -Context $cosmosDbContext -Id $collection -PartitionKey $pklist[$i]
    }
    else
    {
        echo "CosmosDb collection $collection exists"    
    }
    $i++;

}