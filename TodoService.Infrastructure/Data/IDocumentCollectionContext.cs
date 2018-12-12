using Microsoft.Azure.Documents;
using TodoService.Core.Models;

namespace TodoService.Infrastructure.Data
{
    public interface IDocumentCollectionContext<in T> where T : Entity
    {
        string CollectionName { get; }

        string GenerateId(T entity);

        PartitionKey ResolvePartitionKey(string entityId);
    }
}
