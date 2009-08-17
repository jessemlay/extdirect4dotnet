using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExtDirect4DotNet
{

    /// <summary>
    /// Thrown when an error occurs during a Ext.Direct call.
    /// </summary>
    public class DirectException : ApplicationException
    {

        public DirectException()
            : base()
        {
        }

        public DirectException(string msg)
            : base(msg)
        {
        }


        public DirectException(string msg, DirectRequest directRequest)
            : base(msg+" DirectAction: "+ directRequest.Action + " DirectMethod "+ directRequest.Method )
        {
        }

        public uint errorCode;

    }

    /// <summary>
    /// Thrown when an error occurs during a Ext.Direct call.
    /// </summary>
    public class DirectParameterException : DirectException
    {

        public DirectParameterException()
            : base()
        {
        }

        public DirectParameterException(string msg, DirectRequest directRequest)
            : base( msg,  directRequest)
        {
        }

    }
}
