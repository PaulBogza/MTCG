using SWE1.MessageServer.BLL;
using SWE1.MessageServer.HttpServer.Response;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.Routing.Messages
{
    internal class AddMessageCommand : AuthenticatedRouteCommand
    {
        private readonly IMessageManager _messageManager;
        private readonly string _message;

        public AddMessageCommand(IMessageManager messageManager, User identity, string message) : base(identity)
        {
            _messageManager = messageManager;
            _message = message;
        }

        public override HttpResponse Execute()
        {
            var message = _messageManager.AddMessage(Identity, _message);
            var response = new HttpResponse(StatusCode.Created, message.Id.ToString());

            return response;
        }
    }

}
