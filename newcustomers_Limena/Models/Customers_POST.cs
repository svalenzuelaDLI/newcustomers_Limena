using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace newcustomers_Limena.Models
{
    public class Customers_POST
    {
        public class Contacts
        {
            public string firstName { get; set; }
            public string lastName { get; set; }
            public string position { get; set; }
            public string contactPhone { get; set; }
            public string email { get; set; }
        }

        public class Image
        {
            public byte[] ResalesTaxCertificateImage { get; set; }
            public byte[] FederalTaxImge { get; set; }
        }

        public class Properties
        {
            public int code { get; set; }
            public string name { get; set; }
        }

        public class Customer
        {
            public List<Properties> properties { get; set; }
            public List<Contacts> contacts { get; set; }
            public string customerName { get; set; }
            public string storePhone { get; set; }
            public string storeEmail { get; set; }
            public string siteWeb { get; set; }
            public string federalTax { get; set; }
            public string federalTaxImgeUrl { get; set; }
            public string resalesTaxCertificate { get; set; }
            public string resalesTaxCertificateImageUrl { get; set; }
            public string street { get; set; }
            public string city { get; set; }
            public string zipCode { get; set; }
            public string state { get; set; }
            public string country { get; set; }
            public string operationTime { get; set; }
            public string receivingDays { get; set; }
            public string receivingZone { get; set; }
            public bool loadingDock { get; set; }
            public bool sendNotification { get; set; }
            public string userId { get; set; }
            public string fatherCard { get; set; }
            public int priceList { get; set; }
        }


        public class postCustomerDocument
        {
            public bool succeeded { get; set; }
            public int errorCode { get; set; }
            public string errorMessage { get; set; }
            public postCustomerDocument_data data { get; set; }
        }

        public class postCustomerDocument_data
        {
            public string resalesTaxCertificateImageUrl { get; set; }
            public string federalTaxImgeUrl { get; set; }
        }
    }
}