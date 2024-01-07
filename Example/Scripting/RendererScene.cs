using REAL;
using REAL.Networks;
using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(RealAPI))]
public class RendererScene : MonoBehaviour
{
    public RealAPI real;

    private void Awake()
    {
        real = GetComponent<RealAPI>();
    }

    public async void RenderScene()
    {
        var login = real.login;
        
        // Step 1: Get Scene
        
        var camera = Camera.main;
        var scene = SceneManager.GetActiveScene();
        var realScene = Real.RealScene(scene, camera);
        
        // Step 2: Apply new job

        var apiResponse = await ApiRequests.PostRequest(login, RequestService.New);
        var resData = apiResponse.data; 
        var uri = resData.url;
        
        // Step 3: Upload scene

        bool uploaded = await ApiRequests.PutRequest(uri, realScene);
        
        // Step 4: Submit job

        await ApiRequests.PostRequest(login, RequestService.Render, resData.jobID);
    }
}
