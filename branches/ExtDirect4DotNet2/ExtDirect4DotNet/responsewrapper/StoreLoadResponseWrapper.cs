using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ExtDirect4DotNet.customJsonConverter;
using Newtonsoft.Json;
using System.Collections;

namespace ExtDirect4DotNet.responsewrapper
{
    public class StoreLoadResponseWrapper
    {
        [JsonProperty]
        public List<Object> rows;

        [JsonProperty]
        public int results;

        [JsonProperty]
        public Object additionalData = null;

        [JsonProperty]
        public bool comitted = true;
        
        [JsonProperty]
        public Hashtable metaData = null;

        protected StoreLoadResponseWrapper()
        {
        }

        /// <summary>
        /// Creates a StoreLoadResponseWrapper from the assigned dataRows
        /// </summary>
        /// <param name="dataRows">dataRows the StoreLoadResponseWrapper shoud contain</param>
        /// <param name="addMetaData">generate and add Metadata?</param>
        public StoreLoadResponseWrapper(DataRowCollection dataRows, bool addMetaData)
        {
            rows = new List<Object>();
            DataRow[] arr = new DataRow[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            if (arr.Length > 0)
            {
                this.comitted = !arr[0].Table.DataSet.HasChanges();
                if (addMetaData)
                {
                    addMetaDataWrapper(arr[0].Table);
                }
            }
            results = rows.Count;
        }


        public StoreLoadResponseWrapper(DataRowCollection dataRows, int count, bool addMetaData)
        {
            rows = new List<Object>();
            DataRow[] arr = new DataRow[dataRows.Count];
            dataRows.CopyTo(arr,0);
            rows.AddRange(arr);
            if (arr.Length > 0)
            {
                this.comitted = !arr[0].Table.DataSet.HasChanges();
                if (addMetaData)
                {
                    addMetaDataWrapper(arr[0].Table);
                }
            }
            results = count;
        }

        public StoreLoadResponseWrapper(DataRowCollection dataRows, int start, int limit, bool addMetaData)
        {
            rows = new List<Object>();
            DataRow[] arr = new DataRow[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            if (arr.Length > 0)
            {
                this.comitted = !arr[0].Table.DataSet.HasChanges();
                if (addMetaData)
                {
                    addMetaDataWrapper(arr[0].Table);
                }
            }
            results = rows.Count;
            rows = page(rows, start, limit);
        }

        public StoreLoadResponseWrapper(DataRowCollection dataRows, int start, int limit, int count, bool addMetaData)
        {
            
            rows = new List<Object>();
            DataRow[] arr = new DataRow[dataRows.Count];
            dataRows.CopyTo(arr, 0);

            
            rows.AddRange(arr);
            if (arr.Length > 0)
            {
                this.comitted = !arr[0].Table.DataSet.HasChanges();
                if (addMetaData)
                {
                    addMetaDataWrapper(arr[0].Table);
                }
            }
            rows = page(rows, start, limit);
            results = count;
        }


        public StoreLoadResponseWrapper(DataView dataRows, bool addMetaData)
        {
            rows = new List<Object>();
            DataRowView[] arr = new DataRowView[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            if (dataRows.Count > 0)
            {
                if (dataRows.Table.DataSet != null)
                {

                    this.comitted = !dataRows.Table.DataSet.HasChanges();
                }
                else
                {
                    this.comitted = true;
                }
                
            }
            if (addMetaData)
            {
                addMetaDataWrapper(dataRows.Table);
            }
            results = rows.Count;
        }

        public StoreLoadResponseWrapper(DataView dataRows, int count, bool addMetaData)
        {
            rows = new List<Object>();
            DataRowView[] arr = new DataRowView[ dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            if (dataRows.Count > 0)
            {

                this.comitted = !dataRows.Table.DataSet.HasChanges(); ;
            }
            if (addMetaData)
            {
                addMetaDataWrapper(dataRows.Table);
            }
            results = count;
        }

        public StoreLoadResponseWrapper(DataView dataRows, int start, int limit, bool addMetaData)
        {
            rows = new List<Object>();
            DataRowView[ ] arr = new DataRowView[ dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            if (dataRows.Count > 0)
            {

                this.comitted = !dataRows.Table.DataSet.HasChanges(); ;
            }
            if (addMetaData)
            {
                addMetaDataWrapper(dataRows.Table);
            }
            results = rows.Count;
            rows = page(rows, start, limit);
        }

        public StoreLoadResponseWrapper(DataView dataRows, int start, int limit, int count, bool addMetaData)
        {
            rows = new List<Object>();
            DataRowView[] arr = new DataRowView[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            if (dataRows.Count > 0)
            {

                this.comitted = !dataRows.Table.DataSet.HasChanges(); ;
            }
            if (addMetaData)
            {
                addMetaDataWrapper(dataRows.Table);
            }
            rows = page(rows, start, limit);
            results = count;
        }


        public StoreLoadResponseWrapper(DataRow[] dataRows, bool addMetaData)
        {
            if (dataRows.Length > 0)
            {
                this.comitted = !dataRows[0].Table.DataSet.HasChanges();
                if (addMetaData)
                {
                    addMetaDataWrapper(dataRows[0].Table);
                }
            }
            rows = dataRows.ToList<Object>();
            results = dataRows.Length;
        }

        public StoreLoadResponseWrapper(DataRow[] dataRows, int count, bool addMetaData)
        {
            if (dataRows.Length > 0)
            {
                this.comitted = !dataRows[0].Table.DataSet.HasChanges();
                if (addMetaData)
                {
                    addMetaDataWrapper(dataRows[0].Table);
                }
            }
            rows = dataRows.ToList<Object>();
            results = count;
        }

        public StoreLoadResponseWrapper(DataRow[] dataRows, int start, int limit, bool addMetaData)
        {
            if (dataRows.Length > 0)
            {
                this.comitted = !dataRows[0].Table.DataSet.HasChanges();
                if (addMetaData)
                {
                    addMetaDataWrapper(dataRows[0].Table);
                }
            }
            rows = dataRows.ToList<Object>();
            rows = page(rows, start, limit);
            results = dataRows.Length;
        }

        public StoreLoadResponseWrapper(DataRow[] dataRows, int start, int limit, int count, bool addMetaData)
        {
            if (dataRows.Length > 0)
            {
                this.comitted = !dataRows[0].Table.DataSet.HasChanges();
                if (addMetaData)
                {
                    addMetaDataWrapper(dataRows[0].Table);
                }
            }
            rows = dataRows.ToList<Object>();
            rows = page(rows, start, limit);
            results = count;
        }

        /// <summary>
        /// adds the Meta-Data to this StoreLoadResponseWrapper
        /// </summary>
        /// <param name="dataTable">The DataTable to generate the metaData from</param>
        private void addMetaDataWrapper(DataTable dataTable)
        {
            this.metaData = new Hashtable();
            this.metaData.Add("root", "rows");
            this.metaData.Add("totalProperty", "results");
            this.metaData.Add("idProperty", dataTable.ExtendedProperties["primaryKeyColumn"]);

            ArrayList fields = new ArrayList();
            foreach(DataColumn dc in dataTable.Columns) {
                // TODO add DataType
                if (dc.ColumnName == "PASSNR")
                    continue;

                Hashtable field = new Hashtable();
                field.Add("name", dc.ColumnName);
                fields.Add(field);
                mapType(dc.DataType,ref field);
                
                int maxLength = 0;
                if(dc.ExtendedProperties.Contains("maxlength")) {
                    maxLength = int.Parse(dc.ExtendedProperties["maxlength"].ToString());
                } else {
                    maxLength = dc.MaxLength;
                }
                if(maxLength > 0)
                    field.Add("maxLength",maxLength);
            }

            this.metaData.Add("fields", fields);
            // TODO add success Property
            //this.metaData.add("success", "success");

            // TODO add sortInfo
            // TODO Add pagingInfo
        }

        protected void mapType(Type type, ref Hashtable ht)
        {
            if (type == typeof(System.DateTime))
                ht.Add("type", "date");

        }

        protected List<Object> page(List<Object> datarows, int start, int limit)
        {
            /*
            if (datarows.Count > 0)
            {
                // FIX ME vieleicht geht das ja so nicht da object nicht zwansläufig eine Table Propertie hat...
                this.comitted = !datarows[0].Table.DataSet.HasChanges();
            }*/
            List<Object> pagedRows = new List<Object>();
            for (int i = start; i < datarows.Count && (i < (start + limit) || limit == 0); i++)
            {
                pagedRows.Add(datarows[i]);
            }
            return pagedRows;
        }

    }
}
