using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using ExtDirect4DotNet.interfaces;

namespace ExtDirect4DotNet.baseclasses
{
    
    public abstract class SimpleCRUDWithSorting : SimpleCRUDAction, IActionWithAfterInvoke
    {


        protected string defaultSortBy = "";
        protected string defaultSortDir = "ASC";

        #region IActionWithAfterInvoke Member

        public object afterMethodInvoke(DirectMethodType methodType, string methodName, object theReturnedObject)
        {

            if (methodType == DirectMethodType.Read)
            {
                Hashtable storeResponse = (Hashtable)theReturnedObject;
                Object returnedByTheReadMethod = storeResponse[this.getMetaData().getRootPropertyName()];

                string sortBy = StoreParameter[this.getMetaData().getSortByPropertyName()] != null ? StoreParameter[this.getMetaData().getSortByPropertyName()].ToString() : defaultSortBy;
                


                if (sortBy != "")
                {
                    string sortDir = StoreParameter[this.getMetaData().getSortDirPropertyName()] != null? StoreParameter[this.getMetaData().getSortDirPropertyName()].ToString() : defaultSortDir;
                
                    storeResponse[this.getMetaData().getRootPropertyName()] = this.sortResult(returnedByTheReadMethod, sortBy, sortDir);
                }

                return storeResponse;
            }
            return theReturnedObject;
        }

        protected DataView sortResult(Object objectToCast, string sortBy, string sortDir)
        {
            if (objectToCast is DataTable)
            {
                return sortResult((DataTable)objectToCast, sortBy, sortDir);
            }
            else if (objectToCast is DataView)
            {
                return sortResult((DataView)objectToCast, sortBy, sortDir);
            }
            else
            {
                throw new Exception(objectToCast.GetType().Name + " is not supported by sortResult yet");
            }
        }

        /// <summary>
        /// Groups the given dataView by the groupBy field and the groupDir
        /// </summary>
        /// <param name="dataRows">A DataView Containing all Records you want to sort</param>
        /// <param name="sortBy">the field to sort in</param>
        /// <param name="sortDir">the direction to sort by ASC | DESC</param>
        /// <returns>returns a sorted list of the DataRows </returns>
        protected DataView sortResult(DataView dataRows, string sortBy, string sortDir)
        {
            string sortString = dataRows.Sort;
            sortString = sortBy + " " + sortDir;
            if (dataRows.Table.Columns[sortBy] != null)
                dataRows.Sort = sortString;

            return dataRows;
        }

        protected DataView sortResult(DataTable dataTable, string sortBy, string sortDir)
        {
            return sortResult(dataTable.DefaultView, sortBy, sortDir);
        }


        #endregion
    }
}
