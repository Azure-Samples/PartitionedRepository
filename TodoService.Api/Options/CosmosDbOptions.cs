// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.using System

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
