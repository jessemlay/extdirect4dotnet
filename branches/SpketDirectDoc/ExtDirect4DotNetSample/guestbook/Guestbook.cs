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
using ExtDirect4DotNet.baseclasses;
using ExtDirect4DotNet.interfaces;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections;

namespace WebApplication1.guestbook
{
    [DirectAction]
    public class Guestbook : SimpleCRUDWithPaging, IActionWithAfterCreation<HttpContext>, IActionWithBeforeDestroy
    {
        #region Direct Methods

        /// <summary>
        /// Gets called by the Form
        /// Validates the data and Adds a Comment to the Guestbook
        /// </summary>
        /// <param name="lastName">The Lastname to add to the comment</param>
        /// <param name="firstName">The Firstname to add to the comment</param>
        /// <param name="email">The Email-addres Add to the comment</param>
        /// <param name="message">The Message which should appear in the board</param>
        /// <param name="picture">The Uploaded File</param>
        /// <returns></returns>
        [DirectMethod(MethodType=DirectMethodType.Form)]
        public bool addEntry(string lastName, string firstName, string email, string message, HttpPostedFile picture)
        {
            Hashtable errors = new Hashtable();
            
            string filename = null;
            if (picture.FileName != "")
            {
                string extension = System.IO.Path.GetExtension(picture.FileName).ToUpper();
                if (extension != ".JPG" && extension != ".GIF" && extension != ".PNG")
                {
                    errors.Add("picture", "Your choose a File with an unallowed extension (" + extension + ") allowed Filetypes: (*.jpg, *.gif, *.png)");
                }
                else
                {
                    filename = DateTime.Now.ToFileTime() + extension;
                    string savePath = httpContext.Server.MapPath("./guestbook/uploadedFiles/") + filename;
                    picture.SaveAs(savePath);
                }
            }

            DataRow entrie = entries.Tables[0].NewRow();
            if(inputValidator(firstName) != "") {
                errors.Add("firstName", inputValidator(firstName));
            }
            if(inputValidator(lastName) != "") {
                errors.Add("lastName", inputValidator(lastName));
            }
            if(inputValidator(message) != "") {
                errors.Add("message", inputValidator(message));
            }

            if(errors.Count > 0) {
                throw new ExtDirect4DotNet.exceptions.DirectFormInvalidException(errors);
            }

            entrie["id"] = getNewId();
            if(myEntries[entrie["id"]] == null)
                myEntries.Add(entrie["id"], true);
            myEntries = myEntries;
            entrie["firstName"] = firstName;
            entrie["lastName"] = lastName;
            entrie["message"] = message.Replace(Environment.NewLine, "<br>"); ;
            entrie["pictureName"] = filename == null ? "noavatar.jpg" : filename;
            entrie["date"] = DateTime.Now;
            entries.Tables[0].Rows.Add(entrie);

            return true;
        }

        

        /// <summary>
        /// Function to login
        /// </summary>
        /// <param name="pw">plaintext representation of the Passwort to chec</param>
        /// <returns>true if the password is valid</returns>
        [DirectMethod]
        public bool login(string pw)
        {
            if (HashString(pw) == "283de63694da71982074f473320810c5")
            {
                this.loggedIn = true;
            } else {
                this.loggedIn = false;
            }
            return this.loggedIn;
        }

        #region CRUD methods

        /// <summary>
        /// Read method for the dataEntries
        /// </summary>
        /// <returns>the messages as sorted DefaultView</returns>
        [DirectMethod(MethodType = DirectMethodType.Read)]
        public DataView read()
        {
            entries.Tables[0].DefaultView.Sort = "date desc";
            return entries.Tables[0].DefaultView;
        }

        /// <summary>
        /// Removes the Record  with the id form the database
        /// If the Record was not created by the user himself it throws an exception
        /// so we can ask for login
        /// </summary>
        /// <param name="id">the Id of the record</param>
        /// <returns>true if the delet was successfull</returns>
        [DirectMethod(MethodType = DirectMethodType.Delete)]
        public bool destroy(int id)
        {
            if (myEntries[id] == null && loggedIn == false)
            {
                throw new ExtDirect4DotNet.exceptions.DirectException("Sie müssen eingeloggt sein");
            }
            DataRow[] found = this.entries.Tables[0].Select("id = '" + id + "'");

            if (found.Length == 0)
                return true;
            else
            {
                found[0].Delete();
            }
            return true;
        }
        #endregion

        #endregion


