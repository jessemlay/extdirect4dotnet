using System;
using System.Data;
using System.Linq;
using Newtonsoft.Json;

namespace ExtDirect4DotNet.helper {
    /// <summary>
    /// Just a small wrapper class for the correct response for a read Action.
    /// </summary>
    public class LoadResponse {
        [JsonProperty(PropertyName = "success")]
        public bool Success = true;

        public LoadResponse() {
        }

        public LoadResponse(DataTable dataTable) {
            DataRow[] arr = new DataRow[dataTable.Rows.Count];
            dataTable.Rows.CopyTo(arr, 0);

            Rows = arr;
            Results = dataTable.Rows.Count;
        }

        /// <summary>
        /// the property which contains the total dataset size (optional)
        /// </summary>
        [JsonProperty(PropertyName = "results")]
        public int Results { get; set; }

        [JsonProperty(PropertyName = "data")]
        public Object Rows { get; set; }
    }
}