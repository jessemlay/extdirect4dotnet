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
    /// An Abstract Class that adds Paging functionality to the Read Method of a CRUD action
    /// </summary>
    public abstract class SimpleCRUDWithPaging : SimpleCRUDWithSorting, IActionWithAfterInvoke
    {

        #region IActionWithAfterInvoke Member

        public object afterMethodInvoke(DirectMethodType methodType, string methodName, object theReturnedObject)
        {
            

            if (methodType == DirectMethodType.Read)
            {
               

                Hashtable storeResponse = (Hashtable)theReturnedObject;
                Object returnedByTheReadMethod = storeResponse[this.getMetaData().getRootPropertyName()];

                // if the sorting is supported for the given Object do Srting sorting...
                if (returnedByTheReadMethod is DataTable || returnedByTheReadMethod is DataView)
                {
                    theReturnedObject = base.afterMethodInvoke(methodType, methodName, theReturnedObject);
                    // just reset the references
                    storeResponse = (Hashtable)theReturnedObject;
                    returnedByTheReadMethod = storeResponse[this.getMetaData().getRootPropertyName()];
                }

                int start = 0;
                int limit = 0;


                if(StoreParameter[this.getMetaData().getPagingLimitPropertyName()] != null &&
                    StoreParameter[this.getMetaData().getPagingStartPropertyName()] != null)
                {
                    start = Int32.Parse(StoreParameter[this.getMetaData().getPagingStartPropertyName()].ToString());
                    limit = Int32.Parse(StoreParameter[this.getMetaData().getPagingLimitPropertyName()].ToString());
                }
                storeResponse[this.getMetaData().getRootPropertyName()] = this.extractPage(returnedByTheReadMethod, start, limit);
                return storeResponse;
            }
            return theReturnedObject;
        }

        protected List<Object> extractPage(Object objectToCast, int start, int limit)
        {
            if (objectToCast is DataTable)
            {
                return extractPage((DataTable)objectToCast, start, limit);
            }
            else if (objectToCast is DataView)
            {
                return extractPage((DataView)objectToCast, start, limit);
            }
            else if (objectToCast is DataRow[])
            {
                return extractPage((DataRow[])objectToCast, start, limit);
            }
            else if (objectToCast is DataRowCollection)
            {
                return extractPage((DataRowCollection)objectToCast, start, limit);
            }
            else if (objectToCast is IList)
            {
                IList il = objectToCast as IList;
                return extractPage(il, start, limit);
            }
            else
            {
                throw new Exception(objectToCast.GetType().Name + " is not supported by extractPage yet");
            }
        }

        /// <summary>
        /// Extracts a page from the given dataView
        /// </summary>
        /// <param name="dataRows">A DataView Containing all Records you want to page in</param>
        /// <param name="start">the index of the first Record of the page</param>
        /// <param name="limit">the number of records a Page can contain</param>
        /// <returns>returns a list of the DataRows iside the range</returns>
        protected List<Object> extractPage(DataView dataRows, int start, int limit)
        {
            List<Object> rowList = new List<Object>();
            DataRowView[] arr = new DataRowView[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rowList.AddRange(arr);

            return extractPage(rowList, start, limit);
        }

        /// <summary>
        /// Extracts a page from the given DataRow Array
        /// </summary>
        /// <param name="dataRows">An Array of dataRows Containing all Records you want to page in</param>
        /// <param name="start">the index of the first Record of the page</param>
        /// <param name="limit">the number of records a Page can contain</param>
        /// <returns>returns a list of the DataRows iside the range</returns>
        protected List<Object> extractPage(DataRow[] dataRows, int start, int limit)
        {
            List<Object> rowList = dataRows.ToList<Object>();
            return extractPage(rowList, start, limit);
        }

        /// <summary>
        /// Extracts a page from the given DataRowCollection Array
        /// </summary>
        /// <param name="dataRows">An Array of dataRows Containing all Records you want to page in</param>
        /// <param name="start">the index of the first Record of the page</param>
        /// <param name="limit">the number of records a Page can contain</param>
        /// <returns>returns a list of the DataRows iside the range</returns>
        protected List<Object> extractPage(DataRowCollection dataRows, int start, int limit)
        {
            List<Object> rowList = new List<Object>();
            DataRow[] arr = new DataRow[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rowList.AddRange(arr);
            return extractPage(rowList, start, limit);
        }

        protected List<Object> extractPage(DataTable dataTable, int start, int limit)
        {
            return extractPage(dataTable.DefaultView, start, limit);
        }

        private List<Object> extractPage(IList datarows, int start, int limit)
        {
            if (start == 0 && limit == 0)
            {
                limit = datarows.Count;
            }
            List<Object> pagedRows = new List<Object>();
            for (int i = start; i < datarows.Count && i < (start + limit); i++)
            {
                pagedRows.Add(datarows[i]);
            }
            return pagedRows;
        }

        #endregion
    }
}
