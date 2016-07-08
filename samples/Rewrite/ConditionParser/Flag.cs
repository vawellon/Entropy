using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.ConditionParser
{
    public class Flag
    {
        public FlagType Type { get; set; }
        public string Value { get; set; }
        private static readonly IDictionary<string, FlagType> FlagLookup = new Dictionary<string, FlagType>(StringComparer.OrdinalIgnoreCase) {
            { "b", FlagType.EscapeBackreference},
            { "c", FlagType.Chain },
            { "co", FlagType.Cookie },
            { "dpi", FlagType.DiscardPath },
            { "e", FlagType.Env},
            { "end", FlagType.End },
            { "f", FlagType.Forbidden },
            { "g", FlagType.Gone },
            { "h", FlagType.Handler },
            { "l", FlagType.Last },
            { "n", FlagType.Next },
            { "nc", FlagType.NoCase },
            { "ne", FlagType.NoEscape },
            { "ns", FlagType.NoSubReq },
            { "p", FlagType.Proxy },
            { "pt", FlagType.PassThrough },
            { "qsa", FlagType.QSAppend },
            { "qsd", FlagType.QSDiscard },
            { "qsl", FlagType.QSLast },
            { "r", FlagType.Redirect },
            { "s", FlagType.Skip },
            { "t", FlagType.Type }
            };

        public Flag(FlagType flag) : this(flag, null)
        {
        }

        public Flag(FlagType flag, string value)
        {
            Type = flag;
            Value = value;
        }
        public static FlagType LookupFlag(string flag)
        {
            FlagType res;
            if (!FlagLookup.TryGetValue(flag, out res))
            {
                throw new FormatException("Invalid flag");
            }
            return res;
        }
    }

    public enum FlagType
    {
        EscapeBackreference,
        Chain,
        Cookie,
        DiscardPath,
        Env,
        End,
        Forbidden,
        Gone,
        Handler,
        Last,
        Next,
        NoCase,
        NoEscape,
        NoSubReq,
        Proxy,
        PassThrough,
        QSAppend,
        QSDiscard,
        QSLast,
        Redirect,
        Skip,
        Type
    }
}
