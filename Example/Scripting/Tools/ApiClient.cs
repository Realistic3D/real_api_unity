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
        private static void AddJsonToForm(Dictionary<string, object> jsonData, string parentKey, WWWForm form)
        {
            foreach (var kvp in jsonData)
            {
                var key = (string.IsNullOrEmpty(parentKey)) ? kvp.Key : parentKey + "." + kvp.Key;

                if (kvp.Value is Dictionary<string, object> objects)
                {
                    AddJsonToForm(objects, key, form);
                }
                else
                {
                    var value = kvp.Value.ToString();
                    form.AddField(key, value);
                }
            }
        }
        
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
                render.apiResponse = JsonUtility.FromJson<ApiResponse>(response);
            }
            else
            {
                Debug.LogError("Error: " + www.error);
            }
            
            www.Dispose();
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