using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;
using REAL.Tools;
using UnityEngine;

namespace REAL.Networks
{
    public enum AskService {NewJob, Submit, Status, Result}
    [Serializable]
    public class Params
    {
        public string ask;
        public ProdCred prodCred;
        public ServiceParams service;
        public RenderParams renderParams;

        public Params(LoginCred login, AskService askService, string jobID = null)
        {
            ask = Ask(askService);
            prodCred = new ProdCred(login);
            if(askService == AskService.NewJob) renderParams = new RenderParams();
            else service = new ServiceParams(jobID);
        }

        public string Dumps()
        {
            var json = JsonConvert.SerializeObject(this);
            json = json.Replace('"', '\"');
            return json;
        }

        public static string Ask(AskService askService)
        {
            return askService switch
            {
                AskService.NewJob => "new_job",
                AskService.Submit => "submit",
                AskService.Status => "status",
                AskService.Result => "status",
                _ => throw new Exception("Need service")
            };
        }
    }
    [Serializable]
    public class ProdCred
    {
        public int insID;
        public string appKey;
        public string prodKey;

        public ProdCred(LoginCred login)
        {
            insID = login.prodCred.insID;
            appKey = login.userCred.appKey;
            prodKey = login.prodCred.prodKey;
        }
        public string Dumps()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    [Serializable]
    public class ServiceParams
    {
        public string jobID;

        public ServiceParams(string jobID)
        {
            this.jobID = jobID;
        }
        
        public string Dumps()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
    [Serializable]
    public class RenderParams
    {
        public string expFrom = "u3d";
        
        public string Dumps()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}

