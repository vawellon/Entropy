using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using Rewrite.ConditionParser;
namespace RewriteTest
{
    public class ConditionRegexTest
    {
        [Theory]
        [InlineData(">hey", OperationType.Greater, "hey", ConditionType.StringComp)]
        [InlineData("<hey", OperationType.Less, "hey", ConditionType.StringComp)]
        [InlineData(">=hey", OperationType.GreaterEqual, "hey", ConditionType.StringComp)]
        [InlineData("<=hey", OperationType.LessEqual, "hey", ConditionType.StringComp)]
        [InlineData("=hey", OperationType.Equal, "hey", ConditionType.StringComp)]
        public void ConditionParser_CheckStringComp(string condition, OperationType operation, string variable, ConditionType conditionType)
        {
            var results = ConditionRegexParser.ParseCondition(condition);

            var expected = new InvertExpression { Expression = new ConditionExpression { Operation = operation, Type = conditionType, Variable = variable }, Invert = false };
            Assert.True(CompareConditions(results, expected));
        }

        [Fact]
        public void ConditionParser_CheckRegexEqual()
        {
            var condition = @"(.*)";
            var results = ConditionRegexParser.ParseCondition(condition);

            var expected = new InvertExpression { Expression = new ConditionExpression { Type = ConditionType.Regex, Variable = "(.*)" }, Invert = false };
            Assert.True(CompareConditions(results, expected));
        }

        [Theory]
        [InlineData("-d", OperationType.Directory, ConditionType.FileTest)]
        [InlineData("-f", OperationType.RegularFile, ConditionType.FileTest)]
        [InlineData("-F", OperationType.ExistingFile, ConditionType.FileTest)]
        [InlineData("-h", OperationType.SymbolicLink, ConditionType.FileTest)]
        [InlineData("-L", OperationType.SymbolicLink, ConditionType.FileTest)]
        [InlineData("-l", OperationType.SymbolicLink, ConditionType.FileTest)]
        [InlineData("-s", OperationType.Size, ConditionType.FileTest)]
        [InlineData("-U", OperationType.ExistingUrl, ConditionType.FileTest)]
        [InlineData("-x", OperationType.Executable, ConditionType.FileTest)]
        public void ConditionParser_CheckFileOperations(string condition, OperationType operation, ConditionType cond)
        {
            var results = ConditionRegexParser.ParseCondition(condition);

            var expected = new InvertExpression { Expression = new ConditionExpression { Type = cond, Operation = operation }, Invert = false };
            Assert.True(CompareConditions(results, expected));
        }

        [Theory]
        [InlineData("!-d", OperationType.Directory, ConditionType.FileTest)]
        [InlineData("!-f", OperationType.RegularFile, ConditionType.FileTest)]
        [InlineData("!-F", OperationType.ExistingFile, ConditionType.FileTest)]
        [InlineData("!-h", OperationType.SymbolicLink, ConditionType.FileTest)]
        [InlineData("!-L", OperationType.SymbolicLink, ConditionType.FileTest)]
        [InlineData("!-l", OperationType.SymbolicLink, ConditionType.FileTest)]
        [InlineData("!-s", OperationType.Size, ConditionType.FileTest)]
        [InlineData("!-U", OperationType.ExistingUrl, ConditionType.FileTest)]
        [InlineData("!-x", OperationType.Executable, ConditionType.FileTest)]
        public void ConditionParser_CheckFileOperationsInverted(string condition, OperationType operation, ConditionType cond)
        {
            var results = ConditionRegexParser.ParseCondition(condition);

            var expected = new InvertExpression { Expression = new ConditionExpression { Type = cond, Operation = operation }, Invert = true };
            Assert.True(CompareConditions(results, expected));
        }

        [Theory]
        [InlineData("-gt1", OperationType.Greater, "1", ConditionType.IntComp)]
        [InlineData("-lt1", OperationType.Less, "1", ConditionType.IntComp)]
        [InlineData("-ge1", OperationType.GreaterEqual, "1", ConditionType.IntComp)]
        [InlineData("-le1", OperationType.LessEqual, "1", ConditionType.IntComp)]
        [InlineData("-eq1", OperationType.Equal, "1", ConditionType.IntComp)]
        [InlineData("-ne1", OperationType.NotEqual, "1", ConditionType.IntComp)]
        public void ConditionParser_CheckIntComp(string condition, OperationType operation, string variable, ConditionType conditionType)
        {
            var results = ConditionRegexParser.ParseCondition(condition);

            var expected = new InvertExpression { Expression = new ConditionExpression { Operation = operation, Type = conditionType, Variable = variable }, Invert = false };
            Assert.True(CompareConditions(results, expected));
        }

        private bool CompareConditions(InvertExpression i1, InvertExpression i2)
        {
            if (i1.Invert != i2.Invert)
            {
                return false;
            }
            if (!i1.Expression.GetType().Equals(i2.Expression.GetType()))
            {
                return false;
            }
            if (!(i1.Expression is ConditionExpression))
            {
                return false;
            }
            var c1 = (ConditionExpression) i1.Expression;
            var c2 = (ConditionExpression) i1.Expression;
            if (c1.Operation != c2.Operation ||
                c1.Type != c2.Type ||
                c1.Variable != c2.Variable)
            {
                return false;
            }
            return true;
        }
    }
}
