using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtDirect4DotNet.dataclasses
{
    public class CRUDMetaData
    {
        public string idPropertiy = "id";
        public string root = "rows";
        public string totalProperty = "results";
        public string successProperty = "success";
        public CRUDFieldDefinition[] fields = null;

    }
}
