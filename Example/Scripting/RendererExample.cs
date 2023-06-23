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
        public RealAPI real;
        public Job[] jobs;
        public ApiResponse apiResponse;
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
        
        public void Render()
        {
            StartCoroutine(ApiClient.PostRequest(this, AskService.NewJob));
        }

        public void OnMessage(SocketResponse response)
        {
            var jobsData = response.data;
            var type = response.type;
            switch (type)
            {
                case "status":
                case "auth_success":
                    jobs = UpdateJobs(jobsData);
                    break;
            }
        }

        private static Job[] UpdateJobs(Job[] jobList)
        {
            var newList = new List<Job>();
            if(jobList == null) return newList.ToArray();
            newList.AddRange(jobList.Where(jobItem => jobItem.expFrom == "u3d"));
            return newList.ToArray();
        }
    }
}
