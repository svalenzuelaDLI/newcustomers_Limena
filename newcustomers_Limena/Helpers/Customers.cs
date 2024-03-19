using newcustomers_Limena.Data;
using newcustomers_Limena.Models;
using Newtonsoft.Json;
using RestSharp;
using RestSharp.Authenticators;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;

namespace newcustomers_Limena.Helpers
{
    public class Customers
    {
        private API APIprincipal = new API();

        public IRestResponse PostCustomer(Customers_POST.Customer Customer)
        {
            var settings = APIprincipal.GetAPI();

            string bearerToken = settings.token;
            var client = new RestClient(settings.IP);
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(bearerToken, "Bearer");
            var request = new RestRequest("/gateway/Customers/Customers/AppLimena", Method.POST);
            Customer.userId = "1";
            Customer.country = "US";
            Customer.fatherCard = "";
            Customer.priceList = -1;
            if (Customer.resalesTaxCertificate == "")
            {
                Customer.resalesTaxCertificate = "-";
            }
            if (Customer.siteWeb == "")
            {
                Customer.siteWeb = "-";
            }
            request.RequestFormat = DataFormat.Json;
            request.AddJsonBody(Customer);

            var result = client.Execute(request);

            return result;
        }


        public Customers_POST.postCustomerDocument PostCustomerDocument(string image)
        {
            var settings = APIprincipal.GetAPI();

            string bearerToken = settings.token;
            var client = new RestClient(settings.IP);
            client.Authenticator = new OAuth2AuthorizationRequestHeaderAuthenticator(bearerToken, "Bearer");
            var request = new RestRequest("/gateway/Customers/Customers/AppLimena/Images", Method.POST);

            request.AddFile("ResalesTaxCertificateImage", image);
            request.AddFile("FederalTaxImge", image);

            request.AlwaysMultipartFormData = true;

            var result = client.Execute(request);
            var jsonResponse = JsonConvert.DeserializeObject<Customers_POST.postCustomerDocument>(result.Content);
            return jsonResponse;
       
        }

    }
}