using System;
using REAL.Items;
using REAL.Networks;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace REAL.Example
{
    public static class JobTools
    {
        public static async void NewJob()
        {

            #region Step 1: Get Scene

            var camera = Camera.main;
            var scene = SceneManager.GetActiveScene();
            var login = Commons.Renderer.real.login;
            var realScene = Real.RealScene(scene, camera);
            // return;
            
            #endregion
            
            #region Optional checks

            if (realScene == null)
            {
                Commons.Renderer.canvas.infoPanel.SetStatus("Failed to create scene!");
                return;
            }
            
            if (realScene.Length == 0)
            {
                Commons.Renderer.canvas.infoPanel.SetStatus("Empty scene!");
                return;
            }

            #endregion
            
            #region Step 2: Apply new job
            
            var apiResponse = await ApiRequests.PostRequest(login, RequestService.New);
            var resData = apiResponse.data; 
            var uri = resData.url;
            
            #endregion
            
            #region Optional checks
            
            if (uri == null || !uri.StartsWith("http"))
            {
                Commons.Renderer.canvas.infoPanel.SetStatus("Failed to apply for new job!");
                return;
            }
            
            #endregion
            
            #region Step 3: Upload scene

            var uploaded = await ApiRequests.PutRequest(uri, realScene);
            
            #endregion
            
            #region Optional checks
            
            if (!uploaded)
            {
                Commons.Renderer.canvas.infoPanel.SetStatus("Failed to upload job!");
                return;
            }
            Commons.Renderer.canvas.infoPanel.SetStatus("Submitting job!");
            
            #endregion
            
            #region Step 4: Submit job

            await ApiRequests.PostRequest(login, RequestService.Render, resData.jobID);
            // Debug.LogError("SIZE = " + Real.SceneSize(realScene));
            #endregion
        }

        public static async void DownloadResult(Job job, Action<float> progressCallback)
        {
            // var login = Commons.Renderer.real.login;
            // var apiResponse = await ApiRequests.PostRequest(login, RequestService.Result, job.jobID);
            // var data = apiResponse.data;
            var url = job.result;
            if (!url.StartsWith("http"))
            {
                Commons.Renderer.canvas.loginPanel.SetStatus("Failed to apply for result!");
                return;
            }

            var sprite = await ApiRequests.DownloadImageProgress(url, progressCallback);
            Commons.Renderer.canvas.jobPanel.DisplayResult(sprite);
        }
        
    }
}