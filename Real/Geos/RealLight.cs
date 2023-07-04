using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using REAL.Geometry;
using UnityEngine;

namespace REAL.Component
{
    [ExecuteAlways]
    public class RealLight : MonoBehaviour
    {
        
        #region Inspector

        public float intensity = 1f;
        public bool castShadow = true;
        public Color color = Color.white;
        
        public LightType type = LightType.Area;

        #endregion

        #region Private Readonly

        private const string RealName = "REAL_AREA_LIGHT";
        
        private GameObject _planeObject;

        #endregion

        [HideInInspector] public GameObject realLight;

        private void Awake()
        {
            transform.name = RealName;
            Init();
            RealMesh.CreateArea(gameObject);
        }
        
        private void Init()
        {
            var go = gameObject;
#if UNITY_EDITOR
            if(go.GetComponent<MeshFilter>()) DestroyImmediate(go.GetComponent<MeshFilter>());
            if(go.GetComponent<MeshRenderer>()) DestroyImmediate(go.GetComponent<MeshRenderer>());
#else
            if(go.GetComponent<MeshFilter>()) Destroy(go.GetComponent<MeshFilter>());
            if(go.GetComponent<MeshRenderer>()) Destroy(go.GetComponent<MeshRenderer>());
#endif
        }
        
        public void ParseLight()
        {
            if (type != LightType.Area) return;

            var lTrans = transform;
            
            var position = GetPos();
            var quaternion = GetQuaternion();
            var scale = lTrans.localScale;
            
            var posName = $"position|{position.x}_{position.y}_{position.z}";
            var rotName = $"quaternion|{quaternion.x}_{quaternion.y}_{quaternion.z}_{quaternion.w}";
            var scaleName = $"scale|{scale.x}_{scale.y}_{scale.z}";
            
            realLight = new GameObject(RealName);
            var scChild = new GameObject(scaleName);
            var posChild = new GameObject(posName);
            var rotChild = new GameObject(rotName);
            
            scChild.transform.SetParent(realLight.transform);
            posChild.transform.SetParent(realLight.transform);
            rotChild.transform.SetParent(realLight.transform);

            var info = ParseInfo();
            var keys = info.Keys;

            foreach (var keyChild in 
                     from key in keys let value = info[key] 
                     select $"{key}|{value}" into keyName 
                     select new GameObject(keyName))
            {
                keyChild.transform.SetParent(realLight.transform);
            }
            
            gameObject.SetActive(false);
        }

        public void Reset()
        {
            if(realLight) Destroy(realLight);
            gameObject.SetActive(true);
        }
        
        private Dictionary<string, string> ParseInfo()
        {
            var info = new Dictionary<string, string>();
            var rgb = ParseColor();
            
            info.Add("color", rgb);
            info.Add("portal", "false");
            info.Add("beamAngle", "180");
            info.Add("type", "REAL_AREA_LIGHT");
            info.Add("shadowCaustics", "false");
            info.Add("multipleImportance", "true");
            info.Add("castShadow", castShadow.ToString().ToLower());
            info.Add("intensity", intensity.ToString(CultureInfo.InvariantCulture));
            
            return info;
        }

        private string ParseColor()
        {
            // var color = _areaLight.color;
            return $"r:{ParseNumber(color.r)}, g:{ParseNumber(color.g)}, b:{ParseNumber(color.b)}";
        }
        
        private Vector3 GetPos()
        {
            var position = transform.position;
            return new Vector3(-position.x, position.y, position.z);
        }
        
        private Quaternion GetQuaternion()
        {
            var meshRot = transform.rotation.eulerAngles;
            var quarter = Quaternion.Euler(meshRot);
            return new Quaternion(quarter.x, quarter.z, -quarter.y, quarter.w);
        }
        
        private static double ParseNumber(float value)
        {
            return Math.Round(value, 5);
        }
    }
}
