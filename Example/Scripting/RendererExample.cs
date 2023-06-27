using System;
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
            Commons.Renderer = this;
        }
        public void OnMessage(SocketResponse response)
        {
            var type = response.type;
            var jobsData = response.data;
            switch (type)
            {
                case "status":
                    canvas.jobPanel.AddJobs(jobsData);
                    break;
                case "auth_success":
                    OnOnline();
                    canvas.jobPanel.AddJobs(jobsData);
                    break;
            }
        }

        private void OnOnline()
        {
            canvas.infoPanel.SetStatus("Connected!");
            canvas.loginPanel.SetStatus("Online");
            canvas.ShowRenderUI(true);
        }
        public void OnOffline()
        {
            canvas.infoPanel.SetStatus("Logged out!");
            canvas.loginPanel.SetStatus("Offline");
            canvas.ShowRenderUI(false);
        }
    }
}
