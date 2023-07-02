using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using REAL.Geometry;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityGLTF;
using Object = UnityEngine.Object;

namespace REAL
{
    public static class Real
    {
        public static byte[] RealScene(Scene scene, Camera camera = null)
        {
            try
            {
                var realName = "";
                if (camera)
                {
                    realName = camera.name;
                    camera.name = "REAL_EYE";
                }
                var allObjects = scene.GetRootGameObjects().ToList();


                var transforms = Array.ConvertAll(allObjects.Where(go =>
                    !go.GetComponent<Canvas>() &&
                    !go.GetComponent<EventSystem>()
                ).ToArray(), go => go.transform);

                var allLights = RealGeometry.FindLights(transforms);
                var areaLights = allLights[0];
                var realLights = allLights[1];

                var allTransforms = new List<Transform>(transforms);
                allTransforms.AddRange(realLights.Select(light => light.transform));

                var exporter = new GLTFSceneExporter(allTransforms.ToArray());
                
                var sceneName = scene.name;
                var stream = exporter.GetGlb(sceneName);
                
                ResetScene(camera, realName, areaLights, realLights);
                return stream.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        private static void ResetScene(Camera camera, string realName, List<GameObject> areaLights, List<GameObject> realLights)
        {
            if (camera) camera.name = realName;
            foreach (var light in areaLights)
            {
                light.SetActive(true);
            }
            foreach (var light in realLights)
            {
                Object.Destroy(light);
            }
        }
        
        public static float SceneSize(byte[] data)
        {
            var length = data.Length;
            var size = CalSize(length);
            return size;
        }
        public static float SceneSize(string data)
        {
            var length = data.Length;
            var size = CalSize(length);
            return size;
        }

        private static float CalSize(int size)
        {
            const float mbs = 1000f * 1000f;
            return size / mbs;
        }
    }
}