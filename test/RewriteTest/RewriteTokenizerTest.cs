// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Rewrite.FileParser;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace RewriteTest
{
    public class RewriteTokenizerTest
    {
        [Fact]
        public void Tokenize_RewriteCondtion()
        {
            var testString = "RewriteCond %{HTTPS} !-f";
            var tokens = RewriteTokenizer.TokenizeRule(testString);

            var expected = new List<string>();
            expected.Add("RewriteCond");
            expected.Add("%{HTTPS}");
            expected.Add("!-f");
            Assert.True(tokens.SequenceEqual(expected));
        }

        [Fact]
        public void Tokenize_CheckEscapedSpaceIgnored()
        {
            // TODO need consultation on escape characters.
            var testString = @"RewriteCond %{HTTPS}\ what !-f";
            var tokens = RewriteTokenizer.TokenizeRule(testString);

            var expected = new List<string>();
            expected.Add("RewriteCond");
            expected.Add(@"%{HTTPS}\ what"); // TODO maybe just have the space here? talking point
            expected.Add("!-f");
            Assert.True(tokens.SequenceEqual(expected));
        }
    }
}
