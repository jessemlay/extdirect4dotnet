using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ExtDirect4DotNet.interfaces
{

    /// <summary>
    /// Implement this Interface to have multiple instances of the extending Class
    /// per Request. 
    /// </summary>
    public interface IActionWithMultipleInstancesPerRequest
    {
        /// <summary>
        /// This Method gets called with the Parameter of an ExtDirect.Method invokation and
        /// should return an unique Id for this Class.
        /// 
        /// This is usefull if you have got One Class that implements
        /// the CRUD Logic for many diferent data pools
        /// 
        /// You may add a parameter to the store which you will use to route
        /// the CRUD methods. 
        /// 
        /// For example you could add a parameter "table" which tells the CRUD Class
        /// on which table you want to manipulate the data via the CRUD methods.
        /// 
        /// than you would implement the following method body:
        /// 
        /// return parameter["table"].toString();
        /// 
        /// in this case a method needs to be called with the parameter table.
        /// 
        /// The Proccessing Engin of Extdirect4DotNet will create an instance of the
        /// CRUD class for every table in the request.
        /// 
        /// </summary>
        /// <param name="parameter">The Parametrs of the Methodcall</param>
        /// <returns>an Identifier for the class instance</returns>
        string getIstanceId(Hashtable parameter);
    }
}
