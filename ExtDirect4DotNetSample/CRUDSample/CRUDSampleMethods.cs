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
using System.Collections.Generic;
using ExtDirect4DotNet.helper;
using ExtDirect4DotNet.baseclasses;
using System.Web.SessionState;
using ExtDirect4DotNet.interfaces;
using ExtDirect4DotNet.dataclasses;

namespace ExtDirect4DotNetSample
{

    public class Person
    {
        public string id
        {
            get;
            set;
        }
        public string email
        {
            get;
            set;
        }
        public string first
        {
            get;
            set;
        }
        public string last
        {
            get;
            set;
        }
    }

    /// <summary>
    /// A sample Ext.Direct - Action - Class
    /// 
    /// Class is markt as DirectAction via the Direct Action attribute [DirectAction]
    /// 
    /// The Class extends ActionWithSessionState wich makes the curren SessionState in this Action
    /// availible via the Session member.
    /// </summary>
    [DirectAction]
    public class CRUDSampleMethods : SimpleCRUDAction, IActionWithAfterCreation<HttpContext>, IActionWithBeforeDestroy
    {
        /// <summary>
        /// Just a small Id generator jusing the Session to store the highest id
        /// </summary>
        /// <returns>the Id</returns>
        private string generateId()
        {
           lastId = lastId + 1;
           return lastId+"";
        }

        /// <summary>
        /// Returns a (cached) List of Persons from the Session
        /// </summary>
        /// <returns>The List of Persons</returns>
        private List<Person> getData()
        {
            return getData(false);
        }

        /// <summary>
        /// Returns a (cached) List of Persons from the Session
        /// </summary>
        /// <param name="fresh">true to clear the Cache</param>
        /// <returns>The List of Persons</returns>
        private List<Person> getData(Boolean fresh)
        {
            return personList;
        }

        /// <summary>
        /// The C in Crud 
        /// Represents a properitary Logic for creating a Person Object
        /// Adds it to the List in the Session and after that it returns back the Person
        /// 
        /// This Methode has bin marked as a DirectMethod via the DirectMethod Attribute [DirectMethod]
        /// You can configure Different Options for this Methods by setting Properties in the Attribute.
        /// While this Method will get Used in a CRUD Process and some of those Methodes need special Handling
        /// the MethodeType is set To Create.
        /// 
        /// See the MethodType Doc for more information
        /// </summary>
        /// <param name="personToCreate">The Person that should get added to the List in the Session (represents a Record in Extjs)</param>
        /// <returns>The Person that was created</returns>
        [DirectMethod(MethodType = DirectMethodType.Create)]
        public Person create(Person personToCreate)
        {
            personToCreate.id = generateId();

            getData().Add(personToCreate);

            return personToCreate;
        }

        /// <summary>
        /// The R in cRud 
        /// Represents a properitary Logic for Reading.
        /// Will return a List Of Persons Wrapped by a LoadResponse Class
        /// 
        /// This Methode is marked as a DirectMethod via the DirectMethod Attribute [DirectMethod]
        /// You can configure Different Options for this Methods by setting Properties in the Attribute.
        /// 
        /// Special thing here is the ParameterHandling. 
        /// Which is set to AutoResolve her. This means that you can call the Function form the Javascript Side
        /// like this:
        /// 
        /// CRUDSampleMethods.read({sort: 'email', dir:'ASC'})
        /// 
        /// and the DirectMethod Class will try to mapp those Parameters to the Parameters of the .Net Function
        /// 
        /// So this Function will get call as you would call it in .Net the following way.
        /// 
        /// read("email","ASC")
        /// </summary>
        /// <param name="sort">the property to Sort the List of Persons by</param>
        /// <param name="dir">The Direction or "ASC"/"DESC"</param>
        /// <returns>A LoadRespone Object that wraps the Persons</returns>
        [DirectMethod(MethodType = DirectMethodType.Read, ParameterHandling = ParameterHandling.AutoResolve)]
        public List<Person> read(string sort, string dir, int start, int limit)
        {

            List<Person> rows = getData();

            switch (sort)
            {
                case "email":
                    rows.Sort(delegate(Person p1, Person p2)
                    {
                        return p1.email.CompareTo(p2.email);
                    });
                    break;
                case "first":
                    rows.Sort(delegate(Person p1, Person p2)
                    {
                        return p1.first.CompareTo(p2.first);
                    });
                    break;
                case "last":
                default:
                    rows.Sort(delegate(Person p1, Person p2)
                    {
                        return p1.last.CompareTo(p2.last);
                    });
                    break;


            }

            if (dir != "ASC")
            {
                rows.Reverse();
            }

            if (start != null && limit != null)
            {
                List<Person> returnList = new List<Person>(); 
                int i = 0;
                foreach (Person curPers in rows)
                {
                    if (i >= start && i <= (start + limit))
                    {
                        returnList.Add(curPers);
                    }
                    i++;
                }
                return rows;
            }

            return rows;
        }

