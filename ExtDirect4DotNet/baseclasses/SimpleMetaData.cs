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

        public string rootPropertyName = "rows";

        public string getRootPropertyName()
        {
            return rootPropertyName;
        }

        public string successPropertyName = "success";
        public string getSuccessPropertyName()
        {
            return successPropertyName;
        }

        public string totalPropertyName = "results";
        public string getTotalPropertyName()
        {
            return totalPropertyName;
        }

        public string idPropertyName = "id";
        public string getIdPropertyName()
        {
            return idPropertyName;
        }
        public Hashtable[] fields;
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

        public Hashtable sortInfo = new Hashtable();

        public Hashtable getSortInfo()
        {
            return sortInfo;
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
            metadata.Add("sortInfo", getSortInfo());

            return metadata;
        }

        #endregion

        #region IMetaData Member
        public Hashtable additionalData = new Hashtable();

        public Hashtable getAdditionalData()
        {
            return additionalData;
        }

        #endregion
    }
}
