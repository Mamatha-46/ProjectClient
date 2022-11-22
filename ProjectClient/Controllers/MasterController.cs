using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using Google.Apis.Sheets.v4.Data;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ProjectClient.Models;

namespace ProjectClient.Controllers
{
    public class MasterController : Controller
    {
        static readonly string[] Scopes = { SheetsService.Scope.Spreadsheets };
        static readonly string ApplicationName = "ProjectClient";
        String spreadsheetId = "1gJYRryk7FgDPqC2ZTZLc4ahuvC1DzheHBu7GCV5KFpM";
        String Sheet = "Sheet1";
        string Mastersheet = "Sheet2";
        string logsheet = "Sheet3";


        private SheetsService service;
        private string range;
        // private byte[] fileData;
        public MasterController()
        {
            Init();
        }
        private void Init()
        {
            GoogleCredential credential;
            //Reading Credentials File...
            using (var stream = new FileStream("mastertable-007abaa4ffb8.json", FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream)
                    .CreateScoped(Scopes);
            }
            // Creating Google Sheets API service...
            service = new SheetsService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }
        public List<Googlesheet> GetGooglesheets()
        {
            var myInvList = new List<Googlesheet>();
            var range = $"{Sheet}!A:J";
            int j = 0;
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
            // Ecexuting Read Operation...
            var response = request.Execute();
            // Getting all records from Column A to E...
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    j++;
                    if (j > 1)
                    {
                        var myInv = new Googlesheet()
                        {

                            ID = Int32.Parse(row[0].ToString()),
                            FirstName = row[1].ToString(),
                            LastName = row[2].ToString(),
                            Mobile = row[3].ToString(),
                            Alternatemobilenumber = row[4].ToString(),
                            EmailID = row[5].ToString(),
                            IDType = row[6].ToString(),
                            IDProofnumber = row[7].ToString(),
                            IDproof = row[8].ToString(),
                            Photo = row[9].ToString()
                        };

                        myInvList.Add(myInv);
                    }
                }
            }
            return myInvList;
        }
        public ActionResult Index()
        {
            var item = GetGooglesheets();
            return View(item);
        }
        public List<Googlesheet> GetBYID(string Number)
        {
            var myInvList = new List<Googlesheet>();
            var range = $"{Sheet}!A:J";
            int j = 0;
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
            // Ecexuting Read Operation...
            var response = request.Execute();
            // Getting all records from Column A to E...
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    j++;
                    if (j > 1)
                    {
                        var id = Int32.Parse(row[0].ToString());
                        var mobile = row[3].ToString();
                        var IDProofnumber = row[7].ToString();
                        var Alternatemobilenumber = row[4].ToString();

                        if (mobile == Number || Alternatemobilenumber == Number || IDProofnumber == Number)
                        {
                            var myInv = new Googlesheet()
                            {
                                ID = Int32.Parse(row[0].ToString()),
                                FirstName = row[1].ToString(),
                                LastName = row[2].ToString(),
                                Mobile = row[3].ToString(),
                                Alternatemobilenumber = row[4].ToString(),
                                EmailID = row[5].ToString(),
                                IDType = row[6].ToString(),
                                IDProofnumber = row[7].ToString(),
                                IDproof = row[8].ToString(),
                                Photo = row[9].ToString(),

                            };
                            myInvList.Add(myInv);

                        }
                    }

                }

            }
            return myInvList;
        }
        [HttpPost]
        public ActionResult Index(string Number)
        {
            var item = GetBYID(Number);
            return View(item);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Googlesheet GSheet)
        {
            range = $"{Sheet}!A:J";
            var valueRange = new ValueRange();
            int mynewID = 1;
            var myInvList = GetGooglesheets();
            if (myInvList.Count > 0) mynewID = myInvList.Max(x => x.ID) + 1;
            string FileName = Path.GetFileName(GSheet.ImageFile.FileName);
            string imgpath = Path.Combine(Server.MapPath("~/Image"), FileName);
            GSheet.IDproof = "~/Image/" + FileName;
            GSheet.ImageFile.SaveAs(imgpath);
            string FileName1 = Path.GetFileName(GSheet.ImageFile1.FileName);
            string imgpath1 = Path.Combine(Server.MapPath("~/Image"), FileName1);
            GSheet.Photo = "~/Image/" + FileName1;
            GSheet.ImageFile1.SaveAs(imgpath1);
            var oblist = new List<object>() { mynewID, GSheet.FirstName, GSheet.LastName, GSheet.Mobile, GSheet.Alternatemobilenumber, GSheet.EmailID, GSheet.IDType, GSheet.IDProofnumber, GSheet.IDproof, GSheet.Photo };
            valueRange.Values = new List<IList<object>> { oblist };
            var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, range);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();
            return View();
        }
        public ActionResult Edit(int ID)
        {
            int rowID = 0;
            var range = $"{Sheet}!A:J";
            string FirstName = null;
            string LastName = null;
            int Mobile = 0;
            int Alternatemobilenumber = 0;
            string EmailID = null;
            string IDType = null;
            string IDProofnumber = null;
            string IDproof = null;
            string Photo = null;
            Googlesheet model = new Googlesheet();
            int j = 0;
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var GSheet in values)
                {
                    j++;
                    if (j > 1)
                    {
                        var Id = Int32.Parse(GSheet[0].ToString());
                        if (Id == ID)
                        {
                            rowID = j;
                            model.ID = rowID;
                            model.FirstName = GSheet[1].ToString();
                            model.LastName = GSheet[2].ToString();
                            model.Mobile = GSheet[3].ToString();
                            model.Alternatemobilenumber = GSheet[4].ToString();
                            model.EmailID = GSheet[5].ToString();
                            model.IDType = GSheet[6].ToString();
                            model.IDProofnumber = GSheet[7].ToString();
                            model.IDproof = GSheet[8].ToString();
                            model.Photo = GSheet[9].ToString();

                        }
                    }
                }
            }
            var range2 = $"{Sheet}!A{rowID}:J{rowID}";
            var valueRange = new ValueRange();
            var oblist = new List<object>() { rowID, FirstName, LastName, Mobile, Alternatemobilenumber, EmailID, IDType, IDProofnumber, IDproof, Photo };
            valueRange.Values = new List<IList<object>> { oblist };
            return View(model);
        }
        [HttpPost]
        public ActionResult Edit(Googlesheet Gsheet)
        {
            string y = (TempData["Data"]).ToString();
            string p = (TempData["Data1"]).ToString();
            if (Gsheet.ImageFile == null)
            {
                Gsheet.IDproof = y;
            }
            else
            {
                string Filename = Path.GetFileName(Gsheet.ImageFile.FileName);
                string path = Path.Combine(Server.MapPath("~/Image"), Filename);
                Gsheet.ImageFile.SaveAs(path);
                Gsheet.IDproof = "~/Image" + Filename;
            }
            if (Gsheet.ImageFile1 == null)
            {
                Gsheet.Photo = p;
            }
            else
            {
                string FileName1 = Path.GetFileName(Gsheet.ImageFile1.FileName);
                string imgpath1 = Path.Combine(Server.MapPath("~/Image"), FileName1);
                Gsheet.Photo = "~/Image/" + FileName1;
                Gsheet.ImageFile1.SaveAs(imgpath1);
            }
            int rowID = 0;
            var range = $"{Sheet}!A:J";
            // int j= 0;
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            rowID = Gsheet.ID + 1;


            var range2 = $"{Sheet}!A{rowID}:J{rowID}";
            var valueRange = new ValueRange();
            var oblist = new List<object>() { Gsheet.ID, Gsheet.FirstName, Gsheet.LastName, Gsheet.Mobile, Gsheet.Alternatemobilenumber, Gsheet.EmailID, Gsheet.IDType, Gsheet.IDProofnumber, Gsheet.IDproof, Gsheet.Photo };
            valueRange.Values = new List<IList<object>> { oblist };
            // Performing Update Operation...
            var updateRequest = service.Spreadsheets.Values.Update(valueRange, spreadsheetId, range2);
            updateRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.UpdateRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = updateRequest.Execute();
            return RedirectToAction("Index");
        }
        public ActionResult Details(int id)
        {
            int rowID = 0;
            var range = $"{Sheet}!A:J";
            int j = 0;

            Googlesheet GSheet = new Googlesheet();

            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
            var response = request.Execute();
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    j++;
                    if (j > 1)
                    {
                        var Id = Int32.Parse(row[0].ToString());
                        if (Id == id)
                        {
                            rowID = j;
                            GSheet.ID = rowID;
                            GSheet.FirstName = row[1].ToString();
                            GSheet.LastName = row[2].ToString();
                            GSheet.Mobile = row[3].ToString();
                            GSheet.Alternatemobilenumber = row[4].ToString();
                            GSheet.EmailID = row[5].ToString();
                            GSheet.IDType = row[6].ToString();
                            GSheet.IDProofnumber = row[7].ToString();
                            GSheet.IDproof = row[8].ToString();
                            GSheet.Photo = row[9].ToString();

                        }
                    }

                }
            }
            return View(GSheet);
        }
        public void Delete(int Id)
        {
            int rowID = Id + 1;
            var range = $"{Sheet}!A{rowID}:J{rowID}";
            var requestBody = new ClearValuesRequest();
            var deleteRequest = service.Spreadsheets.Values.Clear(requestBody, spreadsheetId, range);
            var deleteReponse = deleteRequest.Execute();
        }
        public List<Googlesheet> Getlogtable()
        {
            var myInvList = new List<Googlesheet>();
            var range = $"{logsheet}!A:E";
            int j = 0;
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
            // Ecexuting Read Operation...
            var response = request.Execute();
            // Getting all records from Column A to E...
            IList<IList<object>> values = response.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var row in values)
                {
                    j++;
                    if (j > 1)
                    {
                        var myInv = new Googlesheet()
                        {
                            LOGID = Int32.Parse(row[0].ToString()),
                            UserID = Int32.Parse(row[1].ToString()),
                        };

                        myInvList.Add(myInv);
                    }
                }
            }
            return myInvList;
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Login(Googlesheet GSHEET)
        {
            var range = $"{Mastersheet}!A:C";
            //int j = 0;
            SpreadsheetsResource.ValuesResource.GetRequest request =
                    service.Spreadsheets.Values.Get(spreadsheetId, range);
            // Ecexuting Read Operation...
            var response = request.Execute();
            // Getting all records from Column A to E...
            IList<IList<object>> values = response.Values;
            var Sheetrange = $"{logsheet}!A:E";
            var valueRange = new ValueRange();
            if (values != null && values.Count > 0)
            {
                int j = 0;
                foreach (var row in values)
                {
                    j++;
                    if (j > 1)
                    {
                        var id = Int32.Parse(row[0].ToString());
                        var UserName = row[1].ToString();
                        var Password = row[2].ToString();

                        if (UserName == GSHEET.UserName && Password == GSHEET.Password)
                        {
                            ViewBag.Value = "Login Successfull";
                            int mynewID = 1;                           
                            var myInvList = Getlogtable();
                            if (myInvList.Count > 0) mynewID = myInvList.Max(x => x.LOGID) + 1;
                            GSHEET.LOGID = mynewID;
                            GSHEET.UserID = id;
                            string massage = string.Format("User{0} Login Succesfully", GSHEET.UserName);
                            GSHEET.LogDescription = massage;
                            var Dt = System.DateTime.Now.ToString();
                            GSHEET.Logdatetime = Dt;
                            string type = string.Format("Login Succesfully");
                            GSHEET.LogType = type;
                        }
                        
                    }
                }
            }
            if((GSHEET.LogType != "Login Succesfully"))
            {
                ViewBag.Value = "Login  Fail";
                if (values != null && values.Count > 0)
                {
                    int j = 0;
                    foreach (var row in values)
                    {
                        j++;
                        if (j > 1)
                        {
                            //var id = Int32.Parse(row[0].ToString());
                            var UserName = row[1].ToString();
                            var Password = row[2].ToString();
                            if (UserName != GSHEET.UserName && Password == GSHEET.Password)
                            {
                                GSHEET.LogDescription = string.Format("User{0} Login Succesfully.Invalid UserName", GSHEET.UserName);
                            }
                            if (UserName == GSHEET.UserName && Password != GSHEET.Password)
                            {
                                // string massage = ;
                                GSHEET.LogDescription = string.Format("User{0} Login Succesfully.Invalid Password", GSHEET.UserName);
                            }
                            if (!(UserName != GSHEET.UserName && Password != GSHEET.Password))
                            { 
                                // string massage = ;
                                GSHEET.LogDescription = string.Format("User{0} Login Succesfully.Invalid UserName and Password", GSHEET.UserName);
                            }
                            //switch (GSHEET.LogDescription)
                            //{
                            //    case "a":if (UserName != GSHEET.UserName && Password == GSHEET.Password)
                                        
                            //            GSHEET.LogDescription = string.Format("User{0} Login Succesfully.Invalid UserName", GSHEET.UserName);
                            //            break;
                            //    case "b":
                            //        if (UserName == GSHEET.UserName && Password != GSHEET.Password)
                            //            // string massage = ;
                            //            GSHEET.LogDescription = string.Format("User{0} Login Succesfully.Invalid Password", GSHEET.UserName);
                            //        break;
                            //    case "c":
                            //        if (UserName != GSHEET.UserName && Password != GSHEET.Password)
                            //            // string massage = ;
                            //            GSHEET.LogDescription = string.Format("User{0} Login Succesfully.Invalid UserName and Password", GSHEET.UserName);
                            //        break;
                            //}
                        }
                    }
                }
                int mynewID = 1;                                
                var myInvList = Getlogtable();
                if (myInvList.Count > 0) mynewID = myInvList.Max(x => x.LOGID) + 1;
                GSHEET.LOGID = mynewID;
                var Dt = System.DateTime.Now.ToString();
                GSHEET.Logdatetime = Dt;
                string type = string.Format(" Login Fail");
                GSHEET.LogType = type;
            }
            var oblist = new List<object>() { GSHEET.LOGID, GSHEET.UserID, GSHEET.LogDescription, GSHEET.Logdatetime, GSHEET.LogType };
            valueRange.Values = new List<IList<object>> { oblist };
            var appendRequest = service.Spreadsheets.Values.Append(valueRange, spreadsheetId, Sheetrange);
            appendRequest.ValueInputOption = SpreadsheetsResource.ValuesResource.AppendRequest.ValueInputOptionEnum.USERENTERED;
            var appendReponse = appendRequest.Execute();
            return View();

        }
    }
}

    


 // GET: Master
 //public ActionResult Index()
 //{
 //  var credential = GoogleCredential.FromStream(new FileStream(Server.MapPath("~/mastertable-007abaa4ffb8.json"), FileMode.Open)).CreateScoped(Scopes);
//  var service = new SheetsService(new BaseClientService.Initializer()
//  {
//     HttpClientInitializer = credential
//     ApplicationName = ApplicationName,
//  });
//    // Define request parameters.  
// SpreadsheetsResource.ValuesResource.GetRequest request =service.Spreadsheets.Values.Get(spreadsheetId, range);
//// Prints the names and majors of students in a sample spreadsheet: 
////ValueRange response = request.Execute();
/////IList<IList<Object>> values = response.Values;
///// ViewBag.List = values;
// return View();
//}

                
            