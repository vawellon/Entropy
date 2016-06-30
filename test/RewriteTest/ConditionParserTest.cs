using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Rewrite.Structure2;
using Xunit;
using Microsoft.AspNetCore.Testing;

namespace RewriteTest
{
    public class ConditionParserTest
    {
        [Fact]
        public void ConditionParser_SingleServerVariable()
        {
            var serverVar = "%{HTTPS}";
            var result = ConditionParser.ParseCondition(serverVar);

            var expected = new List<ConditionsTestStringSegment>();
            expected.Add(new ConditionsTestStringSegment { Type = StringCondtionType.ServerVariable, Variable = "HTTPS" });
            Assert.True(CompareConditions(result, expected));
        }

        [Fact]
        public void ConditionParser_MultipleServerVariables()
        {
            var serverVar = "%{HTTPS}%{REQUEST_URL}";
            var result = ConditionParser.ParseCondition(serverVar);

            var expected = new List<ConditionsTestStringSegment>();
            expected.Add(new ConditionsTestStringSegment { Type = StringCondtionType.ServerVariable, Variable = "HTTPS" });
            expected.Add(new ConditionsTestStringSegment { Type = StringCondtionType.ServerVariable, Variable = "REQUEST_URL" });
            Assert.True(CompareConditions(result, expected));
        }

        [Fact]
        public void ConditionParser_ParseLiteral()
        {
            var serverVar = "Hello!";
            var result = ConditionParser.ParseCondition(serverVar);

            var expected = new List<ConditionsTestStringSegment>();
            expected.Add(new ConditionsTestStringSegment { Type = StringCondtionType.Literal, Variable = "Hello!" });
            Assert.True(CompareConditions(result, expected));
        }

        [Fact]
        public void ConditionParser_ParseConditionParameters()
        {
            var serverVar = "%1";
            var result = ConditionParser.ParseCondition(serverVar);

            var expected = new List<ConditionsTestStringSegment>();
            expected.Add(new ConditionsTestStringSegment { Type = StringCondtionType.ConditionParameter, Variable = "1" });
            Assert.True(CompareConditions(result, expected));
        }

        [Fact]
        public void ConditionParser_ParseMultipleConditionParameters()
        {
            var serverVar = "%1%2";
            var result = ConditionParser.ParseCondition(serverVar);

            var expected = new List<ConditionsTestStringSegment>();
            expected.Add(new ConditionsTestStringSegment { Type = StringCondtionType.ConditionParameter, Variable = "1" });
            expected.Add(new ConditionsTestStringSegment { Type = StringCondtionType.ConditionParameter, Variable = "2" });
            Assert.True(CompareConditions(result, expected));
        }

        [Fact]
        public void ConditionParser_ParseRuleVariable()
        {
            var serverVar = "$1";
            var result = ConditionParser.ParseCondition(serverVar);

            var expected = new List<ConditionsTestStringSegment>();
            expected.Add(new ConditionsTestStringSegment { Type = StringCondtionType.RuleParameter, Variable = "1" });
            Assert.True(CompareConditions(result, expected));
        }
        [Fact]
        public void ConditionParser_ParseMultipleRuleVariables()
        {
            var serverVar = "$1$2";
            var result = ConditionParser.ParseCondition(serverVar);

            var expected = new List<ConditionsTestStringSegment>();
            expected.Add(new ConditionsTestStringSegment { Type = StringCondtionType.RuleParameter, Variable = "1" });
            expected.Add(new ConditionsTestStringSegment { Type = StringCondtionType.RuleParameter, Variable = "2" });
            Assert.True(CompareConditions(result, expected));
        }

        [Fact]
        public void ConditionParser_ParserComplexRequest()
        {
            var serverVar = "%{HTTPS}/$1";
            var result = ConditionParser.ParseCondition(serverVar);

            var expected = new List<ConditionsTestStringSegment>();
            expected.Add(new ConditionsTestStringSegment { Type = StringCondtionType.ServerVariable, Variable = "HTTPS" });
            expected.Add(new ConditionsTestStringSegment { Type = StringCondtionType.Literal, Variable = "/" });
            expected.Add(new ConditionsTestStringSegment { Type = StringCondtionType.RuleParameter, Variable = "1" });
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
            ExceptionAssert.Throws<ArgumentException>(() => ConditionParser.ParseCondition(testString));
        }
        private bool CompareConditions(List<ConditionsTestStringSegment> list1, List<ConditionsTestStringSegment> list2)
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
