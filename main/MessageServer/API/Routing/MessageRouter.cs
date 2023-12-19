using Newtonsoft.Json;
using SWE1.MessageServer.API.Routing.Messages;
using SWE1.MessageServer.API.Routing.Users;
using SWE1.MessageServer.BLL;
using SWE1.MessageServer.HttpServer;
using SWE1.MessageServer.HttpServer.Request;
using SWE1.MessageServer.HttpServer.Response;
using SWE1.MessageServer.HttpServer.Routing;
using SWE1.MessageServer.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpMethod = SWE1.MessageServer.HttpServer.Request.HttpMethod;

namespace SWE1.MessageServer.API.Routing
{
    internal class MessageRouter : IRouter
    {
        private readonly IUserManager _userManager;
        private readonly IMessageManager _messageManager;
        private readonly IdentityProvider _identityProvider;
        private readonly IdRouteParser _routeParser;

        public MessageRouter(IUserManager userManager, IMessageManager messageManager)
        {
            _userManager = userManager;
            _messageManager = messageManager;
            _identityProvider = new IdentityProvider(userManager);
            _routeParser = new IdRouteParser();
        }

        public IRouteCommand? Resolve(HttpRequest request)
        {
            bool isMatch(string path, string route){
                bool matching = false;
                if(path.StartsWith("/users") && route == "users"){
                    matching = _routeParser.IsMatch(path, "/users/{id}");
                }
                else if(path.StartsWith("/tradings") && route == "tradings"){
                    matching = _routeParser.IsMatch(path, "/tradings/{id}");
                }
                return matching;
            }

            int parseId(string path){
                int val = 0;
                if(path.StartsWith("/users")){
                    val = int.Parse(_routeParser.ParseParameters(path, "/users/{id}")["id"]);
                }
                else if(path.StartsWith("/tradings")){
                    val = int.Parse(_routeParser.ParseParameters(path, "/tradings/{id}")["id"]);
                }     
                return val;
            };

            string? parseParameters(string path){
                string? parameter = null;
                if(path.StartsWith("/users")){
                    parameter = _routeParser.ParseParameters(path, "/users/{id}")["id"];
                }
                else if(path.StartsWith("/tradings")){
                    parameter = _routeParser.ParseParameters(path, "/tradings/{id}")["id"];
                }     
                return parameter;
            }

            var checkBody = (string? payload) => payload ?? throw new InvalidDataException();
   
            try
            {
                return request switch
                {
                    { Method: HttpMethod.Post, ResourcePath: "/users" } => new RegisterCommand(_userManager, Deserialize<Credentials>(request.Payload)),
                    { Method: HttpMethod.Post, ResourcePath: "/sessions" } => new LoginCommand(_userManager, Deserialize<Credentials>(request.Payload)),

                    { Method: HttpMethod.Post, ResourcePath: "/messages" } => new AddMessageCommand(_messageManager, GetIdentity(request), checkBody(request.Payload)),
                    { Method: HttpMethod.Get, ResourcePath: "/messages" } => new ListMessagesCommand(_messageManager, GetIdentity(request)),

                    //{ Method: HttpMethod.Get, ResourcePath: var path } when isMatch(path, "messages") => new ShowMessageCommand(_messageManager, GetIdentity(request), parseId(path)),
                    //{ Method: HttpMethod.Put, ResourcePath: var path } when isMatch(path, "messages") => new UpdateMessageCommand(_messageManager, GetIdentity(request), parseId(path), checkBody(request.Payload)),
                    //{ Method: HttpMethod.Delete, ResourcePath: var path } when isMatch(path, "messages") => new RemoveMessageCommand(_messageManager, GetIdentity(request), parseId(path)),

                    { Method: HttpMethod.Get, ResourcePath: var path } when isMatch(path, "users") => new ShowUserCommand(_userManager, GetIdentity(request), parseParameters(path)),
                    { Method: HttpMethod.Put, ResourcePath: var path } when isMatch(path, "users") => new UpdateUserCommand(_userManager, GetIdentity(request), checkBody(request.Payload), parseParameters(path)),

                    _ => null
                };
            }
            catch(InvalidDataException)
            {
                return null;
            }            
        }

        private T Deserialize<T>(string? body) where T : class
        {
            var data = body is not null ? JsonConvert.DeserializeObject<T>(body) : null;
            return data ?? throw new InvalidDataException();
        }

        private User GetIdentity(HttpRequest request)
        {
            return _identityProvider.GetIdentityForRequest(request) ?? throw new RouteNotAuthenticatedException();
        }
    }
}
