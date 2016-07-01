using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rewrite.Structure2;
using Xunit;

namespace RewriteTest
{
    public class RewriteTokenizerTest
    {
        [Fact]
        public void Tokenize_RewriteCondtion()
        {
            var testString = "RewriteCond %{HTTPS} !-f";
            var tokens = new RewriteTokenizer(testString);

            var expected = new List<string>();
            expected.Add("RewriteCond");
            expected.Add("%{HTTPS}");
            expected.Add("!-f");
            Assert.True(tokens._tokens.SequenceEqual(expected));
        }

        [Fact]
        public void Tokenize_CheckEscapedSpaceIgnored()
        {
            // TODO need consultation on escape characters.
            var testString = @"RewriteCond %{HTTPS}\ what !-f";
            var tokens = new RewriteTokenizer(testString);
            
            var expected = new List<string>();
            expected.Add("RewriteCond");
            expected.Add(@"%{HTTPS}\ what"); // TODO maybe just have the space here? talking point
            expected.Add("!-f");
            Assert.True(tokens._tokens.SequenceEqual(expected));
        }
    }
}
