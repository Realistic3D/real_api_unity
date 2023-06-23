using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityGLTF;

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
                var gameObjects = scene.GetRootGameObjects();
                var transforms = Array.ConvertAll(gameObjects, go => go.transform);
                var options = new ExportOptions();
                var exporter = new GLTFSceneExporter(transforms, options);
                var sceneName = scene.name;
                var stream = exporter.GetGLB(sceneName);
                if (camera)
                {
                    camera.name = realName;
                }
                return stream.ToArray();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
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