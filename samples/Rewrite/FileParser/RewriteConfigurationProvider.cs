using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.FileParser
{
    public class RewriteConfigurationProvider : FileConfigurationProvider
    {
        public RewriteConfigurationProvider(RewriteConfigurationSource source) : base(source) { }

        public override void Load(Stream stream)
        {
            var parser = new RewriteConfigurationFileParser();
            try
            {
                Data = parser.Parse(stream);
            }
            catch (RewriteReaderException e) // todo
            {
                string errorLine = string.Empty;
                if (stream.CanSeek)
                {
                    stream.Seek(0, SeekOrigin.Begin);

                    IEnumerable<string> fileContent;
                    using (var streamReader = new StreamReader(stream))
                    {
                        fileContent = ReadLines(streamReader);
                        errorLine = RetrieveErrorContext(e, fileContent);
                    }
                }

                throw new FormatException(Resources.FormatError_JSONParseError(e.LineNumber, errorLine), e);
            }
        }

        private static string RetrieveErrorContext(RewriteReaderException e, IEnumerable<string> fileContent)
        {
            string errorLine;
            if (e.LineNumber >= 2)
            {
                var errorContext = fileContent.Skip(e.LineNumber - 2).Take(2).ToList();
                errorLine = errorContext[0].Trim() + Environment.NewLine + errorContext[1].Trim();
            }
            else
            {
                var possibleLineContent = fileContent.Skip(e.LineNumber - 1).FirstOrDefault();
                errorLine = possibleLineContent ?? string.Empty;
            }

            return errorLine;
        }

        private static IEnumerable<string> ReadLines(StreamReader streamReader)
        {
            string line;
            do
            {
                line = streamReader.ReadLine();
                yield return line;
            } while (line != null);
        }
    }
}
