using System.Collections.Generic;

namespace TodoService.Api.Options
{
    public class CosmosDbOptions
    {
        public string DatabaseName { get; set; }
        public List<CollectionInfo> CollectionNames { get; set; }

        public void Deconstruct(out string databaseName, out List<CollectionInfo> collectionNames)
        {
            databaseName = DatabaseName;
            collectionNames = CollectionNames;
        }
    }

    public class CollectionInfo
    {
        public string Name { get; set; }
        public string PartitionKey { get; set; }
    }
}
