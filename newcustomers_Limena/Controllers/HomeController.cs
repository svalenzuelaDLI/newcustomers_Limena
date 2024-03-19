
using newcustomers_Limena.Helpers;
using newcustomers_Limena.Helpers.MasterData;
using newcustomers_Limena.Models;
using Newtonsoft.Json;
using Postal;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using static newcustomers_Limena.Models.Auxiliar_models;

namespace customers_Limena.Controllers
{
    public class HomeController : Controller
    {
        private dbLimenaEntities dblim = new dbLimenaEntities();
        private DLI_PROEntities dlipro = new DLI_PROEntities();
        private Customers customerclass = new Customers();
        private Properties api_Properties = new Properties();

        public ActionResult Index()
        {
            var tipotamana = dlipro.UFD1.Where(c => c.TableID.Contains("OCRD") && c.FieldID == 50 && !c.Descr.Contains("Not Assigned")).ToList();
            ViewBag.tamanostienda = tipotamana;
            var users = dblim.Sys_Users.Where(a => a.Roles.Contains("Sales Representative")).Select(a=>a.IDSAP).ToList();


            users.Add("6");
            ViewBag.lstreps = JsonConvert.SerializeObject(users);

            //from API
            //Etnias
            var resultEtnias = api_Properties.GetEtnias();
            ViewBag.etnias = resultEtnias.data;
            //Services
            var resultServices = api_Properties.GetServices();
            ViewBag.services = resultServices.data;
            //Trucks
            //var resultTrucks = api_Properties.GetTrucks();
            //ViewBag.trucks = resultTrucks.data;

            return View();
        }

