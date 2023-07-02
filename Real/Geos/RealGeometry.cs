using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Unity.VisualScripting;

namespace REAL.Geometry
{
    public static class RealGeometry
    {
        public static List<List<GameObject>> FindLights(IEnumerable<Transform> transforms)
        {
            var lights = new List<Light>();
            var areaLights = new List<GameObject>();
            FindAreaLight(transforms, lights, areaLights);
            var realLights = ParseLights(lights);

            var counter = 0;
            foreach (var light in realLights)
            {
                light.name = $"{light.name}_{counter}";
                foreach (Transform child in light.transform)
                {
                    child.name += $"|id_{counter}";
                }
                counter++;
            }
            
            return new List<List<GameObject>>
            {
                areaLights,
                realLights
            };
        }

        private static void FindAreaLight(IEnumerable<Transform> transforms, ICollection<Light> lights, ICollection<GameObject> areaLights)
        {
            foreach (var tr in transforms)
            {
                var light = tr.GetComponent<Light>();
                if(!light || !tr.gameObject.activeInHierarchy || light.type != LightType.Area)
                {
                    var trChildren = tr.Cast<Transform>().ToList();
                    FindAreaLight(trChildren, lights, areaLights);
                    continue;
                }
                var lg = tr.gameObject;
                lights.Add(light);
                areaLights.Add(lg);
                lg.SetActive(false);

                var children = tr.Cast<Transform>().ToList();
                FindAreaLight(children, lights, areaLights);
            }
        }
        private static List<GameObject> ParseLights(IReadOnlyCollection<Light> lights)
        {
            var realLights = new List<GameObject>();
            if(lights == null || lights.Count == 0) return realLights;
            realLights.AddRange(lights.Select(ParseLight).Where(parsed => parsed != null));
            return realLights;
        }
        private static GameObject ParseLight(Light light)
        {
            var lightComp = light.GetOrAddComponent<RealLight>();
            if (!lightComp) return null;
            var parsed = lightComp.ParseLight();
            Object.Destroy(lightComp);
            return parsed;
        }
    }
}
