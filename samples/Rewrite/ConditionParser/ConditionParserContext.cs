// Copyright (c) .NET Foundation. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace Rewrite.ConditionParser
{
    // TODO rename 
    public class ConditionParserContext
    {
        private readonly string _template;
        private int _index;
        private int? _mark;

        public ConditionParserContext(string condition)
        {
            _template = condition;
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
        public bool HasNext()
        {
            return (_index + 1) < _template.Length;
        }
        public void Mark()
        {
            _mark = _index;
        }
        public string Capture()
        {
            // TODO make caller responsible for buffering token
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
