using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using REAL.Component;
using Unity.VisualScripting;

namespace REAL.Tools
{
    public static class MeshTools
    {
        public static List<RealLight> FindLights(IEnumerable<Transform> transforms)
        {
            var lights = new List<RealLight>();
            FindAreaLight(transforms, lights);
            var realLights = ParseLights(lights);

            var counter = 0;
            foreach (var light in realLights.Where(light => light.realLight))
            {
                var realLight = light.realLight;
                realLight.name = $"{light.name}_{counter}";
                foreach (Transform child in realLight.transform) child.name += $"|id_{counter}";
                counter++;
            }
            
            return realLights;
        }

        private static void FindAreaLight(IEnumerable<Transform> transforms, ICollection<RealLight> lights)
        {
            foreach (var tr in transforms)
            {
                var light = tr.GetComponent<RealLight>();
                if(light && light.type == LightType.Area && light.isActiveAndEnabled) lights.Add(light);
                var children = tr.Cast<Transform>().ToList();
                FindAreaLight(children, lights);
            }
        }
        
        private static List<RealLight> ParseLights(IReadOnlyCollection<RealLight> lights)
        {
            var realLights = new List<RealLight>();
            if(lights == null || lights.Count == 0) return realLights;
            foreach (var light in lights)
            {
                light.ParseLight();
                if(light.realLight) realLights.Add(light);
            }
            return realLights;
        }
    }
}
