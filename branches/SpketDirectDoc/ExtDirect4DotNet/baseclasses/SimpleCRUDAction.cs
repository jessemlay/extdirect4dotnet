using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections;
using ExtDirect4DotNet.dataclasses;
using ExtDirect4DotNet.baseclasses;
using ExtDirect4DotNet.interfaces;

namespace ExtDirect4DotNet.baseclasses
{
    public abstract class SimpleCRUDAction : IActionWithBeforeInvoke<Hashtable>, ICRUDAction<SimpleMetaData>
    {
        /// <summary>
        /// Contains all parameter the store sent additionally you can acces them via StoreParameter["parametername"]
        /// </summary>
        protected Hashtable StoreParameter;


        #region IActionWithBeforeInvoke<DirectRequest> Member

        public void beforeMethodInvoke(DirectMethodType methodType, string methodName, Hashtable storeParameter)
        {
            StoreParameter = storeParameter;
        }

        #endregion

        #region ICRUDAction<SimpleMetaData> Member

        public SimpleMetaData getMetaData()
        {
            return new SimpleMetaData();
        }




        public abstract int getResultCount();


        public abstract bool addMetaData();

        #endregion
    }
}
