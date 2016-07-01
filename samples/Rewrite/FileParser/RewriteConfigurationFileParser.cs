using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Rewrite.Structure2;
namespace Rewrite.FileParser
{
    public class RewriteConfigurationFileParser
    {
        private StreamReader _reader;
        public void Parse(Stream input)
        {
            _reader = new StreamReader(input);
            string line = null;
            while ((line = _reader.ReadLine()) != null) {
                List<string> tokens = RewriteTokenizer.TokenizeRule(line);
                if (tokens.Count < 3)
                {
                    // I think this is a syntax error no matter what
                }
                switch(tokens[0])
                {
                    case "RewriteCond":
                        List<ConditionTestStringSegment> matchesForCondition = ConditionParser.ParseConditionTestString(tokens[1]);
                        // parse regex
                        break;
                    case "RewriteRule":
                        // parse regex
                        // then do similar logic to the condition test string replacement
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
        }
    }
}
