using System;
using Microsoft.Azure.Documents.Client;
using Microsoft.Extensions.HealthChecks;

namespace TodoService.Api.Extensions
{
    // For more information, visit:
    // - https://docs.microsoft.com/en-us/dotnet/standard/microservices-architecture/implement-resilient-applications/monitor-app-health
    // - https://github.com/dotnet-architecture/HealthChecks
    public static class HealthCheckBuilderCosmosDbExtensions
    {
        public static HealthCheckBuilder AddCosmosDbCheck(this HealthCheckBuilder builder, Uri serviceEndpoint,
            string authKey)
        {
            return AddCosmosDbCheck(builder, serviceEndpoint, authKey, builder.DefaultCacheDuration);
        }

        public static HealthCheckBuilder AddCosmosDbCheck(this HealthCheckBuilder builder, Uri serviceEndpoint,
            string authKey, TimeSpan cacheDuration)
        {
            var checkName = $"CosmosDbCheck({serviceEndpoint})";

            builder.AddCheck(checkName, async () =>
            {
                try
                {
                    using (var documentClient = new DocumentClient(serviceEndpoint, authKey))
                    {
                        await documentClient.OpenAsync();
                        return HealthCheckResult.Healthy($"{checkName}: Healthy");
                    }
                }
                catch (Exception ex)
                {
                    // Failed to connect to CosmosDB.
                    return HealthCheckResult.Unhealthy($"{checkName}: Exception during check: ${ex.Message}");
                }
            }, cacheDuration);

            return builder;
        }
    }
}
