using UnityEngine;
using System;
using System.Collections;

public class LetterThrower : MonoBehaviour
{
    public float throwDuration = 0.5f;
    public float arcHeight = 2.5f;
    public SpriteRenderer spriteRenderer;

    public void ThrowLetter(Transform letter, Vector2 startPos, Vector2 endPos, Action onMidPoint = null, Action onComplete = null)
    {
        StartCoroutine(ThrowCoroutine(letter, startPos, endPos, onMidPoint, onComplete));
    }

    private IEnumerator ThrowCoroutine(Transform letter, Vector2 start, Vector2 end, Action onMidPoint, Action onComplete)
    {
        float elapsed = 0f;
        bool midPointTriggered = false;
        letter.gameObject.SetActive(true);

        letter.position = start;

        while (elapsed < throwDuration)
        {
            elapsed += Time.deltaTime;
            float t = Mathf.Clamp01(elapsed / throwDuration);

            Vector2 currentPos = Vector2.Lerp(start, end, t);

            float heightOffset = arcHeight * 4f * t * (1f - t);
            currentPos.y += heightOffset;

            letter.position = currentPos;

            if (!midPointTriggered && t >= 0.5f)
            {
                midPointTriggered = true;
                onMidPoint?.Invoke();
            }

            yield return null;
        }

        letter.position = end;
        letter.gameObject.SetActive(false);

        onComplete?.Invoke();
    }
}