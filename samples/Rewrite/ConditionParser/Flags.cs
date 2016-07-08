using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class Flags
    {
        private IEnumerable<Flag> FlagList { get; }
        public Flags(IEnumerable<Flag> flags)
        {
            FlagList = flags;
        }
    }
}