        [HttpPost]
        public ActionResult UploadFiles(string id, int idcustomer, string compania, string orientation)
        {
            var isnew = 1;
            Tb_NewCustomers customer = new Tb_NewCustomers();
            if (idcustomer == 0)
            {
                isnew = 1;
            }
            else
            {
                var registro = dblim.Tb_NewCustomers.Where(a => a.ID_customer == idcustomer).FirstOrDefault();
                if (registro == null) { isnew = 1; } else { customer = registro; isnew = 0; idcustomer = registro.ID_customer; }
            }

            customer.CardName = compania.ToUpper();
            // Checking no of files injected in Request object  
            //if (Request.Files.Count > 0)
            //{
            try
            {
                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;
                for (int i = 0; i < files.Count; i++)
                {
                    //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                    //string filename = Path.GetFileName(Request.Files[i].FileName);  

                    HttpPostedFileBase file = files[i];
                    string fname;

                    // Checking for Internet Explorer  
                    if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                    {
                        string[] testfiles = file.FileName.Split(new char[] { '\\' });
                        fname = testfiles[testfiles.Length - 1];
                    }
                    else
                    {
                        fname = file.FileName;
                    }

                    Image TargetImg = Image.FromStream(file.InputStream, true, true);
                    try
                    {
                        int or = Convert.ToInt32(orientation);

                        switch (or)
                        {
                            case 1: // landscape, do nothing
                                break;

                            case 8: // rotated 90 right
                                    // de-rotate:
                                TargetImg.RotateFlip(rotateFlipType: RotateFlipType.Rotate270FlipNone);
                                break;

                            case 3: // bottoms up
                                TargetImg.RotateFlip(rotateFlipType: RotateFlipType.Rotate180FlipNone);
                                break;

                            case 6: // rotated 90 left
                                TargetImg.RotateFlip(rotateFlipType: RotateFlipType.Rotate90FlipNone);
                                break;
                        }

                    }
                    catch
                    {

                    }
                    DateTime time = DateTime.UtcNow;


                    // display a clone for demo purposes
                    //pb2.Image = (Image)TargetImg.Clone();
                    Image imagenfinal = (Image)TargetImg.Clone();

                    var path = "";

                    if (id == "11")
                    { //TAX ID
                        path = Path.Combine(Server.MapPath("~/Content/ftpimages/enroll"), id + "_enroll_" + "TAXID_" + time.Minute + time.Second + ".jpg");
                    }
                    else
                    {
                        path = Path.Combine(Server.MapPath("~/Content/ftpimages/enroll"), id + "_enroll_" + "CERTNUM_" + time.Minute + time.Second + ".jpg");
                    }



                    var tam = file.ContentLength;

                    //if (tam < 600000)
                    //{
                    //Image newimage;
                    //Cambiar tamano no calidad
                    //if (orientation == "-1")
                    //{
                    //    newimage = ScaleImage(bitmapNewImage, 768, 1360);
                    //}
                    //else
                    //{
                    //    newimage = ScaleImage(bitmapNewImage, 1360, 768);
                    //}
                    //newimage.Save(path, ImageFormat.Jpeg);
                    imagenfinal.Save(path, ImageFormat.Jpeg);





                    if (isnew == 0)
                    {//nada mas actualizamos
                     //Guardamos en DB
                        if (id == "11")
                        { //TAX ID
                            customer.url_imageTAXCERT = "~/ftpimages/enroll/" + id + "_enroll_" + "TAXID_" + time.Minute + time.Second + ".jpg";
                        }
                        else
                        {

                            customer.url_imageTAXCERNUM = "~/ftpimages/enroll/" + id + "_enroll_" + "CERTNUM_" + time.Minute + time.Second + ".jpg";
                        }
                        dblim.Entry(customer).State = EntityState.Modified;
                        dblim.SaveChanges();
                    }
                    else
                    {
                        //Creamos un modelo completo
                        customer.CardName = compania.ToUpper();
                        customer.Phone1 = "";
                        customer.E_Mail = "";
                        customer.IntrntSite = "";
                        customer.TAXID = "";
                        customer.TAXCERTNUM = "";
                        customer.FirstName = "";
                        customer.LastName = "";
                        customer.Position = "";
                        customer.Tel1 = "";
                        customer.E_MailL = "";
                        customer.Street = "";
                        customer.City = "";
                        customer.State = "";
                        customer.ZipCode = "";
                        customer.Country = "";
                        customer.StoreServices = "";
                        customer.Etnias = "";
                        customer.OperationTime = "";
                        customer.ReciboMercaderia_dia = "";
                        customer.ReciboMercaderia_area = "";
                        customer.Muelle_descarga = false;
                        customer.Store_size = "";
                        customer.Validated = false;
                        customer.OnSharepoint = false;
                        customer.status = 0;
                        customer.urlsharepoint = "";
                        customer.idsharepoint = 0;

                        //Guardamos en DB
                        if (id == "11")
                        { //TAX ID
                            customer.url_imageTAXCERT = "~/ftpimages/enroll/" + id + "_enroll_" + "TAXID_" + time.Minute + time.Second + ".jpg";
                            customer.url_imageTAXCERNUM = "";
                        }
                        else
                        {
                            customer.url_imageTAXCERNUM = "~/ftpimages/enroll/" + id + "_enroll_" + "CERTNUM__" + time.Minute + time.Second + ".jpg";
                            customer.url_imageTAXCERT = "";
                        }



                        customer.idRep = 0;
                        customer.idSAPRep = 0;
                        customer.emailRep = "";
                        var idsup = 19; //por defecto
                        customer.Supervisor = idsup;

                        dblim.Tb_NewCustomers.Add(customer);
                        dblim.SaveChanges();

                        idcustomer = customer.ID_customer;
                    }


                }


                // Returns message that successfully uploaded  
                var result = new { result = "File Uploaded Successfully!", result2 = idcustomer };

                return Json(result, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var result = new { result = "Error occurred.", result2 = idcustomer };

                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }
        [HttpPost]
        public ActionResult UploadFilesAPI(string id, int idcustomer, string compania, string orientation)
        {



            // Checking no of files injected in Request object  
            //if (Request.Files.Count > 0)
            //{
            try
            {
                //  Get all files from Request object  
                HttpFileCollectionBase files = Request.Files;

                //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                //string filename = Path.GetFileName(Request.Files[i].FileName);  

                HttpPostedFileBase file = files[0];
                string fname;

                // Checking for Internet Explorer  
                if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                {
                    string[] testfiles = file.FileName.Split(new char[] { '\\' });
                    fname = testfiles[testfiles.Length - 1];
                }
                else
                {
                    fname = file.FileName;
                }

                Image TargetImg = Image.FromStream(file.InputStream, true, true);
                try
                {
                    int or = Convert.ToInt32(orientation);

                    switch (or)
                    {
                        case 1: // landscape, do nothing
                            break;

                        case 8: // rotated 90 right
                                // de-rotate:
                            TargetImg.RotateFlip(rotateFlipType: RotateFlipType.Rotate270FlipNone);
                            break;

                        case 3: // bottoms up
                            TargetImg.RotateFlip(rotateFlipType: RotateFlipType.Rotate180FlipNone);
                            break;

                        case 6: // rotated 90 left
                            TargetImg.RotateFlip(rotateFlipType: RotateFlipType.Rotate90FlipNone);
                            break;
                    }

                }
                catch
                {

                }
                DateTime time = DateTime.UtcNow;


                // display a clone for demo purposes
                //pb2.Image = (Image)TargetImg.Clone();
                Image imagenfinal = (Image)TargetImg.Clone();

                var path = "";

                if (id == "11")
                { //TAX ID
                    path = Path.Combine(Server.MapPath("~/Content/ftpimages/enroll"), id + "_enroll_" + "TAXID_" + time.Minute + time.Second + ".jpg");
                }
                else
                {
                    path = Path.Combine(Server.MapPath("~/Content/ftpimages/enroll"), id + "_enroll_" + "CERTNUM_" + time.Minute + time.Second + ".jpg");
                }



                var tam = file.ContentLength;


                imagenfinal.Save(path, ImageFormat.Jpeg);


                var response = customerclass.PostCustomerDocument(path);

                if (response.succeeded)
                {
                    var result = new { result = response.data.federalTaxImgeUrl};
                    return Json(result, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var result = new { result = "Error" };
                    return Json(result, JsonRequestBehavior.AllowGet);
                }

            }
            catch (Exception ex)
            {
                var result = new { result = "Error", result2 = idcustomer };

                return Json(result, JsonRequestBehavior.AllowGet);
            }


        }

        [HttpPost]
        public ActionResult UpdateInformationNewCustomer(int idcustomer, string CardName, string Phone1, string E_Mail, string IntrntSite, string TAXID, string TAXCERTNUM, string FirstName, string LastName, string Position, string Tel1, string E_MailL, string countryId, string stateId, string cityId, string Street, string ZipCode, string servicesSelected, string etniasselected, string HorarioOperacion, string ReciboMercaderiaDia, string ReciboMercaderiaArea, Boolean MuelleDescarga, string TamanoTienda, string idrep)
        {
                try
                {
                    //Creamos modelo
                    Tb_NewCustomers newCustomer = dblim.Tb_NewCustomers.Find(idcustomer);

                    newCustomer.CardName = CardName.ToUpper();
                    newCustomer.Phone1 = Phone1;
                    newCustomer.E_Mail = E_Mail.ToUpper();
                    newCustomer.IntrntSite = IntrntSite.ToUpper();
                    newCustomer.TAXID = TAXID.ToUpper();
                    newCustomer.TAXCERTNUM = TAXCERTNUM.ToUpper();
                    newCustomer.FirstName = FirstName.ToUpper();
                    newCustomer.LastName = LastName.ToUpper();
                    newCustomer.Position = Position.ToUpper();
                    newCustomer.Tel1 = Tel1;
                    newCustomer.E_MailL = E_MailL.ToUpper();
                    newCustomer.Street = Street.ToUpper();
                    newCustomer.City = cityId.ToUpper();
                if (newCustomer.City == "JACKSON") {
                    newCustomer.City = "JACKSON MS";
                }
                    newCustomer.State = stateId.ToUpper();

                if (newCustomer.State == "") {
                    newCustomer.State = "MISSISSIPPI";
                }

                    newCustomer.ZipCode = ZipCode.ToUpper();
                    newCustomer.Country = countryId.ToUpper();
                    newCustomer.StoreServices = servicesSelected;
                    newCustomer.Etnias = etniasselected;
                    newCustomer.OperationTime = HorarioOperacion;
                    newCustomer.ReciboMercaderia_dia = ReciboMercaderiaDia.ToUpper();
                    newCustomer.ReciboMercaderia_area = ReciboMercaderiaArea.ToUpper();
                    newCustomer.Muelle_descarga = MuelleDescarga;
                    newCustomer.Store_size = TamanoTienda;
                    newCustomer.Validated = false;
                    newCustomer.OnSharepoint = false;
                    newCustomer.status = 0;
                    newCustomer.urlsharepoint = "";
                    newCustomer.idsharepoint = 0;
                newCustomer.idRep = 0;
                newCustomer.idSAPRep = 0;
                newCustomer.emailRep = "";

                var usuario = dblim.Sys_Users.Where(a => a.IDSAP == idrep).FirstOrDefault();
                if (usuario != null)
                {
                    newCustomer.idRep = usuario.ID_User;
                    if (usuario.IDSAP == "" || usuario.IDSAP == null)
                    {
                        newCustomer.idSAPRep = 0;
                    }
                    else
                    {
                        newCustomer.idSAPRep = Convert.ToInt32(usuario.IDSAP);
                    }

                    newCustomer.emailRep = usuario.Email;
                }


                //validar rep
                //Cambio 05/08/2020 : Se colocara por defecto Gerencia Comercia (ID:0) a supervisor, de esta forma validamos supervisores por Estado
                var idsup = 19;
                    newCustomer.Supervisor = idsup;
                    try
                    {
                    //07.22.2021 se agrego campo de ciudad para filtrar
                        List<ZipcodeStatesSupervisor> statesList = dlipro.Database.SqlQuery<ZipcodeStatesSupervisor>("SELECT * FROM SDK_State_and_Supervisor").ToList();
                        var statess = stateId.ToUpper();
                        var city = cityId.ToUpper();
                        var existe = statesList.Where(c => c.State == statess && c.City==city).ToList();
                            if (existe.Count > 0)
                            {
                                idsup = existe[0].Supervisor;
                                newCustomer.Supervisor = idsup;
                        }
                        else
                        {
                            //Hacemos comprobacion SOLO por estado
                            var existe2 = statesList.Where(c => c.State == statess).ToList();
                            if (existe2.Count > 0)
                            {
                                idsup = existe2[0].Supervisor;
                                newCustomer.Supervisor = idsup;
                        }
                    }
                    }
                    catch
                    {

                    }


                    dblim.Entry(newCustomer).State = EntityState.Modified;
                    dblim.SaveChanges();
                //Se quitara por el momento dicho proceso para los supervisores y se le enviara a Customer Services ya que ellos validaran 
                try
                {
      
                        //Enviamos a Customer Services
                        dynamic semail = new Email("email_notificationEnrollSup");
                        //semail.To = supervisor.Email.ToString();
                        semail.To = "d.velasquez@limenainc.net"; //Customer services, no se creara perfil
                        semail.From = "donotreply@limenainc.net";
                        semail.user = newCustomer.FirstName + " " + newCustomer.LastName;
                        semail.company = newCustomer.CardName;

                        semail.Send();
                    
                }
                catch
                {

                }

                try
                    {
                        //Notificacion a cliente
                        dynamic semail = new Email("email_notificationEnrollNC");
                        semail.To = newCustomer.E_MailL;
                        semail.From = "donotreply@limenainc.net";
                        //semail.ccrep = newCustomer.emailRep;
                        semail.user = newCustomer.FirstName + " " + newCustomer.LastName;
                        if (newCustomer.emailRep == "") { semail.ccrep = "donotreply@limenainc.net"; } else { semail.ccrep = newCustomer.emailRep; }

                        semail.Send();
                    }
                    catch
                    {

                    }



                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }

        }


        //models aux for services and etnias
        public class etnias
        {
            public string code { get; set; }
            public string name { get; set; }
        }
        public class services
        {
            public string code { get; set; }
            public string name { get; set; }
        }
        [HttpPost]
        public ActionResult createnewcustomer(string CardName, string Phone1, string E_Mail, string IntrntSite, string TAXID, string TAXCERTNUM, string FirstName, string LastName, 
            string Position, string Tel1, string E_MailL, string countryId, string stateId, string cityId, string Street, string ZipCode, 
            List<services> servicesSelected, List<etnias> etniasselected, string HorarioOperacion, string ReciboMercaderiaDia, string ReciboMercaderiaArea, 
            Boolean MuelleDescarga, string TamanoTienda, string idrep, string Urltaxid,string Urlcertnum)
        {
            try
            {
                Customers_POST.Customer newCustomer = new Customers_POST.Customer();

                newCustomer.customerName = CardName.ToUpper();
                newCustomer.storePhone = Phone1;
                newCustomer.storeEmail = E_Mail.ToUpper();
                newCustomer.siteWeb = IntrntSite.ToUpper();
                newCustomer.federalTax = TAXID.ToUpper();
                newCustomer.federalTaxImgeUrl =Urltaxid;
                newCustomer.resalesTaxCertificate = TAXCERTNUM.ToUpper();
                newCustomer.resalesTaxCertificateImageUrl = Urlcertnum;
                //llenamos contacto principal
                Customers_POST.Contacts newcontact = new Customers_POST.Contacts();
                newcontact.firstName = FirstName.ToUpper();
                newcontact.lastName = LastName.ToUpper();
                newcontact.position = Position.ToUpper();
                newcontact.contactPhone = Tel1;
                newcontact.email = E_MailL.ToUpper();

                newCustomer.contacts = new List<Customers_POST.Contacts>();
                newCustomer.contacts.Add(newcontact);


                newCustomer.street = Street.ToUpper();
                newCustomer.city = cityId.ToUpper();
                if (newCustomer.city == "JACKSON")
                {
                    newCustomer.city = "JACKSON MS";
                }
                newCustomer.state = stateId.ToUpper();

                if (newCustomer.state == "")
                {
                    newCustomer.state = "MISSISSIPPI";
                }

                newCustomer.zipCode = ZipCode.ToUpper();
                newCustomer.country = countryId.ToUpper();
                newCustomer.receivingDays = ReciboMercaderiaDia.ToUpper();
                newCustomer.receivingZone = ReciboMercaderiaArea.ToUpper();
                newCustomer.operationTime = HorarioOperacion;
                newCustomer.loadingDock = MuelleDescarga;
                newCustomer.sendNotification = true;
                newCustomer.properties = new List<Customers_POST.Properties>();
                if (servicesSelected.Count > 0)
                {
                    foreach(var item in servicesSelected)
                    {
                        Customers_POST.Properties property = new Customers_POST.Properties();
                        property.code = Convert.ToInt32(item.code);
                        property.name = item.name;
                  
                        newCustomer.properties.Add(property);
                    }
                  
                }

                if (etniasselected.Count > 0)
                {
                    foreach (var item in etniasselected)
                    {
                        Customers_POST.Properties property = new Customers_POST.Properties();
                        property.code = Convert.ToInt32(item.code);
                        property.name = item.name;
                        newCustomer.properties.Add(property);
                    }

                }


                //newCustomer.StoreServices = servicesSelected;
                //newCustomer.Etnias = etniasselected;
                var result = customerclass.PostCustomer(newCustomer);
                // Returns message that successfully uploaded  
                if (result.IsSuccessful)
                {
                    return Json("File Uploaded Successfully!");
                }
                else
                {
                    return Json("Error");
                }

            }
            catch (Exception ex)
            {
                return Json("Error occurred. Error details: " + ex.Message);
            }

        }


        public ActionResult Success()
        {
            return View();
        }
    }
}