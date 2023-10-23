using SWE1.MessageServer.HttpServer.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.HttpServer.Routing
{
    internal interface IRouter
    {
        IRouteCommand? Resolve(HttpRequest request);
    }
}
