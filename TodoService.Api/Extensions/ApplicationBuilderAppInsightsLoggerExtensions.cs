// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.using System

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Logging;

namespace TodoService.Api.Extensions
{
    public static class ApplicationBuilderAppInsightsLoggerExtensions
    {
        public static IApplicationBuilder UseAppInsightsLogger(this IApplicationBuilder builder,
            ILoggerFactory loggerFactory)
        {
            loggerFactory.AddApplicationInsights(builder.ApplicationServices, LogLevel.Warning);
            return builder;
        }
    }
}
