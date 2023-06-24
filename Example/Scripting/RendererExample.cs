using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using REAL.Items;
using REAL.Networks;
using UnityEngine;

namespace REAL.Example
{
    [RequireComponent(typeof(RealAPI))]
    public class RendererExample : MonoBehaviour
    {
        #region Inspector

        public RealAPI real;
        public Job[] jobs;

        #endregion

        #region Test Only

        public ApiResponse apiResponse;

        #endregion
        
        private void Awake()
        {
            jobs = Array.Empty<Job>();
            real = GetComponent<RealAPI>();
            if (!real) throw new Exception("API Instance is missing");
            RealSocket.RendererExample = this;
        }

        public void Start()
        {
            // RealSocket.Connect();
        }
        
        public void RenderClick()
        {
            JobTools.NewJob(this);
        }

        public void OnMessage(SocketResponse response)
        {
            var jobsData = response.data;
            var type = response.type;
            switch (type)
            {
                case "status":
                case "auth_success":
                    jobs = JobTools.UpdateJobs(jobsData);
                    break;
            }
        }

        
    }
}
