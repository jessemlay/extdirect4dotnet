using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using Newtonsoft.Json;

namespace ExtDirect4DotNet.responsewrapper {
    public class StoreLoadResponseWrapper {
        [JsonProperty]
        public Object additionalData;

        [JsonProperty]
        public bool comitted = true;

        [JsonProperty]
        public int results;

        [JsonProperty]
        public List<Object> rows;

        public StoreLoadResponseWrapper(DataRowCollection dataRows) {
            rows = new List<Object>();
            DataRow[] arr = new DataRow[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            if (arr.Length > 0) {
                comitted = !arr[0].Table.DataSet.HasChanges();
            }
            results = rows.Count;
        }

        public StoreLoadResponseWrapper(DataRowCollection dataRows, int count) {
            rows = new List<Object>();
            DataRow[] arr = new DataRow[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            if (arr.Length > 0) {
                comitted = !arr[0].Table.DataSet.HasChanges();
            }
            results = count;
        }

        public StoreLoadResponseWrapper(DataRowCollection dataRows, int start, int limit) {
            rows = new List<Object>();
            DataRow[] arr = new DataRow[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            if (arr.Length > 0) {
                comitted = !arr[0].Table.DataSet.HasChanges();
            }
            results = rows.Count;
            rows = page(rows, start, limit);
        }

        public StoreLoadResponseWrapper(DataRowCollection dataRows, int start, int limit, int count) {
            rows = new List<Object>();
            DataRow[] arr = new DataRow[dataRows.Count];
            dataRows.CopyTo(arr, 0);

            rows.AddRange(arr);
            if (arr.Length > 0) {
                comitted = !arr[0].Table.DataSet.HasChanges();
            }
            rows = page(rows, start, limit);
            results = count;
        }

        public StoreLoadResponseWrapper(DataView dataRows) {
            rows = new List<Object>();
            DataRowView[] arr = new DataRowView[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            if (dataRows.Count > 0) {
                comitted = !dataRows.Table.DataSet.HasChanges();
                ;
            }
            results = rows.Count;
        }

        public StoreLoadResponseWrapper(DataView dataRows, int count) {
            rows = new List<Object>();
            DataRowView[] arr = new DataRowView[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            if (dataRows.Count > 0) {
                comitted = !dataRows.Table.DataSet.HasChanges();
                ;
            }
            results = count;
        }

        public StoreLoadResponseWrapper(DataView dataRows, int start, int limit) {
            rows = new List<Object>();
            DataRowView[] arr = new DataRowView[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            if (dataRows.Count > 0) {
                comitted = !dataRows.Table.DataSet.HasChanges();
                ;
            }
            results = rows.Count;
            rows = page(rows, start, limit);
        }

        public StoreLoadResponseWrapper(DataView dataRows, int start, int limit, int count) {
            rows = new List<Object>();
            DataRowView[] arr = new DataRowView[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            if (dataRows.Count > 0) {
                comitted = !dataRows.Table.DataSet.HasChanges();
                ;
            }
            rows = page(rows, start, limit);
            results = count;
        }

        public StoreLoadResponseWrapper(DataRow[] dataRows) {
            if (dataRows.Length > 0) {
                comitted = !dataRows[0].Table.DataSet.HasChanges();
            }
            rows = dataRows.ToList<Object>();
            results = dataRows.Length;
        }

        public StoreLoadResponseWrapper(DataRow[] dataRows, int count) {
            if (dataRows.Length > 0) {
                comitted = !dataRows[0].Table.DataSet.HasChanges();
            }
            rows = dataRows.ToList<Object>();
            results = count;
        }

        public StoreLoadResponseWrapper(DataRow[] dataRows, int start, int limit) {
            if (dataRows.Length > 0) {
                comitted = !dataRows[0].Table.DataSet.HasChanges();
            }
            rows = dataRows.ToList<Object>();
            rows = page(rows, start, limit);
            results = dataRows.Length;
        }

        public StoreLoadResponseWrapper(DataRow[] dataRows, int start, int limit, int count) {
            if (dataRows.Length > 0) {
                comitted = !dataRows[0].Table.DataSet.HasChanges();
            }
            rows = dataRows.ToList<Object>();
            rows = page(rows, start, limit);
            results = count;
        }

        private List<Object> page(List<Object> datarows, int start, int limit) {
            /*
            if (datarows.Count > 0)
            {
                // FIX ME vieleicht geht das ja so nicht da object nicht zwansläufig eine Table Propertie hat...
                this.comitted = !datarows[0].Table.DataSet.HasChanges();
            }*/
            List<Object> pagedRows = new List<Object>();
            for (int i = start; i < datarows.Count && i < (start + limit); i++) {
                pagedRows.Add(datarows[i]);
            }
            return pagedRows;
        }
    }
}