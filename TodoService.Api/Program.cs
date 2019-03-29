// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.using System

using System;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Azure.KeyVault;
using Microsoft.Azure.Services.AppAuthentication;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Configuration.AzureKeyVault;
using TodoService.Api.Options;

namespace TodoService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateWebHostBuilder(args).Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((ctx, builder) =>
                {
                    var config = builder.Build();
                    var mode = (KeyVaultUsage)Enum.Parse(typeof(KeyVaultUsage), (config["Secrets:Mode"]));
                    if (mode != KeyVaultUsage.UseLocalSecretStore)
                    {
                        KeyVaultOptions kvc = config.GetSection("Secrets").Get<KeyVaultOptions>();
                        if (mode == KeyVaultUsage.UseClientSecret)
                        {
                            builder.AddAzureKeyVault(kvc.KeyVaultUri, kvc.ClientId, kvc.ClientSecret);
                        }
                        else //UseMsi
                        {
                            var tokenProvider = new AzureServiceTokenProvider();
                            //Create the Key Vault client
                            var kvClient = new KeyVaultClient((authority, resource, scope) => tokenProvider.KeyVaultTokenCallback(authority, resource, scope));
                            //Add Key Vault to configuration pipeline
                            builder.AddAzureKeyVault(kvc.KeyVaultUri, kvClient, new DefaultKeyVaultSecretManager());
                        }
                    }
                })
                .UseHealthChecks("/")
                .UseApplicationInsights()
                .UseStartup<Startup>();
    }
}
