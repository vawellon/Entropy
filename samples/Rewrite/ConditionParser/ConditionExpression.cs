using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public enum ConditionType
    {
        Regex, FileTest, StringComp, IntComp
    }
    public enum OperationType
    {
        None, Equal, Greater, GreaterEqual, Less, LessEqual, NotEqual,
        Directory, RegularFile, ExistingFile, SymbolicLink, Size, ExistingUrl, Executable
    }
    public class ConditionExpression : Expression
    {
        public ConditionType Type { get; set; }
        public OperationType Operation { get; set; }
    }
}
