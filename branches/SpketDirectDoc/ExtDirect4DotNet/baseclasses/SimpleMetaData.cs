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
        protected Hashtable[] fields;
        public Hashtable[] getFieldDescription()
        {
            if (fields == null)
            {
                throw new NotImplementedException("you have to set the fields Propertie of the MetaData to use addMetaData = true");
            }
            return fields;
        }

        public string getPagingStartPropertyName()
        {
            return "start";
        }

        public string getPagingLimitPropertyName()
        {
            return "limit";
        }


        public string getGroupByPropertyName()
        {
            return "groupBy";
        }

        public string getGroupDirPropertyName()
        {
            return "groupDir";
        }


        public string getSortByPropertyName()
        {
            return "sort";
        }

        public string getSortDirPropertyName()
        {
            return "dir";
        }

        #endregion

        #region IMetaData Member


        public Hashtable toSerializible()
        {
            Hashtable metadata = new Hashtable();
            metadata.Add("idProperty", getIdPropertyName());
            metadata.Add("root", getRootPropertyName());
            metadata.Add("totalProperty", getTotalPropertyName());
            metadata.Add("successProperty",getSuccessPropertyName());
            metadata.Add("fields", getFieldDescription());
            return metadata;
        }

        #endregion
    }
}
