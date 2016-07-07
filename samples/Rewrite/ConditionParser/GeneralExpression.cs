// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Rewrite.ConditionParser
{
    public class GeneralExpression : Expression
    {
        public bool Invert { get; set; }
        public ConditionType Type { get; set; }
        public OperationType Operation { get; set; }
    }
    public enum ConditionType
    {
        Regex,
        PropertyTest,
        StringComp,
        IntComp
    }
    public enum OperationType
    {
        None,
        Equal,
        Greater,
        GreaterEqual,
        Less,
        LessEqual,
        NotEqual,
        Directory,
        RegularFile,
        ExistingFile,
        SymbolicLink,
        Size,
        ExistingUrl,
        Executable
    }
}
