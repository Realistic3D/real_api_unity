using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace REAL.Tools
{
    [Serializable]
    public class LoginCred
    {
        public UserLogin userCred;
        public ProductLogin prodCred;
    }
    [Serializable]
    public class UserLogin
    {
        public string appKey;
        public string userName;
        public string appSecret;
    }
    [Serializable]
    public class ProductLogin
    {
        public string prodKey;
        public int insID;
    }
}
