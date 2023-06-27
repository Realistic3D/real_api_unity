using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AnimateImage : MonoBehaviour
{
    public Text text;
    public float duration = 0.5f;
    public float colDelay = 0.05f;
    public float trDuration = 1f;
    
    private Image _image;
    private Vector2 _targetSize;
    private Vector2 _initialSize;
    private Coroutine _colCoroutine;
    
    private void Start()
    {
        _image = GetComponent<Image>();
        _targetSize = _image.rectTransform.sizeDelta;
        _initialSize = new Vector2(10f, 10f);
        _image.rectTransform.sizeDelta = _initialSize;
        StartCoroutine(AnimateSize());
        // _colCoroutine = StartCoroutine(ChangeColorContinuously());
    }

    public void SetPercent(float value)
    {
        var abs = (int) value;
        text.text = $"{abs}%";
        if(value >= 100f) GameObject.Destroy(gameObject);
    }
    
    #region Size Animation

    private IEnumerator AnimateSize()
    {
        var elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            var t = elapsedTime / duration;
            var newSize = Vector2.Lerp(_initialSize, _targetSize, t);
            _image.rectTransform.sizeDelta = newSize;
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        _image.rectTransform.sizeDelta = _targetSize;
    }

    #endregion

    #region Color Animation

    private IEnumerator ChangeColorContinuously()
    {
        while (true)
        {
            // Generate a random color
            var randomColor = Random.ColorHSV();
            // Transition the image color smoothly
            yield return StartCoroutine(TransitionColor(randomColor));
            yield return new WaitForSeconds(colDelay);
        }
    }

    private IEnumerator TransitionColor(Color targetColor)
    {
        var initialColor = _image.color;
        var elapsedTime = 0f;

        while (elapsedTime < trDuration)
        {
            // Calculate the current progress based on the elapsed time
            var t = elapsedTime / trDuration;
            // Interpolate the color between the initial color and the target color
            var newColor = Color.Lerp(initialColor, targetColor, t);
            // Update the image color
            _image.color = newColor;
            // Increment the elapsed time
            elapsedTime += Time.deltaTime;

            yield return null;
        }

        // Ensure the final color is set to the target color
        _image.color = targetColor;
    }

    private void OnDisable()
    {
        if (_colCoroutine != null)
        {
            StopCoroutine(_colCoroutine);
        }
    }

    private void OnDestroy()
    {
        if (_colCoroutine != null)
        {
            StopCoroutine(_colCoroutine);
        }
    }

    #endregion
    
}
