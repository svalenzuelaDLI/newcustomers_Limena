using newcustomers_Limena.Data;
using newcustomers_Limena.Models.API.Properties;
using RestSharp;
using Newtonsoft.Json;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newcustomers_Limena.Helpers.MasterData
{
    public class Properties
    {
        private API APIprincipal = new API();
        public GetEtnias GetEtnias()
        {
            var settings = APIprincipal.GetAPI();
            var auth = APIprincipal.GetAPI_WToken();
            string bearerToken = auth.token;
            var client = new RestClient(settings.IP);
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(bearerToken, "Bearer");
            var request = new RestRequest("/gateway/Customers/Properties/Ethnicities", Method.GET);

            request.AddHeader("cache-control", "no-cache");

            var result = client.Execute(request);
            var jsonResponse = JsonConvert.DeserializeObject<GetEtnias>(result.Content);

            return jsonResponse;
        }
        public GetServices GetServices()
        {
            var settings = APIprincipal.GetAPI();
            var auth = APIprincipal.GetAPI_WToken();
            string bearerToken = auth.token;
            var client = new RestClient(settings.IP);
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(bearerToken, "Bearer");
            var request = new RestRequest("/gateway/Customers/Properties/Services", Method.GET);

            request.AddHeader("cache-control", "no-cache");

            var result = client.Execute(request);
            var jsonResponse = JsonConvert.DeserializeObject<GetServices>(result.Content);

            return jsonResponse;
        }

        public GetTrucks GetTrucks()
        {
            var settings = APIprincipal.GetAPI();
            var auth = APIprincipal.GetAPI_WToken();
            string bearerToken = auth.token;
            var client = new RestClient(settings.IP);
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(bearerToken, "Bearer");
            var request = new RestRequest("/gateway/Customers/Properties/TypesTruck", Method.GET);

            request.AddHeader("cache-control", "no-cache");

            var result = client.Execute(request);
            var jsonResponse = JsonConvert.DeserializeObject<GetTrucks>(result.Content);

            return jsonResponse;
        }

    }
}