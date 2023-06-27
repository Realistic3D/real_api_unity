using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using REAL.Example;
using REAL.Networks;
using REAL.Tools;
using UnityEngine;
using UnityEngine.Networking;

public static class ApiRequests
{
    public static async Task<ApiResponse> PostRequest(LoginCred login, AskService ask, string jobID = null)
    {
        var param = new Params(login, ask, jobID);
        var json = param.Dumps();

        var data = Encoding.UTF8.GetBytes(json);
        var www = UnityWebRequest.Post(RealNetwork.ApiPath, "");
        www.uploadHandler = new UploadHandlerRaw(data);
        www.downloadHandler = new DownloadHandlerBuffer();
        www.SetRequestHeader("Content-Type", "application/json");

        var asyncOperation = www.SendWebRequest();
        
        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }
        
        // if(ask == AskService.Result)
        // {
        //     Debug.LogError(json);
        //     // Debug.Log(response);
        // }
        if (www.result == UnityWebRequest.Result.Success)
        {
            var response = www.downloadHandler.text;
            
            return JsonUtility.FromJson<ApiResponse>(response);
        }
        Debug.LogError("Error: " + www.error);
        www.Dispose();
        return null;
    }
    
    public static async Task<bool> PutRequest(string url, byte[] scene)
    {
        var www = UnityWebRequest.Put(url, scene);
        www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");

        var asyncOperation = www.SendWebRequest();
        
        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }
        return www.result == UnityWebRequest.Result.Success;
    }
    
    public static async Task<Sprite> DownloadImage(string url)
    {
        using var www = UnityWebRequestTexture.GetTexture(url);
        var asyncOperation = www.SendWebRequest();
        
        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }

        if (www.result != UnityWebRequest.Result.Success) return null;
        var texture = DownloadHandlerTexture.GetContent(www);
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return sprite;
    }
    
    public static async Task<Sprite> DownloadImageProgress(string url, Action<float> progressCallback)
    {
        using var www = UnityWebRequestTexture.GetTexture(url);
        var asyncOperation = www.SendWebRequest();
        
        while (!asyncOperation.isDone)
        {
            var progress = Mathf.Clamp01(www.downloadProgress);
            var round = (float) Math.Round(progress * 100f, 3);
            progressCallback?.Invoke(round);
            await Task.Yield();
        }

        if (www.result != UnityWebRequest.Result.Success) return null;
        var texture = DownloadHandlerTexture.GetContent(www);
        var sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
        return sprite;
    }
}
