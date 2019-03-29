// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.using System

namespace TodoService.Api.Options
{
    public enum KeyVaultUsage
    {
        UseLocalSecretStore,
        UseClientSecret,
        UseMsi
    }

    public class KeyVaultOptions
    {
        public KeyVaultUsage Mode { get; set; }
        public string KeyVaultUri { get; set; }
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }
    }
}
