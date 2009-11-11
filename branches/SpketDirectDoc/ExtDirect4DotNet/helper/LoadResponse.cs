using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using System.Data;
using System.Collections;

namespace ExtDirect4DotNet.helper
{
    /// <summary>
    /// Just a small wrapper class for the correct response for a read Action.
    /// </summary>
    public class LoadResponse
    {
        public LoadResponse() { }
        public LoadResponse(DataTable dataTable)
        {
            DataRow[] arr = new DataRow[ dataTable.Rows.Count];
            dataTable.Rows.CopyTo(arr, 0);

            Rows = arr;
            Results = dataTable.Rows.Count;
        }

        [JsonProperty(PropertyName = "success")]
        public bool Success = true;

        /// <summary>
        /// the property which contains the total dataset size (optional)
        /// </summary>
        [JsonProperty(PropertyName="results")]
        public int Results
        {
            get;
            set;
        }


        [JsonProperty(PropertyName = "rows")]
        public Object Rows
        {
            get;
            set;
        }
    }
}
