using System;
using REAL.Items;

namespace REAL.Networks
{
    [Serializable]

    public class SocketResponse
    {
        public string msg;
        public string type;
        public Job[] data;
    }

    [Serializable]
    public class ApiResponse
    {
        public string msg;
        public ApiResponseData data;
    }

    [Serializable]
    public class ApiResponseData
    {
        public string url;
        public string jobID;
        public string status;
    }
}
