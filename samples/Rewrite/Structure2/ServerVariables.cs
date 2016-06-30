using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Rewrite.Structure2
{
    public class ServerVariables
    {
        public string LookupServerVariable(HttpContext context, string variable)
        {
            switch(variable)
            {
                case "HTTP_ACCEPT":
                    return null;
                case "HTTP_COOKIE":
                    return null;
                case "HTTP_FORWARDED":
                    return null;
                case "HTTP_HOST":
                    return null;
                case "HTTP_PROXY_CONNECTION":
                    return null;
                case "HTTP_REFERER":
                    return null;
                case "HTTP_USER_AGENT":
                    return null;
                case "AUTH_TYPE":
                    return null;
                case "CONN_REMOTE_ADDR":
                    return null;
                case "CONTEXT_PREFIX":
                    return null;
                case "CONTEXT_DOCUMENT_ROOT":
                    return null;
                case "IPV6":
                    return null;
                case "PATH_INFO":
                    return null;
                case "QUERY_STRING":
                    return null;
                case "REMOTE_ADDR":
                    return null;
                case "REMOTE_HOST":
                    return null;
                case "REMOTE_IDENT":
                    return null;
                case "REMOTE_PORT":
                    return null;
                case "REMOTE_USER":
                    return null;
                case "REQUEST_METHOD":
                    return null;
                case "SCRIPT_FILENAME":
                    return null;
                case "DOCUMENT_ROOT":
                    return null;
                case "SCRIPT_GROUP":
                    return null;
                case "SCRIPT_USER":
                    return null;
                case "SERVER_ADDR":
                    return null;
                case "SERVER_ADMIN":
                    return null;
                case "SERVER_NAME":
                    return null;
                case "SERVER_PORT":
                    return null;
                case "SERVER_PROTOCOL":
                    return null;
                case "SERVER_SOFTWARE":
                    return null;
                case "TIME_YEAR":
                    return null;
                case "TIME_MON":
                    return null;
                case "TIME_DAY":
                    return null;
                case "TIME_HOUR":
                    return null;
                case "TIME_MIN":
                    return null;
                case "TIME_SEC":
                    return null;
                case "TIME_WDAY":
                    return null;
                case "TIME":
                    return null;
                case "API_VERSION":
                    return null;
                case "HTTPS":
                    return null;
                case "IS_SUBREQ":
                    return null;
                case "REQUEST_FILENAME":
                    return null;
                case "REQUEST_SCHEME":
                    return null;
                case "REQUEST_URI":
                    return null;
                case "THE_REQUEST":
                    return null;
                default:
                    return null;
            }
        }
    }
}
