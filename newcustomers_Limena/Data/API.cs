using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace newcustomers_Limena.Data
{
    public class API
    {
        private string TOKEN = string.Format("XuTg/WNI0WItokbnkiMWlDJTzUiqcm4Zzk32gJUsFRhR1iRbrDh6X2WerjTgkdPvMsUtP3+s9SkoVqK+fo+uhwy5D/YLmbKgCB8UUuFXCnMBexL31pgXbgjfPo0ogCSwXmOgxULa6ax+VK8RzwtjRVGV84O9l7ZT3DMRFbtelQT1lEXq2E+mvcpAo/XVSN7jw7WFWA/O6CFkp9D3m13e6SBrFRdJ/G8/FV2ZKFK17hwvYJS4YyDZ7QJC3l6wJ8X0xYKG8yBdtkHhZzS70hx2v2ppLeWAbEy8SDdjg6ZXW6CRtuQgpIPhPR64hQwJQ0ZPq8yXuPvyVMKwOJolSiTHljN3jxB2mFMeUiuvFycqajF1SAkA7Mu5klVnNen/55USN4S1B0suHo5OIJ3m1gSSpU4/oETmOv5uoMjqvsCdWLZ4Je9gjAvgORN+DUgYbtaa"); //string.Format("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");

        private string IP = ConfigurationManager.AppSettings["API_principal"];
        private string IP_Auth = ConfigurationManager.AppSettings["API_auth"];
        public class APISettings
        {
            public string token { get; set; }
            public string IP { get; set; }
        }
        public class loguser
        {
            public string username { get; set; }
            public string password { get; set; }
        }

        public class newToken
        {
            public string token { get; set; }
            public DateTime expiration { get; set; }


        }

        public APISettings GetAPI()
        {

            APISettings settings = new APISettings();
            settings.IP = this.IP;
            settings.token = this.TOKEN;
            //return settings;
            loguser user = new loguser();
            user.username = "Administrator";
            user.password = "SuperAdminPasswd1234!";

            return settings;
            //Evaluar token
            //Consultar si token no ha  expirado
            //var tokenCookies = HttpContext.Current.Request.Cookies["token"];

            //if (tokenCookies != null)
            //{
            //    settings.token = tokenCookies.Value;
            //    return settings;
            //}
            //else
            //{
            //    var client = new RestClient(settings.IP);
            //    var request = new RestRequest("/api/login", Method.POST);

            //    request.RequestFormat = DataFormat.Json;
            //    request.AddJsonBody(user);

            //    var result = client.Execute(request);
            //    var jsonResponse = JsonConvert.DeserializeObject<newToken>(result.Content);

            //    settings.token = result.Headers[2].Value.ToString();



            //    var c = new System.Web.HttpCookie("token_auth");
            //    c.Value = result.Headers[2].Value.ToString();
            //    c.Expires = DateTime.Now.AddMinutes(4);
            //    HttpContext.Current.Response.Cookies.Add(c);


            //    return settings;

            //    }
            }
        public APISettings GetAPI_WToken()
        {
            APISettings settings = new APISettings();
            settings.IP = this.IP_Auth;
            settings.token = this.TOKEN;
            // return settings;

            loguser user = new loguser();
            user.username = "Administrator";
            user.password = "SuperAdminPasswd1234!";

            //Evaluar token
            //Consultar si token no ha  expirado
            var tokenCookies = HttpContext.Current.Request.Cookies["token_auth"];

            if (tokenCookies != null)
            {
                settings.token = tokenCookies.Value;
                return settings;
            }
            else
            {
                var client = new RestClient(settings.IP);
                var request = new RestRequest("gateway/Security/Login", Method.POST);

                request.RequestFormat = DataFormat.Json;
                request.AddJsonBody(user);

                var result = client.Execute(request);
                var jsonResponse = JsonConvert.DeserializeObject<newToken>(result.Content);

                settings.token = result.Headers[1].Value.ToString();


                var c = new System.Web.HttpCookie("token_auth");
                c.Value = result.Headers[1].Value.ToString();
                c.Expires = DateTime.Now.AddMinutes(4);
                HttpContext.Current.Response.Cookies.Add(c);


                return settings;

            }
        }
    }
}