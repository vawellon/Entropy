using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class GeneralExpression
    {
        public string Variable { get; set; }
        public bool Invert { get; set; }
        public ConditionType Type { get; set; }
        public OperationType Operation { get; set; }
    }
    public enum ConditionType
    {
        Regex, FileTest, StringComp, IntComp
    }
    public enum OperationType
    {
        None, Equal, Greater, GreaterEqual, Less, LessEqual, NotEqual,
        Directory, RegularFile, ExistingFile, SymbolicLink, Size, ExistingUrl, Executable
    }
}
