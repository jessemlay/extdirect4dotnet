using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using System.Data;
using ExtDirect4DotNet.interfaces;

namespace ExtDirect4DotNet.baseclasses
{
    
    public abstract class SimpleCRUDWithSorting : SimpleCRUDAction, IActionWithBeforeInvoke<Hashtable>
    {

        #region IActionWithAfterInvoke Member

        public object afterMethodInvoke(DirectMethodType methodType, string methodName, object theReturnedObject)
        {

            if (methodType == DirectMethodType.Read)
            {
                Hashtable storeResponse = (Hashtable)theReturnedObject;
                Object returnedByTheReadMethod = storeResponse[this.getMetaData().getRootPropertyName()];

                string sortBy = "";
                string sortDir = "ASC";


                if (StoreParameter[this.getMetaData().getSortByPropertyName()] != null)
                {
                    sortBy = StoreParameter[this.getMetaData().getSortByPropertyName()].ToString();
                    if (StoreParameter[this.getMetaData().getSortDirPropertyName()] != null)
                        sortDir = StoreParameter[this.getMetaData().getSortDirPropertyName()].ToString();

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
        /// <param name="dataRows">A DataView Containing all Records you want to page in</param>
        /// <param name="start">the index of the first Record of the page</param>
        /// <param name="limit">the number of records a Page can contain</param>
        /// <returns>returns a list of the DataRows iside the range</returns>
        protected DataView sortResult(DataView dataRows, string sortBy, string sortDir)
        {
            string sortString = dataRows.Sort;
            sortString = sortBy + " " + sortDir;
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
