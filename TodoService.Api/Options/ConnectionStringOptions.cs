using System;

namespace TodoService.Api.Options
{
    public class ConnectionStringOptions
    {
        public Uri ServiceEndpoint { get; set; }
        public string AuthKey { get; set; }

        public void Deconstruct(out Uri serviceEndpoint, out string authKey)
        {
            serviceEndpoint = ServiceEndpoint;
            authKey = AuthKey;
        }
    }
}
