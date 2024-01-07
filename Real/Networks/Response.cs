using System;
using Newtonsoft.Json;
using REAL.Items;

namespace REAL.Networks
{
    [Serializable]

    public class SocketResponse
    {
        public string msg;
        public string type;
        public Job[] data;
        
        public string Dumps()
        {
            return JsonConvert.SerializeObject(this);
        }
    }

    [Serializable]
    public class ApiResponse
    {
        public string msg;
        public ApiResponseData data;
        
        public string Dumps()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    
    [Serializable]
    public class ApiResponseData
    {
        public string url;
        public string jobID;
        public string status;
        public bool finished;
        public string expFrom;
        
        public string Dumps()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    
    [Serializable]
    public class AccountResponse
    {
        public string msg;
        public AccResponseData data;
    }
    
    [Serializable]
    public class AccResponseData
    {
        public int insID;
        public float balance;
        public string userName;
        public string prodName;
        public string currency;
    }
}
