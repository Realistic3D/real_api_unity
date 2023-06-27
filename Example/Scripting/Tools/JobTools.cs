using System;
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
        public static async void NewJob()
        {

            #region Step 1: Get Scene

            var camera = Camera.main;
            var scene = SceneManager.GetActiveScene();
            var login = Commons.Renderer.real.login;
            
            var realScene = Real.RealScene(scene, camera);
            
            #endregion
            
            #region Optional checks

            if (realScene == null)
            {
                Commons.Renderer.canvas.loginPanel.SetStatus("Failed to create scene!");
                return;
            }
            
            if (realScene.Length == 0)
            {
                Commons.Renderer.canvas.loginPanel.SetStatus("Empty scene!");
                return;
            }

            #endregion
            
            #region Step 2: Apply new job
            
            var apiResponse = await ApiRequests.PostRequest(login, AskService.NewJob);
            var resData = apiResponse.data; 

            var uri = resData.url;
            
            #endregion
            
            #region Optional checks
            
            if (uri == null || !uri.StartsWith("http"))
            {
                Commons.Renderer.canvas.loginPanel.SetStatus("Failed to apply for new job!");
                return;
            }
            
            #endregion
            
            #region Step 3: Upload scene

            var uploaded = await ApiRequests.PutRequest(uri, realScene);
            
            #endregion
            
            #region Optional checks
            
            if (!uploaded)
            {
                Commons.Renderer.canvas.loginPanel.SetStatus("Failed to upload job!");
                return;
            }
            Commons.Renderer.canvas.loginPanel.SetStatus("Submitting job!");
            
            #endregion
            
            #region Step 4: Submit job

            Commons.Renderer.apiResponse = await ApiRequests.PostRequest(login, AskService.Submit, resData.jobID);
            // Debug.LogError("SIZE = " + Real.SceneSize(realScene));
            #endregion
        }

        public static async void DownloadResult(Job job, Action<float> progressCallback)
        {
            var login = Commons.Renderer.real.login;
            var apiResponse = await ApiRequests.PostRequest(login, AskService.Result, job.jobID);
            var data = apiResponse.data;
            
            if (data == null || !data.url.StartsWith("http"))
            {
                Commons.Renderer.canvas.loginPanel.SetStatus("Failed to apply for result!");
                return;
            }

            var url = data.url;
            var sprite = await ApiRequests.DownloadImageProgress(url, progressCallback);
            Commons.Renderer.canvas.jobPanel.jobPreview.sprite = sprite;
            Commons.Renderer.canvas.uiPanel.ActivateView(true);
        }
        
    }
}