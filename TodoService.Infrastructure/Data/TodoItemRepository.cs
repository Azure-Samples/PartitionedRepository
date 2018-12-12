using System;
using Microsoft.Azure.Documents;
using TodoService.Core.Interfaces;
using TodoService.Core.Models;

namespace TodoService.Infrastructure.Data
{
    public class TodoItemRepository : CosmosDbRepository<TodoItem> , ITodoItemRepository
    {
        public TodoItemRepository(ICosmosDbClientFactory factory) : base(factory) { }

        public override string CollectionName { get; } = "todoItems";
        public override string GenerateId(TodoItem entity) => $"{entity.Category}:{Guid.NewGuid()}";
        public override PartitionKey ResolvePartitionKey(string entityId) => new PartitionKey(entityId.Split(':')[0]);
    }
}
