// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Mvc.FormUploadSample
{
    public class BlobConfiguration
    {
        public BlobConfiguration(string endpoint, string saskey)
        {
            Endpoint = endpoint;
            SASKey = saskey;
        }

        public string Endpoint { get; }

        public string SASKey { get; }
    }
}
