using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using REAL.Networks;
using REAL.Tools;
using UnityEngine;
using UnityEngine.Networking;

namespace REAL.Example
{
    public static class ApiClient
    {
        private static readonly string URL = $"https://{RealNetwork.Domain}/rapi/ask_service";
        public static IEnumerator PostRequest(RendererExample render, AskService ask)
        {
            var param = new Params(render.real.login, ask);
            var json = param.Dumps();

            var data = Encoding.UTF8.GetBytes(json);
            var www = UnityWebRequest.Post(URL, "");
            www.uploadHandler = new UploadHandlerRaw(data);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var response = www.downloadHandler.text;
                var apiRes = JsonUtility.FromJson<ApiResponse>(response);
                SendResponse(render, ask, apiRes);
                
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
            
            www.Dispose();
        }

        public static IEnumerator PutRequest(LoginCred login, string url, byte[] scene)
        {
            var www = UnityWebRequest.Put(url, scene);
            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                Debug.Log("Response: " + www.downloadHandler.text);
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
        }
        private static void SendResponse(RendererExample render, AskService ask, ApiResponse response)
        {
            render.apiResponse = response;
            switch (ask)
            {
                case AskService.NewJob:
                    JobTools.NewJob(render, response);
                    break;
                case AskService.Submit:
                    // JobTools.Submit(render, response);
                    break;
                case AskService.Status:
                    // JobTools.Status(render, response);
                    break;
                case AskService.Result:
                    // JobTools.Result(render, response);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(ask), ask, null);
            }
      
        }
        
        public static IEnumerator PostRequestOld(LoginCred login, AskService ask)
        {
            var param = new Params(login, ask);
            var json = param.Dumps();
            
            var data = Encoding.UTF8.GetBytes(json);
            var header = new Hashtable { { "Content-Type", "application/json" } };
            var www = new WWW (URL, data, header);
            yield return www;
            Debug.Log(www.text);
        }

    }

}