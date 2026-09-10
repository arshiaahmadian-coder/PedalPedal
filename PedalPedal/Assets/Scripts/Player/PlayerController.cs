using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;

    [Header("Paddle Settings")]
    [SerializeField] private float paddleForce;
    [SerializeField] private float paddleForceSide;
    [SerializeField] private float maxSpeed;

    public bool canPaddle = true;

    public void PaddleNE() {
        Paddle(new Vector2(1, 1).normalized, paddleForce);
    }

    public void PaddleE() {
        Paddle(Vector2.right, paddleForceSide);
    }

    public void PaddleSE() {
        Paddle(new Vector2(1, -1).normalized, paddleForce);
    }

    public void PaddleSW() {
        Paddle(new Vector2(-1, -1).normalized, paddleForce);
    }

    public void PaddleW() {
        Paddle(Vector2.left, paddleForceSide);
    }

    public void PaddleNW() {
        Paddle(new Vector2(-1, 1).normalized, paddleForce);
    }

    private void Paddle(Vector2 direction, float force)
    {
        if (!canPaddle) return;

        direction.Normalize();
        float currentSpeedInDirection = Vector2.Dot(rb.linearVelocity, direction);

        if (currentSpeedInDirection < 0)
        {
            rb.linearVelocity -= direction * currentSpeedInDirection;
        }

        rb.AddForce(direction * force, ForceMode2D.Impulse);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }
    }
}
