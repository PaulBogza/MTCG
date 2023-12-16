using CardClass;
using PlayerClass;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.DAL;
using SWE1.MessageServer.HttpServer.Response;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.API.Routing.Messages
{
    internal class ShowCardsCommand : AuthenticatedRouteCommand
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
            List<Card>? Stack;
            try
            {
                Stack = DatabaseCardDao.showCards(player);
            }
            catch (MessageNotFoundException)
            {
                Stack = null;
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
