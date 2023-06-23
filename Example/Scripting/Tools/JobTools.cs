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
        public static void NewJob(RendererExample render, ApiResponse response)
        {
            var camera = Camera.main;
            var scene = SceneManager.GetActiveScene();
            var realScene = Real.RealScene(scene, camera);
            ApiClient.PutRequest(realScene);
            Debug.LogError("SIZE = " + Real.SceneSize(realScene));
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