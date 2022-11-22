using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace ProjectClient.Models
{
    public class Googlesheet
    {
        public int ID { get; set; }
        //[Required(ErrorMessage= "FirstName is required")]
        //[StringLength(12,MinimumLength =1)]
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Mobile { get; set; }
        public string Alternatemobilenumber { get; set; }
        public string EmailID { get; set; }
        public string IDType { get; set; }
        public string IDProofnumber { get; set; }
        public string IDproof { get; set; }
        public string Photo { get; set; }
        public HttpPostedFileBase ImageFile { get; set; }
        public HttpPostedFileBase ImageFile1 { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int UserID { get; set; }
        public int LOGID { get; set; }
        public string LogDescription { get; set; }
        public string Logdatetime { get; set; }
        //string date = DateTime.Now.ToString("yyyy-mm-dd hh:mm:ss");
        // public DateTime Logdatetime = Convert.ToDateTime(DateTime.Now.("yyyy-mm-dd hh:mm:ss"));

        //public DateTime dateTime = DateTime.UtcNow.GetDateTimeFormats("yyyy-mm-dd hh:mm:ss");

        public string LogType { get; set; }
        
    }
}
