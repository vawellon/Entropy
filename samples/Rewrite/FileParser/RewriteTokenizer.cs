using Microsoft.Extensions.Primitives;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.FileParser
{
    public static class RewriteTokenizer
    {
        private const char Space = ' ';
        private const char Escape = '\\'; 

        public static List<string> TokenizeRule(string rule)
        {
            if (rule == null || rule == String.Empty)
            {
                return null;
            }
            var context = new TokenContext(rule);
            if (!context.Next())
            {
                return null;
            }

            var tokens = new List<string>();
            context.Mark();
            while (true)
            {
                if (!context.Next())
                {
                    // End of string. Capture.
                    break;
                }
                else if (context.Current == Escape)
                {
                    // Need to progress such that the next character is not evaluated.
                    if (!context.Next())
                    {
                        // Means that a character was not escaped appropriately Ex: "foo\"
                        throw new ArgumentException();
                    }
                }
                else if (context.Current == Space)
                {
                    
                    // time to capture!
                    var token = context.Capture();
                    tokens.Add(token);
                    while (context.Current == '\t' || context.Current == ' ')
                    {
                        if (!context.Next())
                        {
                            // At end of string, we can return at this point.
                            return tokens;
                        }
                    }
                    context.Mark();
                }
            }
            var done = context.Capture();
            tokens.Add(done);
            return tokens;
        }

        private class TokenContext
        {
            private readonly string _template;
            private int _index;
            private int? _mark;

            public TokenContext(string line)
            {
                _template = line;
                _index = -1;
            }
            public char Current
            {
                get { return (_index < _template.Length && _index >= 0) ? _template[_index] : (char)0; }
            }
            public string Error
            {
                get;
                set;
            }
            public bool Back()
            {
                return --_index >= 0;
            }
            public bool Next()
            {
                return ++_index < _template.Length;
            }
            public void Mark()
            {
                _mark = _index;
            }
            public string Capture()
            {
                if (_mark.HasValue)
                {
                    var value = _template.Substring(_mark.Value, _index - _mark.Value);
                    _mark = null;
                    return value;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}
