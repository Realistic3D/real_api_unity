using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LBPanel : MonoBehaviour
{
    public GameObject lbPref;
    private AnimateImage _image;
    public void SetStatus(float progress)
    {
        if (!_image)
        {
            _image = Instantiate(lbPref, transform).GetComponent<AnimateImage>();
        }
        _image.SetPercent(progress);
    }
}
