using System;
using UnityEngine;
using UnityEngine.UI;

namespace REAL.Example
{
    public class InfoPanel : MonoBehaviour
    {
        public GameObject infoPref;

        public void SetStatus(string info)
        {
            Instantiate(infoPref, transform).GetComponent<InfoUI>().SetInfo(info);
        }
    }
}