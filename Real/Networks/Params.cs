using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using REAL.Tools;
using UnityEngine;

namespace REAL.Networks
{
    public enum RequestService {New, Render, Result}
    [Serializable]
    public class Params
    {
        public Cred cred;
        public string type;
        public string jobID;
        public RenderParams render;

        public Params(LoginCred login, RequestService requestService, string jobID = null, RenderParams render = null)
        {
            this.jobID = jobID;
            cred = new Cred(login);
            type = RequestType(requestService);

            if (requestService == RequestService.New && render == null) render = new RenderParams();
            this.render = render;
        }

        public string Dumps()
        {
            var settings = new JsonSerializerSettings
            {
                StringEscapeHandling = StringEscapeHandling.EscapeHtml,
                NullValueHandling = NullValueHandling.Ignore // Exclude null properties
            };
            var json = JsonConvert.SerializeObject(this, settings);
            return json;
        }

        public static string RequestType(RequestService askService)
        {
            return askService switch
            {
                RequestService.New => "new",
                RequestService.Render => "render",
                RequestService.Result => "result",
                _ => throw new Exception("Invalid service asked!")
            };
        }
    }
    [Serializable]
    public class Cred
    {
        public int insID;
        public string appKey;
        public string prodKey;

        public Cred(LoginCred login)
        {
            insID = login.insID;
            appKey = login.appKey;
            prodKey = login.prodKey;
        }
        public string Dumps()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    [Serializable]
    public class RenderParams
    {
        public int samples = 20;
        public int quality = 512;
        public bool bake = false;
        public string output = "PNG";
        public string expFrom = "u3d";
        
        public string Dumps()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

