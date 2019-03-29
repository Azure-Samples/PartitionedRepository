// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.using System

using Newtonsoft.Json;

namespace TodoService.Api.Middleware
{
    public class ErrorDetails
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string ErrorType { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
