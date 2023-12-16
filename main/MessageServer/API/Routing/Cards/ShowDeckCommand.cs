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
    internal class ShowDeckCommand : AuthenticatedRouteCommand
    {
        private readonly IMessageManager _messageManager;
        private readonly int _messageId;

        public ShowMessageCommand(IMessageManager messageManager, User identity, int messageId) : base(identity)
        {
            _messageId = messageId;
            _messageManager = messageManager;
        }
        public override HttpResponse Execute()
        {
            Message? message;
            try
            {
                message = _messageManager.ShowMessage(Identity, _messageId);
            }
            catch (MessageNotFoundException)
            {
                message = null;
            }

            HttpResponse response;
            if (message == null)
            {
                response = new HttpResponse(StatusCode.NotFound);
            }
            else
            {
                response = new HttpResponse(StatusCode.Ok, message.Content);
            }

            return response;
        }

    }
}