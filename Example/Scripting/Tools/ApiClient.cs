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
        public static IEnumerator PostRequest(RendererExample render, AskService ask)
        {
            var param = new Params(render.real.login, ask);
            var json = param.Dumps();

            var data = Encoding.UTF8.GetBytes(json);
            var www = UnityWebRequest.Post(RealNetwork.ApiPath, "");
            www.uploadHandler = new UploadHandlerRaw(data);
            www.downloadHandler = new DownloadHandlerBuffer();
            www.SetRequestHeader("Content-Type", "application/json");

            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.Success)
            {
                var response = www.downloadHandler.text;
                Debug.Log("Response: " + response);
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
        
        public static IEnumerator PostRequestOld(LoginCred login, AskService ask)
        {
            var param = new Params(login, ask);
            var json = param.Dumps();
            
            var data = Encoding.UTF8.GetBytes(json);
            var header = new Hashtable { { "Content-Type", "application/json" } };
            var www = new WWW (RealNetwork.ApiPath, data, header);
            yield return www;
            Debug.Log(www.text);
        }

    }

}