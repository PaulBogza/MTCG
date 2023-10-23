using SWE1.MessageServer.BLL;
using SWE1.MessageServer.HttpServer.Response;
using SWE1.MessageServer.HttpServer.Routing;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.Routing.Messages
{
    internal class ListMessagesCommand : AuthenticatedRouteCommand
    {
        private readonly IMessageManager _messageManager;

        public ListMessagesCommand(IMessageManager messageManager, User identity) : base(identity)
        {
            _messageManager = messageManager;
        }

        public override HttpResponse Execute()
        {
            var messages = _messageManager.ListMessages(Identity).ToList();

            HttpResponse response;

            if (messages.Any())
            {
                var payload = new StringBuilder();
                foreach (var message in messages)
                {
                    payload.Append(message.Id);
                    payload.Append(": ");
                    payload.Append(message.Content);
                    payload.Append('\n');
                }
                response = new HttpResponse(StatusCode.Ok, payload.ToString());
            }
            else
            {
                response = new HttpResponse(StatusCode.NoContent);
            }

            return response;
        }
    }
}
