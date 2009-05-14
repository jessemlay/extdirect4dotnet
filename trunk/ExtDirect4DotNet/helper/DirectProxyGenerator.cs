using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;
using System.Web;

namespace ExtDirect.Direct
{
    public static class DirectProxyGenerator
    {
        /**
        public static string generateDirectApi()
        {
            StringBuilder resStr = new StringBuilder();
            AppDomain MyDomain = AppDomain.CurrentDomain;
            Assembly[] AssembliesLoaded = MyDomain.GetAssemblies();
            resStr.Append("\"actions\":");
         
         
         
            bool flag = false;            
            foreach (var allAssembly in AssembliesLoaded)           
            {
                foreach (var theType in allAssembly.GetTypes())
                {
                    object[] allCustomAttribute = theType.GetCustomAttributes(false);
                    foreach (var _theType in allCustomAttribute)
                    {
                        Type directType = _theType.GetType();
                        if (typeof(DirectServiceAttribute) == directType)
                        {
                            string className = theType.Name;
                            resStr.Append("{\"" + className + "\":[");                            
                            MethodInfo[] methodInfo = theType.GetMethods();

                            List<string> methodList = new List<string>();

                            foreach (var _m in methodInfo)
                            {
                                object[] _mAtt = _m.GetCustomAttributes(false);
                                foreach (var _xyz in _mAtt)
                                {
                                    Type methodType = _xyz.GetType();

                                    if (typeof(DirectMethodAttribute) == methodType)
                                    {
                                        string tempMey = "";
                                        DirectMethodAttribute methodDesc = (DirectMethodAttribute)_xyz;
                                        string methodName = _m.Name;
                                        MethodVisibility vis = methodDesc.Visibility;
                                        DirectAction gMethod = methodDesc.Action;
                                        tempMey = "{\"name\":";
                                        tempMey += "\"" + methodName + "\",";
                                        if (DirectAction.No == gMethod)
                                        {
                                            ParameterInfo[] p = _m.GetParameters();
                                            int count = p.Length;
                                            tempMey += "\"len\":" + count + "}";

                                        }
                                        else if (DirectAction.Save == gMethod)
                                        {
                                            tempMey += "\"len\": 2}";
                                        }
                                        else if (DirectAction.FormSubmission == gMethod)
                                        {
                                            tempMey += "\"formHandler\": true,\"len\": 1}";
                                        }
                                        else if (DirectAction.Load == gMethod)
                                        {
                                            tempMey += "\"len\": 0}";
                                        }
                                        else
                                        {
                                            tempMey += "\"len\": 1}";
                                        }
                                        resStr.Append(tempMey + ",");

                                        methodList.Add(tempMey);
                                    }

                                }
                            }
                            resStr.Remove(resStr.Length - 1, 1);
                            resStr.Append("]},");
                            flag = true;
                        }
                    }

                }
                if (flag)
                {
                    resStr.Remove(resStr.Length - 1, 1);
                    flag = false;
                }
            }
            
            return resStr.ToString();
            
            
        }
        **/
    }
}
