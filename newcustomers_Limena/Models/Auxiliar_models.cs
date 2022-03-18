using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newcustomers_Limena.Models
{
    public class Auxiliar_models
    {
        public class ZipcodeStatesSupervisor
        {
            public int Supervisor { get; set; }
            public string StateCode { get; set; }
            public string State { get; set; }
            public string City { get; set; }
        }
    }
}