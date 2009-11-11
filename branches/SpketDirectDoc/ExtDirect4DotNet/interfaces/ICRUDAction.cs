using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;

namespace ExtDirect4DotNet.interfaces
{
    public interface ICRUDAction<T> where T : IMetaData
    {

        T getMetaData();

        /// <summary>
        /// Used by the direct method to add the Result count to the response object
        /// </summary>
        /// <returns>The nuber of total Records (not the number of records Response. Used for paging)</returns>
        int getResultCount();
        
        /// <summary>
        /// used by the DirectMethod as indicator if it sould add the MetaData to a read response
        /// </summary>
        /// <returns>return True to let direct Method add the metaData to the reponseobject after invoking a as READ marked methdo</returns>
        bool addMetaData();
    }
}
