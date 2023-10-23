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
    internal class UpdateMessageCommand : AuthenticatedRouteCommand
    {
        private readonly IMessageManager _messageManager;
        private readonly string _content;
        private readonly int _messageId;

        public UpdateMessageCommand(IMessageManager messageManager, User identity, int messageId, string content) : base(identity)
        {
            _messageId = messageId;
            _content = content;
            _messageManager = messageManager;
        }

        public override HttpResponse Execute()
        {
            HttpResponse response;
            try
            {
                _messageManager.UpdateMessage(Identity, _messageId, _content);
                response = new HttpResponse(StatusCode.Ok);
            }
            catch (MessageNotFoundException)
            {
                response = new HttpResponse(StatusCode.NotFound);
            }

            return response;
        }
    }

}
