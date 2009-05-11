using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Script.Serialization;
using ExtDirect;
using ExtDirect.Direct;
public class Employee
{
    public string id
    {
        get;
        set;
    }
    public string firstName
    {
        get;
        set;
    }
    public string lastName
    {
        get;
        set;
    }
    public string country
    {
        get;
        set;
    }
}
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

[DirectService("TestAction")]
public class TestAction
{
    [DirectMethod("doEcho")]
    public string doEcho(string data)
    {
        return "Server Return : " + data;
    }
    [DirectMethod("multiply")]
    public string multiply(string d1, string d2)
    {
        return (Convert.ToInt32(d1) * Convert.ToInt32(d2)).ToString();
    }
    [DirectMethod("submit",DirectAction.FormSubmission, MethodVisibility.Visible)]
    public string submit(string first, string last, string company, string email, string time)
    {
        System.Text.StringBuilder str = new System.Text.StringBuilder();
        str.Append("Name : " + first + "  " + last);
        str.Append(" Company : " + company);
        str.Append(" Email : " + email);
        str.Append(" Time : " + time);

        return str.ToString();
    }
    [DirectMethod("getData",DirectAction.Load,MethodVisibility.Visible)]
    public string getData()
    {

        //List<Employee> empList = new List<Employee>();
        List<Person> empList = new List<Person>();
        for (int i = 0; i < 20; i++)
        {
            empList.Add(new Person
            {
                id = i.ToString(),
                email = "check@hotmail.com",
                first = "ABC" + i.ToString(),
                last = "XYZ" + i.ToString(),


            });
        }
        JavaScriptSerializer jSri = new JavaScriptSerializer();
        var res = "{\"success\":true,\"data\":" + jSri.Serialize(empList) + "}";
        return res;
    }
    [DirectMethod("createData",DirectAction.Create, MethodVisibility.Visible)]
    public string createData(string email, string first, string last)
    {
        //return "{\"id\":\"100" + first.ToString() + "\"}";
        System.Text.StringBuilder str = new System.Text.StringBuilder();
        str.Append("{");
        str.Append("\"id\":\"" + 100 + first.ToString() + "\","); ;
        str.Append("\"email\":\"" + email + "\","); ;
        str.Append("\"first\":\"" + first + "\","); ;
        str.Append("\"last\":\"" + last + "\""); ;
        str.Append("}");
        return str.ToString();
    }
    [DirectMethod("deleteData",DirectAction.Delete, MethodVisibility.Visible)]
    public string deleteData(string id)
    {
        return "{\"success\":true}";
    }
    [DirectMethod("saveData", DirectAction.Save, MethodVisibility.Visible)]
    public string saveData(string id, string email, string first, string last)
    {
        System.Text.StringBuilder str = new System.Text.StringBuilder();
        str.Append("{");
        str.Append("\"id\":\"" + id + "\","); ;
        str.Append("\"email\":\"" + email + "\","); ;
        str.Append("\"first\":\"" + first + "\","); ;
        str.Append("\"last\":\"" + last + "\""); ;
        str.Append("}");
        return str.ToString();
       

    }

}

