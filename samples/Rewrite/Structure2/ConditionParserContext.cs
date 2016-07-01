using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
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
        public void Mark()
        {
            _mark = _index;
        }
        public string Capture()
        {
            if (_mark.HasValue)
            {
                var value = _template.Substring(_mark.Value, _index - _mark.Value + 1);
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
