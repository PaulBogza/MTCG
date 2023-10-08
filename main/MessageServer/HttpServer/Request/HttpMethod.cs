using System.Net;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System;
using System.Threading.Tasks;

namespace MessageServer.HttpServer.Request{
    enum HttpMethod{
        Get, 
        Post, 
        Put, 
        Delete,
        Patch
    }

    static class MethodUtilities{
        public static HttpMethod GetMethod(string method){
            return method.ToLower() switch{
                "get" => HttpMethod.Get,
                "post" => HttpMethod.Post, 
                "put" => HttpMethod.Put,
                "delete" => HttpMethod.Delete,
                "patch" => HttpMethod.Patch,
                _ => throw new InvalidDataException()
            };
        }
    }
}