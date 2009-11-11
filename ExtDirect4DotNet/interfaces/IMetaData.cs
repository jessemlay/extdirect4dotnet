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
        ArrayList getFieldDescription();
    }
}
