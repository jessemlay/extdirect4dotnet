using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ExtDirect4DotNet.dataclasses;

namespace ExtDirect4DotNet.baseclasses
{
    interface ICRUDActionWithMetaData 
    {
        /// <summary>
        /// Used by the DirectMethod to add the MetaData provided by this method to the response of a READ call
        /// </summary>
        CRUDMetaData getMetaData();
    }
}
