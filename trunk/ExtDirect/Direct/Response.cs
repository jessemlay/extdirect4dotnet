using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExtDirect.Direct
{
    public class Response
    {
        public string type
        {
            get;
            set;
        }
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
        public string result
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
