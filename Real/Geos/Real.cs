using System;
using System.Collections.Generic;
using System.Linq;
using REAL.Component;
using REAL.Tools;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityGLTF;

namespace REAL
{
    public static class Real
    {
        #region Parse Scenes

        public static byte[] RealScene(Scene scene, Camera camera = null)
        {
            try
            {
                var allObjects = scene.GetRootGameObjects().ToList();
                return RealScene(allObjects, camera, scene.name);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public static byte[] RealScene(IEnumerable<GameObject> allObjects, Camera camera = null,
            string sceneName = "REAL_SCENE")
        {
            try
            {
                var transforms = allObjects.Select(root => root.transform).ToList();
                return RealScene(transforms, camera, sceneName);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }
        public static byte[] RealScene(IEnumerable<Transform> allObjects, Camera camera = null,
            string sceneName = "REAL_SCENE")
        {
            try
            {
                var transforms = Array.ConvertAll(allObjects.Where(go =>
                    !go.GetComponent<Canvas>() &&
                    !go.GetComponent<EventSystem>()
                ).ToArray(), go => go.transform);

                var realLights = MeshTools.FindLights(transforms);
                var allTransforms = new List<Transform>(transforms);
                foreach (var light in realLights.Where(
                             light => light.realLight && !allTransforms.Contains(light.realLight.transform)))
                {
                    allTransforms.Add(light.realLight.transform);
                }
                return RealScene(allTransforms.ToArray(), sceneName, realLights, camera);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        #endregion

        #region Parse Operations

        private static byte[] RealScene(Transform[] allTransforms, string sceneName, List<RealLight> realLights,
            Camera camera = null)
        {
            var realName = "";
            if (camera)
            {
                realName = camera.name;
                camera.name = "REAL_EYE";
            }
            
            var exporter = new GLTFSceneExporter(allTransforms);
            var stream = exporter.GetGlb(sceneName);
            ResetScene(camera, realName, realLights);
            return stream.ToArray();
        }
        private static void ResetScene(Camera camera, string realName, List<RealLight> realLights)
        {
            if (camera) camera.name = realName;
            foreach (var light in realLights) light.Reset();
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

        #endregion
    }
}