using SWE1.MessageServer.HttpServer.Response;
using SWE1.MessageServer.HttpServer.Routing;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.Routing
{
    internal abstract class AuthenticatedRouteCommand : IRouteCommand
    {
        public User Identity { get; init; }

        protected AuthenticatedRouteCommand(User identity)
        {
            Identity = identity;
        }

        public abstract HttpResponse Execute();
    }
}
