using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace ExtDirect4DotNet.helper
{
    /// <summary>
    /// Just a small wrapper class for the correct response for a read Action.
    /// </summary>
    public class LoadResponse
    {

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


        [JsonProperty(PropertyName = "data")]
        public Object Rows
        {
            get;
            set;
        }
    }
}
