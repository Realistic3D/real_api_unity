using UnityEngine;
using UnityEngine.UI;

public class FadeOutImage : MonoBehaviour
{
    public float fadeDuration = 2f; // Duration of the fade-out effect in seconds

    private float _fadeTimer;
    private Image _image;

    private void Start()
    {
        _image = GetComponent<Image>();
        _fadeTimer = fadeDuration;
    }

    private void Update()
    {
        _fadeTimer -= Time.deltaTime;

        var alpha = _fadeTimer / fadeDuration;

        var color = _image.color;
        color.a = alpha;
        _image.color = color;

        if (_fadeTimer <= 0f)
        {
            Destroy(gameObject);
        }
    }
}