        #region class specific overrids called by ExtDirect4DotNet
        public override int getResultCount()
        {
            return entries.Tables[0].Rows.Count;
        }

        public override bool addMetaData()
        {
            return false;
        }

        #region IActionWithAfterCreation<HttpContext> Member
        private HttpContext httpContext;
        private DataSet entries;

        /// <summary>
        /// After Creation method. 
        /// Called directly after the instance of this class has been created
        /// by ExtDirect4Dotnet. 
        /// 
        /// Used here to deserialze the entries.
        /// </summary>
        /// <param name="parameter"></param>
        public void afterCreation(HttpContext parameter)
        {
            httpContext = parameter;
            try
            {
                FileStream fs = new FileStream(httpContext.Server.MapPath("./guestbook/uploadedFiles/data.txt"), FileMode.Open, FileAccess.Read);

                BinaryFormatter bf = new BinaryFormatter();

                entries = (DataSet)bf.Deserialize(fs);
                fs.Close();
            }
            catch (Exception e)
            {
                entries = new DataSet();
                entries.Tables.Add("entries");
                entries.Tables["entries"].Columns.Add("id", typeof(int));
                entries.Tables["entries"].Columns.Add("firstName");
                entries.Tables["entries"].Columns.Add("lastName");
                entries.Tables["entries"].Columns.Add("pictureName");
                entries.Tables["entries"].Columns.Add("message");
                entries.Tables["entries"].Columns.Add("date", typeof(DateTime));
            }

        }

        #endregion

        #region IActionWithBeforeDestroy Member

        /// <summary>
        /// This method gets called by ExtDirect4DotNet just before the instance of this class
        /// gets destroyed and the respose will get send back to the client
        /// </summary>
        /// <param name="parameter"></param>
        public void beforeDestroy(HttpContext parameter)
        {
            string dataPath = httpContext.Server.MapPath("./guestbook/uploadedFiles/data.txt");
            FileStream fs = null;
            try
            {
                fs = new FileStream(dataPath, FileMode.Create, FileAccess.Write);

                BinaryFormatter bf = new BinaryFormatter();

                bf.Serialize(fs, entries);

            }
            catch (Exception e)
            {
            }
            finally
            {
                if (fs != null)
                    fs.Close();
            }


        }

        #endregion

        #endregion

        /// <summary>
        /// Small MD5 helper mehtod
        /// </summary>
        /// <param name="Value">string to compute via md5</param>
        /// <returns>the cmputed String</returns>
        private string HashString(string Value)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider x = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] data = System.Text.Encoding.ASCII.GetBytes(Value);
            data = x.ComputeHash(data);
            string ret = "";
            for (int i = 0; i < data.Length; i++)
                ret += data[i].ToString("x2").ToLower();
            return ret;
        }

        /// <summary>
        /// Checks if the given string contains any invalid carracter...
        /// If so it will return an errormessage otherwise it will return ""
        /// </summary>
        /// <param name="input">string to check</param>
        /// <returns>returns "" if the sting is valid otherwise an errormessage</returns>
        private string inputValidator(string input)
        {

            if (input.IndexOf('<') != -1)
            {
                return "Invalid character < found";
            }

            if (input.IndexOf('>') != -1)
            {
                return "Invalid character > found";
            }

            return "";
        }

        
        /// <summary>
        /// Creats a new primary key for a guestbook entrie
        /// </summary>
        /// <returns>the created Id</returns>
        private int getNewId() {
            if (this.entries.Tables[0].Rows.Count == 0)
            {
                return 0;
            }

            return ((int)this.entries.Tables[0].Select("MAX(id) = id")[0]["id"])+1;

        }
        
        /// <summary>
        /// Stores the ids of the entries a user created.
        /// Used to indicate if the user is allowed to delete the record.
        /// </summary>
        private Hashtable myEntries
        {
            set
            {
                httpContext.Session["myEntries"] = value;
            }
            get
            {
                if (httpContext.Session["myEntries"] == null)
                {
                    httpContext.Session["myEntries"] = new Hashtable();
                }
                return (Hashtable)httpContext.Session["myEntries"];
            }
        }

        /// <summary>
        /// indicats if the user is logged in
        /// </summary>
        private bool loggedIn
        {
            set
            {
                httpContext.Session["loggedin"] = value;

            }
            get
            {
                if (httpContext.Session["loggedin"] == null)
                {
                    httpContext.Session["loggedin"] = false;
                }
                return (bool)httpContext.Session["loggedin"];
            }
        }

    }
        
}
