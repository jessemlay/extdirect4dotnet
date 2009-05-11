using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Reflection;

namespace ExtDirect.Direct
{
    public class ExtAction : IAction
    {
        public Response ExecuteForm(HttpRequest httpRequest)
        {

            int i = 0;
            var responseForm = new Request
            {
                action = httpRequest["extAction"],
                method = httpRequest["extMethod"],
                tid = Convert.ToInt32(httpRequest["extTID"])
            };

            var action = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(responseForm.action);
            Type thisType = action.GetType();
            MethodInfo theMethod = thisType.GetMethod(responseForm.method);
            ParameterInfo[] parmInfo = theMethod.GetParameters();
            object[] parmList;
            parmList = new object[parmInfo.Length];

            foreach (var parm in parmInfo)
            {

                parmList[i] = httpRequest[parm.Name];
                i++;
            }
            string requestData = (string)theMethod.Invoke(action, parmList);

            var _res = new Response
            {
                type = "rpc",
                method = responseForm.method,
                tid = responseForm.tid,
                result = requestData,
                action = responseForm.action

            };
            return _res;
        }
        public Response ExecuteLoad(Request request)
        {
            
            var action = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(request.action);
            Type thisType = action.GetType();
            MethodInfo theMethod = thisType.GetMethod(request.method);
            ParameterInfo[] parmInfo = theMethod.GetParameters();
            string[] parmList;
            parmList = new string[0];
            request.data = parmList;
            var _res = new Response
            {
                type = "rpc",
                method = request.method,
                tid = request.tid,
                result = (string)theMethod.Invoke(action, request.data),
                action = request.action

            };           
            return _res;
        }
        public Response ExecuteCreate(Request request)
        {
            Response _res = new Response();
            return _res;
        }
        public Response ExecuteSave(Request request)
        {
            Response _res = new Response();
            return _res;
        }
        public Response ExecuteUpdate(Request request, List<Dictionary<string, string>> dataList)
        {
            
            object[] funcParList;
            var action = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(request.action);

            Type thisType = action.GetType();
            MethodInfo theMethod = thisType.GetMethod(request.method);
            ParameterInfo[] parmInfo = theMethod.GetParameters();
            funcParList = new object[parmInfo.Length];
            string res = "";
            foreach (var dat in dataList)
            {
                int i = 0;
                foreach (var parm in parmInfo)
                {

                    try
                    {
                        funcParList[i] = dat[parm.Name];
                    }
                    catch
                    {
                        funcParList[i] = "";
                    }
                    i++;
                }
               res+= (string)theMethod.Invoke(action, funcParList);
               res += ",";
            }
            if (dataList.Count > 0)
            {
                res = res.Remove(res.Length - 1, 1);
                if (dataList.Count > 1)
                {
                    res = "[" + res + "]";
                }
            }
            else
            {
                res = "{}";
            }
            
            var _res = new Response
            {
                type = "rpc",
                method = request.method,
                tid = request.tid,
                result = res,
                action = request.action

            };    
            return _res;
        }
        public Response ExecuteDelete(Request request)
        {
            Response _res = new Response();
            return _res;
        }
        public Response ExecuteNormalAction(Request request)
        {

            var obj = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(request.action);
            Type thisType = obj.GetType();
            MethodInfo theMethod = thisType.GetMethod(request.method);
            string requestData = (string)theMethod.Invoke(obj, request.data);
            var _res = new Response
            {
                type = "rpc",
                method = request.method,
                tid = request.tid,
                result = requestData,
                action = request.action

            };
            return _res;
        }
        public Response ExecuteCRUD(Request request, List<Dictionary<string,string>> recordList)
        {
            object[] funcParList;
            string json = "";
            var action = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(request.action);
            Type thisType = action.GetType();
            MethodInfo theMethod = thisType.GetMethod(request.method);
            ParameterInfo[] parmInfo = theMethod.GetParameters();
            
            foreach (var _t in recordList)
            {
                funcParList = new object[parmInfo.Length];
                int i = 0;
                foreach (var parm in parmInfo)
                {

                    funcParList[i] = _t[parm.Name];
                    i++;
                }
                json += (string)theMethod.Invoke(action, funcParList) + ",";

            }
            if (recordList.Count > 0)
            {
                json = json.Remove(json.Length - 1, 1);
            }
            if (recordList.Count > 1)
            {
                json = "[" + json + "]";

            }
            var _res = new Response
            {
                type = "rpc",
                method = request.method,
                tid = request.tid,
                result = json,
                action = request.action

            };
            return _res;
            
        }
        //public Response ExecuteCRUD(Request request, List<string> recordList)
        //{
            
        //    string json = "";

        //    List<Dictionary<string, string>> dataList = new List<Dictionary<string, string>>();
        //    var action = System.Reflection.Assembly.GetExecutingAssembly().CreateInstance(request.action);
        //    Type thisType = action.GetType();
        //    MethodInfo theMethod = thisType.GetMethod(request.method);
        //    ParameterInfo[] parmInfo = theMethod.GetParameters();
            

        //    foreach (var _tt in recordList)
        //    {
        //        string _tempReq = _tt;
        //        object[] funcParList;
        //        string[] _tempList = _tempReq.getRequestField();
        //        string _dataField = _tempList[2];
        //        dataList = _dataField.getDataList();
        //        Dictionary<string, string> _rpcDataList = _dataField.getData();
        //        funcParList = new object[parmInfo.Length];
        //        int i = 0;
        //        foreach (var parm in parmInfo)
        //        {

        //            funcParList[i] = _rpcDataList[parm.Name];
        //            i++;
        //        }
                
        //        json += (string)theMethod.Invoke(action, funcParList) +",";

        //    }
        //    json = json.Remove(json.Length - 1, 1);
        //    if (recordList.Count > 1)
        //    {
        //        json = "[" + json + "]";
                
        //    }
        //    var _res = new Response
        //    {
        //        type = "rpc",
        //        method = request.method,
        //        tid = request.tid,
        //        result = json,
        //        action = request.action

        //    };           
        //    return _res;
        //}
        

    }
}
