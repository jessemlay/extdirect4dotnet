using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtDirect.Direct
{
    public class Request
    {
        public string action
        {
            get;
            set;
        }
        public string method
        {
            get;
            set;
        }
        public string[] data
        {
            get;
            set;
        }
        public int tid
        {
            get;
            set;
        }
    }
}