        /// <summary>
        /// The U in crUd 
        /// Represents a properitary Logic for Updating an existing Person in the List.
        /// 
        /// This Methode is marked as a DirectMethod via the DirectMethod Attribute [DirectMethod]
        /// You can configure Different Options for this Methods by setting Properties in the Attribute.
        /// 
        /// The MethodType is Set to Uptdate here.
        /// This means a Special Parameterhandling as well.
        /// The Method will get Called from Extjs in the following Way:
        /// 
        /// update(1,{RECORD});
        /// 
        /// DirectMethodType.Update 
        /// 
        /// Will Pass that into your Function
        /// </summary>
        /// <param name="sort">the property to Sort the List of Persons by</param>
        /// <param name="dir">The Direction or "ASC"/"DESC"</param>
        /// <returns>A LoadRespone Object that wraps the Persons</returns>
        [DirectMethod(MethodType=DirectMethodType.Update)]
        public Person update(Person personWithUpdatedValues)
        {
            string id = personWithUpdatedValues.id;
            List<Person> persons = getData();
            Person person = persons.Find(t => t.id == id);

            // update logic
            if (personWithUpdatedValues.last != null)
            {
                person.last = personWithUpdatedValues.last;
            }

            if (personWithUpdatedValues.first != null)
            {
                person.first = personWithUpdatedValues.first;
            }

            if (personWithUpdatedValues.email != null)
            {
                person.email = personWithUpdatedValues.email;
            }

            return person;
        }

        /// <summary>
        /// The D in cruD 
        /// Represents a properitary Logic for Deleting an existing Person in the List.
        /// 
        /// This Methode is marked as a DirectMethod via the DirectMethod Attribute [DirectMethod]
        /// You can configure Different Options for this Methods by setting Properties in the Attribute.
        /// 
        /// The MethodType is Set to Delete here.
        /// 
        /// An nother Special thing here is the OutputHandling Parameter
        /// This ensures that the Followin Proccess Logic will not rerender the result of this methods
        /// 
        /// Make Sure that the ToString Function of this Function returns Clean JSON if you want to use this Method!
        /// </summary>
        /// <param name="id">Id Of the Person which should get deleted</param>
        /// <returns>just retunrs Success to tell the store that the record was deleted on the Server.</returns>
        [DirectMethod(MethodType = DirectMethodType.Delete )]
        public bool destroy(string id)
        {
            List<Person> persons = getData();
            Person person = persons.Find(t => t.id == id);
            persons.Remove(person);

            return true;
        }

        /// <summary>
        /// just a small function that resets the content of the Session Object
        /// </summary>
        [DirectMethod]
        public void reset()
        {
            getData(true);
        }

        private List<Person> personList;
        private int lastId = 0;

        #region IActionWithAfterCreation<HttpContext> Member

        /// <summary>
        /// This method gets called once per request. We use it here to acces the data from the Sesseion
        /// </summary>
        /// <param name="parameter">Cause we set up HttpContext as Parameter it gets filled with the current
        /// http Context from the actual request</param>
        public void afterCreation(HttpContext parameter)
        {

            personList = (List<Person>)parameter.Session["CRUDMethodsData"];
            if (personList == null)
            {
                personList = new List<Person>();


                Person p1 = new Person() { first = "Martin", last = "Späth", email = "email1@extjs.com", id = "1" };
                personList.Add(p1);


                Person p2 = new Person() { first = "Heinz", last = "Erhart", email = "email2@extjs.com", id = "2" };
                personList.Add(p2);

                Person p3 = new Person() { first = "Albert", last = "Einstein", email = "email1@extjs.com", id = "3" };
                personList.Add(p3);
                parameter.Session["CRUDMethodsData"] = personList;
                parameter.Session["lastId"] = 3;

            }

            lastId = (int)parameter.Session["lastId"];


        }

        #endregion

        #region IActionWithBeforeDestroy Member

        /// <summary>
        /// This method gets called once per request just before this instance of the class gets destroyed
        /// we use it her to write data back to the session
        /// </summary>
        public void beforeDestroy(HttpContext parameter)
        {
            // write the person list back to the session object
            parameter.Session["CRUDMethodsData"] = personList;
            // write back the last id as well.
            parameter.Session["lastId"] = lastId;
        }

        #endregion

        public override int getResultCount()
        {
            return personList.Count;
        }

        public override bool addMetaData()
        {
            return false;
        }
    }
}
