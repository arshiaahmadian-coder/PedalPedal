
using UnityEngine;
using System.Collections;

public class ButtonPulseAnimation : MonoBehaviour
{
    [SerializeField, Range(1f, 1.5f)]
    private float scaleMultiplier = 1.1f;

    [SerializeField, Min(0.01f)]
    private float animationDuration = 0.15f;

    private Vector3 originalScale;
    private Coroutine animationCoroutine;

    private void Awake()
    {
        originalScale = transform.localScale;
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    public void PlayAnimation()
    {
        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = StartCoroutine(PulseAnimation());
    }

    private IEnumerator PulseAnimation()
    {
        yield return ScaleTo(originalScale * scaleMultiplier);
        yield return ScaleTo(originalScale * 0.95f);
        yield return ScaleTo(originalScale);

        animationCoroutine = null;
    }

    private IEnumerator ScaleTo(Vector3 targetScale)
    {
        Vector3 startScale = transform.localScale;
        float elapsed = 0f;

        while (elapsed < animationDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / animationDuration);
            t = Mathf.SmoothStep(0f, 1f, t);

            transform.localScale = Vector3.Lerp(
                startScale,
                targetScale,
                t
            );

            yield return null;
        }

        transform.localScale = targetScale;
    }
}
