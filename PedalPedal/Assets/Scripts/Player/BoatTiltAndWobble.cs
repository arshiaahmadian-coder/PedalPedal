using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class BoatTiltAndWobble : MonoBehaviour
{
    public float maxTiltAngle = 12f;

    public float tiltSensitivity = 8f;

    public float tiltSmoothSpeed = 6f;

    public float idleWobbleAmount = 1.8f;

    public float moveWobbleAmount = 2.5f;
    public float wobbleSpeed = 2.2f;
    public float returnSpeed = 4f;

    public Rigidbody2D rb;
    private float currentTilt = 0f;
    private float wobbleTimer = 0f;

    void Update()
    {
        float velocityX = rb. linearVelocity.x;

        float targetTilt = -Mathf.Clamp(velocityX * tiltSensitivity, -maxTiltAngle, maxTiltAngle);

        if (Mathf.Abs(velocityX) < 0.15f)
        {
            currentTilt = Mathf.Lerp(currentTilt, 0f, Time.deltaTime * returnSpeed);
        }
        else
        {
            currentTilt = Mathf.Lerp(currentTilt, targetTilt, Time.deltaTime * tiltSmoothSpeed);
        }

        wobbleTimer += Time.deltaTime * wobbleSpeed;

        float speedFactor = Mathf.Clamp01(Mathf.Abs(velocityX) * 0.4f);
        float currentWobbleAmount = Mathf.Lerp(idleWobbleAmount, moveWobbleAmount, speedFactor);

        float wobble = Mathf.Sin(wobbleTimer) * currentWobbleAmount;

        transform.rotation = Quaternion.Euler(0f, 0f, (currentTilt + wobble) * -1);
    }
}