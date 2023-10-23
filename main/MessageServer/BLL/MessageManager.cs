using SWE1.MessageServer.DAL;
using SWE1.MessageServer.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace SWE1.MessageServer.BLL
{
    internal class MessageManager : IMessageManager
    {
        private readonly IMessageDao _messageDao;

        public MessageManager(IMessageDao messageDao)
        {
            _messageDao = messageDao;
        }

        public Message AddMessage(User user, string content)
        {
            var message = new Message(content);
            _messageDao.InsertMessage(user.Username, message);

            return message;
        }

        public IEnumerable<Message> ListMessages(User user)
        {
            return _messageDao.GetMessages(user.Username);
        }

        public void RemoveMessage(User user, int messageId)
        {
            if (_messageDao.GetMessageById(user.Username, messageId) != null)
            {
                _messageDao.DeleteMessage(user.Username, messageId);
            }
            else
            {
                throw new MessageNotFoundException();
            }
        }

        public Message ShowMessage(User user, int messageId)
        {
            var message = _messageDao.GetMessageById(user.Username, messageId);
            return message is not null ? message : throw new MessageNotFoundException();
        }

        public void UpdateMessage(User user, int messageId, string content)
        {
            var message = _messageDao.GetMessageById(user.Username, messageId);
            if (message is not null)
            {
                message.Content = content;
                _messageDao.UpdateMessage(user.Username, message);
            }
            else
            {
                throw new MessageNotFoundException();
            }
        }
    }
}
