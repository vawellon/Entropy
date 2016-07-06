using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.FileProviders;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.FileParser
{
    public static class RewriteConfigurationExtensions
    {

        public static void AddRewriteFile(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException(nameof(path));
            }
            Stream stream = File.Open(path, FileMode.Open);
            RewriteConfigurationFileParser.Parse(stream);
        }
    }
}
