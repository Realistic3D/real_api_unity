using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using REAL.Networks;
using UnityEngine;

namespace REAL.Example
{
    [RequireComponent(typeof(RealAPI))]
    public class RendererExample : MonoBehaviour
    {
        #region Inspector

        public RealAPI real;
        public UITools canvas;

        #endregion

        #region Test Only

        public ApiResponse apiResponse;

        #endregion
        
        private void Awake()
        {
            real = GetComponent<RealAPI>();
            if (!real) throw new Exception("API Instance is missing");
            RealSocket.RendererExample = this;
        }
        
        public void RenderClick()
        {
            print("Exporting....");
            JobTools.NewJob(this);
        }
        public void OnMessage(SocketResponse response)
        {
            var type = response.type;
            var jobsData = response.data;
            switch (type)
            {
                case "status":
                case "auth_success":
                    canvas.loginPanel.SetStatus("Online");
                    canvas.jobPanel.AddJobs(jobsData);
                    break;
            }
        }
    }
}
