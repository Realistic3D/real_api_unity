using System;
using REAL.Tools;
using System.Text;
using UnityEngine;
using REAL.Networks;
using System.Threading.Tasks;
using UnityEngine.Networking;
using RenderParams = REAL.Networks.RenderParams;

public static class ApiRequests
{
    public static async Task<AccountResponse> LoginProduct(LoginCred login)
    {
        const string domain = RealNetwork.Domain;
        var insID = login.insID;
        var appKey = login.appKey;
        var prodKey = login.prodKey;
        
        var uri = $"https://{domain}/rapi/login?insID={insID}&appKey={appKey}&prodKey={prodKey}";
        
        var www = UnityWebRequest.Get(uri);
        www.downloadHandler = new DownloadHandlerBuffer();
        
        var asyncOperation = www.SendWebRequest();
        
        while (!asyncOperation.isDone)
        {
            await Task.Yield();
        }
        
        if (www.result == UnityWebRequest.Result.Success)
        {
            var response = www.downloadHandler.text;
            
            return JsonUtility.FromJson<AccountResponse>(response);
        }
        
        Debug.LogError("Error: " + www.error);
        www.Dispose();
        return null;
    }
    
    public static async Task<ApiResponse> PostRequest(LoginCred login, RequestService ask, string jobID = null, RenderParams render = null)
    {
        var param = new Params(login, ask, jobID, render);
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

        var success = www.result == UnityWebRequest.Result.Success;
        www.Dispose();
        return success;
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
        www.Dispose();
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
        www.Dispose();
        return sprite;
    }
}
