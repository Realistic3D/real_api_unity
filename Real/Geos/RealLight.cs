using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using UnityEngine;
using Object = UnityEngine.Object;

namespace REAL.Geometry
{
    public class RealLight : MonoBehaviour
    {
        #region Inspector

        public float intensity;
        public LightType lightType;
        public Vector3 scale;
        public Vector3 rotation;
        public Vector3 position;
        public bool castShadow;
        public string realName = "REAL_AREA_LIGHT";

        #endregion

        #region Private Readonly

        private const bool Portal = false;
        private const float BeamAngle = 180f;
        private const bool ShadowCaustics = false;
        private const bool MultipleImportance = true;

        private Light _areaLight;

        #endregion

        public GameObject ParseLight()
        {
            SetLight();
            if (lightType != LightType.Area) return null;
            SetData();
            
            var posName = $"position|{position.x}_{position.y}_{position.z}";
            var rotName = $"rotation|{rotation.x}_{rotation.y}_{rotation.z}";
            var scaleName = $"scale|{scale.x}_{scale.y}_{scale.z}";
            
            var scChild = new GameObject(scaleName);
            var posChild = new GameObject(posName);
            var rotChild = new GameObject(rotName);
            var realLight = new GameObject(realName);
            
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
            return realLight;
        }
        
        private void SetLight()
        {
            _areaLight = GetComponent<Light>();
            lightType = _areaLight.type;
            intensity = _areaLight.intensity;
            castShadow = _areaLight.shadows != LightShadows.None;
        }

        private void SetData()
        {
            var lTrans = transform;
            
            var pos = lTrans.position;
            var angles = lTrans.rotation.eulerAngles;
            
            var xPos = -pos.x;
            var yPos = pos.y;
            var zPos = pos.z;
            
            var xAxis = Mathf.Deg2Rad * angles.x - Mathf.Deg2Rad * 90f;
            var yAxis = Mathf.Deg2Rad * angles.z;
            var zAxis = -Mathf.Deg2Rad * angles.y;
            
            position = new Vector3(xPos, yPos, zPos);
            rotation = new Vector3(xAxis, yAxis, zAxis);
            
            var range = GetComponent<Light>().range;
            var lScale = GetComponent<Light>().areaSize;
            scale = new Vector3(lScale.x, range, lScale.y);
        }

        private Dictionary<string, string> ParseInfo()
        {
            var info = new Dictionary<string, string>();
            var color = ParseColor();
            
            info.Add("color", color);
            info.Add("type", realName);
            info.Add("portal", Portal.ToString().ToLower());
            info.Add("castShadow", castShadow.ToString().ToLower());
            info.Add("shadowCaustics", ShadowCaustics.ToString().ToLower());
            info.Add("multipleImportance", MultipleImportance.ToString().ToLower());
            info.Add("intensity", intensity.ToString(CultureInfo.InvariantCulture));
            info.Add("beamAngle", BeamAngle.ToString(CultureInfo.InvariantCulture));
            
            return info;
        }

        private string ParseColor()
        {
            var color = _areaLight.color;
            return $"r:{ParseNumber(color.r)}, g:{ParseNumber(color.g)}, b:{ParseNumber(color.b)}";
        }

        private static double ParseNumber(float value)
        {
            return Math.Round(value, 5);
        }
    }
}
