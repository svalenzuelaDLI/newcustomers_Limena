using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newcustomers_Limena.Models.API.Properties
{
    public class GetTrucks
    {
        public bool succeeded { get; set; }
        public int errorCode { get; set; }
        public string errorMessage { get; set; }
        public List<string> validationErrors { get; set; }
        public List<Trucks> data { get; set; }
    }

    public class Trucks
    {
        public string fldValue { get; set; }
        public string descr { get; set; }

    }
}