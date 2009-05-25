using System;
using System.Data;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;
using ExtDirect4DotNet;

namespace WebApplication1.CallTypes
{
    public class FormResponse
    {
        public string firstName;
        public string lastName;
        public int age;
    }

    [DirectAction]
    public class CallTypes
    {
        [DirectMethod]
        public string Echo(string name)
        {
            return "Echo: " + name;
        }



        [DirectMethod]
        public string GetTime()
        {
            return DateTime.Now.ToLongTimeString();
        }

        /// <summary>
        /// Upload Method with request mapping handling inside
        /// 
        /// </summary>
        /// <param name="request"></param>
        [DirectMethod(MethodType = DirectMethodType.Form, OutputHandling=OutputHandling.JSON)]
        public string UploadHttpRequestParam(HttpRequest request)
        {
            return "{success:false, errors: {firstName: 'bullshit'}}";
            // return "the File: " + request.Files[0].FileName + " was successfully uploaded Firstname:" + request.Form["firstName"]; 
        }

        /// <summary>
        /// Upload Method names Solved via the ExtDirect4DotNet
        /// </summary>
        /// <param name="file"></param>
        /// <param name="firstName"></param>
        /// <returns></returns>
        [DirectMethod(MethodType = DirectMethodType.Form)]
        public string UploadNamedParameter(HttpPostedFile file, string firstName)
        {
            return "the File: " + file.FileName + " was successfully uploaded Firstname:" + firstName; 
        }


        /// <summary>
        /// A Hyprid Bethod
        /// You can call this method from the Browser
        /// Via CallTypes.SaveMethod(1,"Martin","Spaeth")
        /// Or with a Form as Parameter but with the Postfix _Form CallTypes.SaveMethod_Form(1,"Martin","Spaeth")
        /// </summary>
        /// <param name="age"></param>
        /// <param name="firstName"></param>
        /// <param name="lastName"></param>
        /// <returns></returns>
        [DirectMethod(MethodType=DirectMethodType.Hybrid)]
        public FormResponse SaveMethod(int age, string firstName, string lastName)
        {
           
            return new FormResponse() { age = age, firstName = firstName, lastName = lastName };
        }



        [DirectMethod]
        public string DateSample(DateTime d1, DateTime d2)
        {

            return d1.ToString() + " - " + d2.ToString();

        }


    }
}
