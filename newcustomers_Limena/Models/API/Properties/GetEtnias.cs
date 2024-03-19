using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newcustomers_Limena.Models.API.Properties
{
    public class GetEtnias
    {
        public bool succeeded { get; set; }
        public int errorCode { get; set; }
        public string errorMessage { get; set; }
        public List<string> validationErrors { get; set; }
        public List<Etnias> data { get; set; }
    }

    public class Etnias
    {
        public int code { get; set; }
        public string name { get; set; }

    }
}