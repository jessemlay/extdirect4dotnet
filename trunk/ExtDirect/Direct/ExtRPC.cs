using System.Web;
using System.Text;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.Web.Script.Serialization;


namespace ExtDirect.Direct
{

    public class ExtRPC
    {


        public string ExecuteRPC(HttpRequest request)
        {
            
            string requestData;
            bool flag = true;
            var rpc = new ExtAction();
            var checkFormPost = request["extAction"];
            JavaScriptSerializer jSri = new JavaScriptSerializer();

            try
            {
                if (checkFormPost.Length > 0)
                {
                    flag = false;

                }
            }
            catch
            {

            }
            if (flag)
            {
                var _tempDataList = new List<Dictionary<string, string>>();
                byte[] requestDataInByte = request.BinaryRead(request.TotalBytes);
                System.Text.ASCIIEncoding enc = new System.Text.ASCIIEncoding();

                var deserializedObject = new Request();
                requestData = enc.GetString(requestDataInByte);                
                var data = jSri.Deserialize<Request>(requestData);
                var IsNull = data.IsNullData();
                if (data.IsSpecialField())
                {
                    var upDataRequest = data.IsUpdateRequest();
                    if (upDataRequest)
                    {
                        var upData = data.data[1];
                        _tempDataList = ExtractData(data.data[1]);
                        var rp = rpc.ExecuteUpdate(data, _tempDataList);
                        return jSri.Serialize(rp);
                    }
                    else
                    {
                        if (IsNull)
                        {
                            string[] parmList = new string[0];
                            data.data = parmList;
                            requestData = jSri.Serialize(rpc.ExecuteNormalAction(data));
                            return requestData;
                        }
                        else
                        {
                            _tempDataList = ExtractData(data.data[0]);
                            return jSri.Serialize(rpc.ExecuteCRUD(data, _tempDataList));
                        }

                    }

                }
                else
                {
                    requestData = jSri.Serialize(rpc.ExecuteNormalAction(data));
                    return requestData;
                }
              

            }
            else
            {
                Response _res = rpc.ExecuteForm(request);
                return jSri.Serialize(_res);

            }            
            
        }
        internal string stringFy(string str)
        {
            return str.Replace("\"","");
        }
        internal Dictionary<string, string> ExtractSingleRecord(string str)
        {
            //  var temp = new List<Dictionary<string, string>>();
            str = str.Replace("{", "");
            str = str.Replace("}", "");
            var _d = new Dictionary<string, string>();
            string[] splitStr = str.Split(',');
            foreach (var s in splitStr)
            {
                var _tS = s.Split(':');
                _d.Add(_tS[0], _tS[1]);                
            }
            return _d;

        }
        internal List<Dictionary<string, string>> ExtractMultipleRecord(string str)
        {
            List<Dictionary<string, string>> dataL = new List<Dictionary<string, string>>();
            str = str.Replace("[", "");
            str = str.Replace("]", "");
            string oldStr = str;
            int idx;
            while (true)
            {
                idx = oldStr.IndexOf(",{");
                string tempStr = "";
                if (idx != -1)
                {
                    tempStr = oldStr.Substring(0, idx);
                    dataL.Add( ExtractSingleRecord(tempStr));
                    oldStr = oldStr.Replace(tempStr + ",", "");
                }
                else
                {
                    tempStr = oldStr;
                    if ((tempStr.Length > 1) && (tempStr.IndexOf("{") != -1) && (tempStr.IndexOf("}") != -1))
                    {
                        dataL.Add(ExtractSingleRecord(tempStr));
                        break;
                    }
                }
            }

            return dataL;
        }
        internal List<Dictionary<string, string>> ExtractData(string str)
        {
            var temp = new List<Dictionary<string, string>>();
            str = str.Replace("\"", "");
            var intt = str.IndexOf("[");
            if (intt != -1)
            {

                temp = ExtractMultipleRecord(str);
            }
            else
            {

                temp.Add(ExtractSingleRecord(str));


            }
            return temp;
        }
        //internal bool IsSpecialField(
      
    }
    public class ExtPool
    {
        internal class PoolResult
        {
            public string type
            {
                get;
                set;
            }
            public string name
            {
                get;
                set;
            }
            public string data
            {
                get;
                set;
            }

        }
        public string BindPool(string poolType, string name, string Data)
        {
            JavaScriptSerializer jSri = new JavaScriptSerializer();
            string responseData = "";
            var tempRes = new PoolResult
            {
                type = poolType,
                name = name,
                data = Data
            };
            responseData = jSri.Serialize(tempRes);
            return responseData;
        }
    }
}