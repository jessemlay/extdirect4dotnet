using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtDirect4DotNet.interfaces;
using System.Collections;
using System.Data;

namespace ExtDirect4DotNet.baseclasses
{

    /// <summary>
    /// Implements the Baseclass for An CRUDAction with Automatically response grouping
    /// </summary>
    public abstract class SimpleCRUDWithGrouping : SimpleCRUDWithSorting, IActionWithBeforeInvoke<Hashtable>
    {
        #region IActionWithAfterInvoke Member

        public object afterMethodInvoke(DirectMethodType methodType, string methodName, object theReturnedObject)
        {
            // do sorting first...
            theReturnedObject = base.afterMethodInvoke(methodType, methodName, theReturnedObject);

            if (methodType == DirectMethodType.Read)
            {
                Hashtable storeResponse = (Hashtable)theReturnedObject;
                Object returnedByTheReadMethod = storeResponse[this.getMetaData().getRootPropertyName()];

                string groupBy = "";
                string groupDir = "ASC";


                if (StoreParameter[this.getMetaData().getGroupByPropertyName()] != null )
                {
                    groupBy = StoreParameter[this.getMetaData().getGroupByPropertyName()].ToString();
                    if (StoreParameter[this.getMetaData().getGroupDirPropertyName()] != null)
                        groupDir = StoreParameter[this.getMetaData().getGroupDirPropertyName()].ToString();

                    storeResponse[this.getMetaData().getRootPropertyName()] = this.groupResult(returnedByTheReadMethod, groupBy, groupDir);
                }
                
                return storeResponse;
            }
            return theReturnedObject;
        }

        protected DataView groupResult(Object objectToCast, string groupBy, string groupDir)
        {
            if (objectToCast is DataTable)
            {
                return groupResult((DataTable)objectToCast, groupBy, groupDir);
            }
            else if (objectToCast is DataView)
            {
                return groupResult((DataView)objectToCast, groupBy, groupDir);
            }
            else
            {
                throw new Exception(objectToCast.GetType().Name + " is not supported by extractPage yet");
            }
        }

        /// <summary>
        /// Groups the given dataView by the groupBy field and the groupDir
        /// </summary>
        /// <param name="dataRows">A DataView Containing all Records you want to page in</param>
        /// <param name="start">the index of the first Record of the page</param>
        /// <param name="limit">the number of records a Page can contain</param>
        /// <returns>returns a list of the DataRows iside the range</returns>
        protected DataView groupResult(DataView dataRows, string groupBy, string groupDir)
        {
            string sortString = dataRows.Sort;
            sortString = sortString == "" ? groupBy + " " + groupDir : groupBy + " " + groupDir + ", " + sortString;
            dataRows.Sort = sortString;

            return dataRows;
        }

        protected DataView groupResult(DataTable dataTable, string groupBy, string groupDir)
        {
            return groupResult(dataTable.DefaultView, groupBy, groupDir);
        }

        #endregion
    }
}
