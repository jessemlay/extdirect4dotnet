using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;

namespace ExtDirect.Direct
{
    internal static class Util
    {
        internal static bool IsSpecialField(this Request req)
        {
          
            try
            {
                string data = req.data[0];
                if (data.Contains("{") || data.Contains("["))
                {
                    return true;
                }
                else
                {
                    data = req.data[1];
                    if (data.Contains("{") || data.Contains("["))
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
                
            }
            catch
            {
                return false;
            }
        }
        internal static bool hasData(this Request req)
        {
            try
            {
                var len = req.data.Length;
                if(len>0)
                {
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch
            {
                return false;
            }
           
        }
        internal static bool IsNullData(this Request req)
        {
            try
            {
                var data = req.data[0];
                if (data.Length > 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
                

            }
            catch
            {
                return true;
            }

        }
        internal static bool IsUpdateRequest(this Request req)
        {
            try
            {
                var len = req.data.Length;
                if (len == 2)
                {
                    return true;
                }
                else
                {
                    return false;

                }

            }
            catch
            {
                return false;
            }
        }
            
    }
}
