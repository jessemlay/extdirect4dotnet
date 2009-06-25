using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using ExtDirect4DotNet.customJsonConverter;
using Newtonsoft.Json;

namespace ExtDirect4DotNet.responsewrapper
{
    public class StoreLoadResponseWrapper
    {
        [JsonProperty]
        public List<Object> rows;

        [JsonProperty]
        public int results;

        public StoreLoadResponseWrapper(DataRowCollection dataRows)
        {
            rows = new List<Object>();
            DataRow[] arr = new DataRow[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            results = rows.Count;
        }

        public StoreLoadResponseWrapper(DataRowCollection dataRows, int count)
        {
            rows = new List<Object>();
            DataRow[] arr = new DataRow[dataRows.Count];
            dataRows.CopyTo(arr,0);
            rows.AddRange(arr);
            results = count;
        }

        public StoreLoadResponseWrapper(DataRowCollection dataRows, int start, int limit)
        {
            rows = new List<Object>();
            DataRow[] arr = new DataRow[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            results = rows.Count;
            rows = page(rows, start, limit);
        }

        public StoreLoadResponseWrapper(DataRowCollection dataRows, int start, int limit, int count)
        {
            rows = new List<Object>();
            DataRow[] arr = new DataRow[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            rows = page(rows, start, limit);
            results = count;
        }


        public StoreLoadResponseWrapper(DataView dataRows)
        {
            rows = new List<Object>();
            DataRowView[] arr = new DataRowView[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            results = rows.Count;
        }

        public StoreLoadResponseWrapper(DataView dataRows, int count)
        {
            rows = new List<Object>();
            DataRowView[] arr = new DataRowView[ dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            results = count;
        }

        public StoreLoadResponseWrapper(DataView dataRows, int start, int limit)
        {
            rows = new List<Object>();
            DataRowView[ ] arr = new DataRowView[ dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            results = rows.Count;
            rows = page(rows, start, limit);
        }

        public StoreLoadResponseWrapper(DataView dataRows, int start, int limit, int count)
        {
            rows = new List<Object>();
            DataRowView[] arr = new DataRowView[dataRows.Count];
            dataRows.CopyTo(arr, 0);
            rows.AddRange(arr);
            rows = page(rows, start, limit);
            results = count;
        }


        public StoreLoadResponseWrapper(DataRow[] dataRows)
        {
            rows = dataRows.ToList<Object>();
            results = dataRows.Length;
        }

        public StoreLoadResponseWrapper(DataRow[] dataRows, int count)
        {
            rows = dataRows.ToList<Object>();
            results = count;
        }

        public StoreLoadResponseWrapper(DataRow[] dataRows, int start, int limit)
        {
            rows = dataRows.ToList<Object>();
            rows = page(rows, start, limit);
            results = dataRows.Length;
        }

        public StoreLoadResponseWrapper(DataRow[] dataRows, int start, int limit, int count)
        {
            rows = dataRows.ToList<Object>();
            rows = page(rows, start, limit);
            results = count;
        }

        private List<Object> page(List<Object> datarows, int start, int limit)
        {
            List<Object> pagedRows = new List<Object>();
            for (int i = start; i < datarows.Count && i < (start + limit); i++)
            {
                pagedRows.Add(datarows[i]);
            }
            return pagedRows;
        }

    }
}
