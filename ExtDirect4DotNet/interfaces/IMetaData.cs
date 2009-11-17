using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;

namespace ExtDirect4DotNet.interfaces
{
    public interface IMetaData
    {
        string getRootPropertyName();
        string getTotalPropertyName();
        string getIdPropertyName();
        string getSuccessPropertyName();
        string getPagingStartPropertyName();
        string getPagingLimitPropertyName();
        string getSortByPropertyName();
        string getSortDirPropertyName();
        string getGroupByPropertyName();
        string getGroupDirPropertyName();
        ArrayList getFieldDescription();

        Hashtable toSerializible();
    }
}
