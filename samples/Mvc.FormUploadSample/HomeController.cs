// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Blob;

namespace Mvc.FormUploadSample
{
    public class HomeController : Controller
    {
        private readonly BlobConfiguration _configuration;

        public HomeController(BlobConfiguration configuration)
        {
            _configuration = configuration;
        }

        public IActionResult Index()
        {
            var filesUploaded = TempData["files-uploaded-count"];
            if (filesUploaded != null)
            {
                ViewData["banner"] = $"{filesUploaded} files successfully uploaded.";
            }

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [NoFormValueProviderFilter]
        public async Task<IActionResult> Upload(IFormFileCollection files, CancellationToken cancellationToken)
        {
            var name = Request.Form["name"];

            var client = new CloudBlobClient(new Uri(_configuration.Endpoint), new StorageCredentials(_configuration.SASKey));
            var container = client.GetContainerReference("uploaded-files");

            var uploadTasks = files.Select(async file =>
            {
                var blobName = name + "_" + Guid.NewGuid().ToString().ToLowerInvariant();
                var blobReference = container.GetBlockBlobReference(blobName);
                using (var outputStream = await blobReference.OpenWriteAsync())
                {
                    await file.CopyToAsync(outputStream, cancellationToken);
                }
            });

            await Task.WhenAll(uploadTasks);
            TempData["files-uploaded-count"] = files.Count;
            return RedirectToAction(nameof(Index));
        }
    }
}
