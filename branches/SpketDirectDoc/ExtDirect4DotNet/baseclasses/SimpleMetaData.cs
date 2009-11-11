using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtDirect4DotNet.interfaces;
using System.Collections;

namespace ExtDirect4DotNet.baseclasses
{
    public class SimpleMetaData : IMetaData
    {
        #region IMetaData Member

        public string getRootPropertyName()
        {
            return "rows";
        }

        public string getSuccessPropertyName()
        {
            return "success";
        }

        public string getTotalPropertyName()
        {
            return "results";
        }

        public string getIdPropertyName()
        {
            return "id";
        }

        public ArrayList getFieldDescription()
        {
            throw new NotImplementedException();
        }

        public string getPagingStartPropertyName()
        {
            return "start";
        }

        public string getPagingLimitPropertyName()
        {
            return "limit";
        }

        #endregion
    }
}
