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
        public DataRowCollection rows;

        [JsonProperty]
        public int results;

        public StoreLoadResponseWrapper(DataRowCollection dataRows)
        {
            rows = dataRows;
            results = dataRows.Count;
        }

        public StoreLoadResponseWrapper(DataRowCollection dataRows, int count)
        {
            rows = dataRows;
            results = count;
        }
    }
}
