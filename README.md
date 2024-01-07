# real_api_unity

## Introduction
- This is a RealAPI plugin for unity.
- You can render or bake unity scene online by using this plugin.
- No extra hardware or GPU is required now.

## Docs
* For detailed documents please visit: https://docs.real-api.com/
* For Unity docs please visit: https://docs.real-api.com/en/unity-3d

## Screenshots:

![1](https://github.com/Realistic3D/real_api_unity/assets/119076217/7d5ce8f4-3ab9-40b4-9570-b790ecd1cc23)

![2](https://github.com/Realistic3D/real_api_unity/assets/119076217/32b16425-9267-4094-b83a-2ebfd7bca7f8)

### Installation
* You can find `.unitypackage` file at
```CURL
https://github.com/Realistic3D/real_api_unity/releases
```

### Basic usage details:

* Signup at `https://realistic3.com/` 
* create `New Product`
* You just need your `appKey` and `prodKey`

### Scripting

* Add login class
    - Add class `RealAPI` to any active `gameObject` and add your login information

```c#
using REAL;
using REAL.Networks;

[RequireComponent(typeof(RealAPI))]
public class RendererScene : MonoBehaviour
{
    public RealAPI real;

    private void Awake()
    {
        real = GetComponent<RealAPI>();
    }
}
```

## Step 1:

* Get Real API scene (binary scene)
  - You can render **whole scene** or even **some gameObjects** of your scene it depends upon your requirement

```c#
// Step 1: Get scene from whole Scene

Camera camera = myCam; // Optional
Scene scene = myScene; // Required
var realScene = Real.RealScene(scene, camera);
```

OR

```c#
// Step 1: Get scene from some gameobjects in Scene

Camera camera = myCam; // Optional
List<GameObject> allObjects = requiredObject; // Required
var realScene = Real.RealScene(allObjects, camera);
```

## Step 2:

* Create new job
```c#
// Step 2: Apply new job

var apiResponse = await ApiRequests.PostRequest(login, RequestService.New);
var resData = apiResponse.data; 
var uri = resData.url;
```

## Step 3:

* Upload scene
```c#
 // Step 3: Upload scene

bool uploaded = await ApiRequests.PutRequest(uri, realScene);
```

## Step 4:

* Submit job
```c#
// Step 4: Render job

await ApiRequests.PostRequest(login, RequestService.Render, resData.jobID);
```

### Final script

```c#
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
        
        // Step 4: Render job

        await ApiRequests.PostRequest(login, RequestService.Render, resData.jobID);
    }
}
```
