using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.FileParser
{
    public class RewriteReaderException : Exception
    {
        public RewriteReaderException() { }

        public RewriteReaderException(string message) : base(message) { }

        public RewriteReaderException(string message, Exception inner) : base(message, inner) { }
    }
}