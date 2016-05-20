// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Mvc.FormUploadSample
{
    public class NoFormValueProviderFilterAttribute : Attribute, IResourceFilter
    {
        public void OnResourceExecuted(ResourceExecutedContext context)
        {
            // Do nothing
        }

        public void OnResourceExecuting(ResourceExecutingContext context)
        {
            for (var i = 0; i < context.ValueProviderFactories.Count; i++)
            {
                if (context.ValueProviderFactories[i].GetType() == typeof(FormValueProviderFactory) ||
                    context.ValueProviderFactories[i].GetType() == typeof(JQueryFormValueProviderFactory))
                {
                    context.ValueProviderFactories.RemoveAt(i);
                    i--;
                }
            }
        }
    }
}
