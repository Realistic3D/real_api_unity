using System.Collections;
using System.Collections.Generic;
using System.Linq;
using REAL.Items;
using REAL.Networks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace REAL.Example
{
    public static class JobTools
    {
        public static async void NewJob(RendererExample render)
        {
            var camera = Camera.main;
            var scene = SceneManager.GetActiveScene();
            var login = render.real.login;
            
            var realScene = Real.RealScene(scene, camera);
            if (realScene == null)
            {
                Debug.LogError("Failed to create scene");
                return;
            }
            
            if (realScene.Length == 0)
            {
                Debug.LogError("Empty scene");
                return;
            }
            
            var apiResponse = await ApiRequests.PostRequest(login, AskService.NewJob);
            render.apiResponse = apiResponse;

            var uri = apiResponse.data.url;
            if (uri == null || !uri.StartsWith("http"))
            {
                Debug.LogError("Failed to apply for new job");
                return;
            }
            var uploaded = await ApiRequests.PutRequest(uri, realScene);
            
            if (!uploaded)
            {
                Debug.LogError("Failed to upload job");
                return;
            }
            
            Debug.Log("SUBMITTING");
            
            render.apiResponse = await ApiRequests.PostRequest(login, AskService.Submit);
            
            // Debug.LogError("SIZE = " + Real.SceneSize(realScene));
        }
        public static Job[] UpdateJobs(Job[] jobList)
        {
            var newList = new List<Job>();
            if(jobList == null) return newList.ToArray();
            newList.AddRange(jobList.Where(jobItem => jobItem.expFrom == "u3d"));
            return newList.ToArray();
        }
    }
}