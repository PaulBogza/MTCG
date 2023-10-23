using SWE1.MessageServer.HttpServer.Response;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.HttpServer.Routing
{
    internal interface IRouteCommand
    {
        HttpResponse Execute();
    }
}
