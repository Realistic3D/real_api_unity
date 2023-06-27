using UnityEngine;
using UnityEngine.UI;

namespace REAL.Example
{
    public class InfoUI : MonoBehaviour
    {
        public Text info;

        public void SetInfo(string message)
        {
            info.text = message;
            gameObject.AddComponent<FadeOutImage>();
        }
    }
}
