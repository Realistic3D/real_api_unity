using System;
using REAL.Items;
using REAL.Networks;
using UnityEngine;

namespace REAL.Example
{
    [RequireComponent(typeof(RealAPI))]
    public class RendererExample : MonoBehaviour
    {
        [HideInInspector]
        public RealAPI real;

        public UICanvas canvas;
        
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
        private async void OnOnline()
        {
            canvas.infoPanel.SetStatus("Connected!");
            canvas.loginPanel.SetStatus("Online");
            canvas.ShowRenderUI(true);
            var userInfo = await ApiRequests.GetAccount(Commons.Renderer.real.login);
            canvas.loginPanel.SetAccountInfo(userInfo);
        }
        public void OnOffline()
        {
            canvas.loginPanel.SetStatus("Offline");
            canvas.ShowRenderUI(false);
        }

        private void OnApplicationQuit()
        {
            RealSocket.Abort();
        }
    }
}
