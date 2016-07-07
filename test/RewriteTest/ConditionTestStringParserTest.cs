// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.
using Microsoft.AspNetCore.Testing;
using Rewrite.ConditionParser;
using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
namespace RewriteTest
{
    public class ConditionTestStringParserTest
    {
        [Fact]
        public void ConditionParser_SingleServerVariable()
        {
            var serverVar = "%{HTTPS}";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var expected = new List<ConditionTestStringSegment>();
            expected.Add(new ConditionTestStringSegment { Type = TestStringType.ServerParameter, Variable = "HTTPS" });
            Assert.True(CompareConditions(result, expected));
        }

        [Fact]
        public void ConditionParser_MultipleServerVariables()
        {
            var serverVar = "%{HTTPS}%{REQUEST_URL}";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var expected = new List<ConditionTestStringSegment>();
            expected.Add(new ConditionTestStringSegment { Type = TestStringType.ServerParameter, Variable = "HTTPS" });
            expected.Add(new ConditionTestStringSegment { Type = TestStringType.ServerParameter, Variable = "REQUEST_URL" });
            Assert.True(CompareConditions(result, expected));
        }

        [Fact]
        public void ConditionParser_ParseLiteral()
        {
            var serverVar = "Hello!";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var expected = new List<ConditionTestStringSegment>();
            expected.Add(new ConditionTestStringSegment { Type = TestStringType.Literal, Variable = "Hello!" });
            Assert.True(CompareConditions(result, expected));
        }

        [Fact]
        public void ConditionParser_ParseConditionParameters()
        {
            var serverVar = "%1";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var expected = new List<ConditionTestStringSegment>();
            expected.Add(new ConditionTestStringSegment { Type = TestStringType.ConditionParameter, Variable = "1" });
            Assert.True(CompareConditions(result, expected));
        }

        [Fact]
        public void ConditionParser_ParseMultipleConditionParameters()
        {
            var serverVar = "%1%2";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var expected = new List<ConditionTestStringSegment>();
            expected.Add(new ConditionTestStringSegment { Type = TestStringType.ConditionParameter, Variable = "1" });
            expected.Add(new ConditionTestStringSegment { Type = TestStringType.ConditionParameter, Variable = "2" });
            Assert.True(CompareConditions(result, expected));
        }

        [Fact]
        public void ConditionParser_ParseRuleVariable()
        {
            var serverVar = "$1";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var expected = new List<ConditionTestStringSegment>();
            expected.Add(new ConditionTestStringSegment { Type = TestStringType.RuleParameter, Variable = "1" });
            Assert.True(CompareConditions(result, expected));
        }
        [Fact]
        public void ConditionParser_ParseMultipleRuleVariables()
        {
            var serverVar = "$1$2";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var expected = new List<ConditionTestStringSegment>();
            expected.Add(new ConditionTestStringSegment { Type = TestStringType.RuleParameter, Variable = "1" });
            expected.Add(new ConditionTestStringSegment { Type = TestStringType.RuleParameter, Variable = "2" });
            Assert.True(CompareConditions(result, expected));
        }

        [Fact]
        public void ConditionParser_ParserComplexRequest()
        {
            var serverVar = "%{HTTPS}/$1";
            var result = ConditionTestStringParser.ParseConditionTestString(serverVar);

            var expected = new List<ConditionTestStringSegment>();
            expected.Add(new ConditionTestStringSegment { Type = TestStringType.ServerParameter, Variable = "HTTPS" });
            expected.Add(new ConditionTestStringSegment { Type = TestStringType.Literal, Variable = "/" });
            expected.Add(new ConditionTestStringSegment { Type = TestStringType.RuleParameter, Variable = "1" });
            Assert.True(CompareConditions(result, expected));
        }

        [Theory]
        [InlineData(@"%}")] // no } at end
        [InlineData(@"%{")] // no closing }
        [InlineData(@"%a")] // invalid character after %
        [InlineData(@"$a")] // invalid character after $
        [InlineData(@"%{asdf")] // no closing } with characters
        public void ConditionParser_Bad(string testString)
        {
            ExceptionAssert.Throws<ArgumentException>(() => ConditionTestStringParser.ParseConditionTestString(testString));
        }
        private bool CompareConditions(List<ConditionTestStringSegment> list1, List<ConditionTestStringSegment> list2)
        {
            if (list1.Count != list2.Count)
            {
                return false;
            }
            for (var i = 0; i < list1.Count; i++)
            {
                if (list1.ElementAt(i).Type != list2.ElementAt(i).Type || list1.ElementAt(i).Variable != list2.ElementAt(i).Variable)
                {
                    return false;
                }
            }
            return true;
        }
    }
}
