using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animatorLeft;
    [SerializeField] private Animator animatorRight;

    [Header("Paddle Settings")]
    [SerializeField] private float paddleForce;
    [SerializeField] private float paddleForceSide;
    [SerializeField] private float maxSpeed;

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip[] PedalClips;

    public bool canPaddle = true;

    public void PaddleNE() {
        Paddle(new Vector2(1, 1).normalized, paddleForce);
        animatorLeft.SetTrigger("Paddle");
    }

    public void PaddleE() {
        Paddle(Vector2.right, paddleForceSide);
        animatorLeft.SetTrigger("Paddle");
    }

    public void PaddleSE() {
        Paddle(new Vector2(1, -1).normalized, paddleForce);
        animatorLeft.SetTrigger("Paddle");
    }

    public void PaddleSW() {
        Paddle(new Vector2(-1, -1).normalized, paddleForce); 
        animatorRight.SetTrigger("Paddle");
    }

    public void PaddleW() {
        Paddle(Vector2.left, paddleForceSide);
        animatorRight.SetTrigger("Paddle");
    }

    public void PaddleNW() {
        Paddle(new Vector2(-1, 1).normalized, paddleForce);
        animatorRight.SetTrigger("Paddle");
    }

    private void Paddle(Vector2 direction, float force)
    {
        if (!canPaddle) return;

        direction.Normalize();
        float currentSpeedInDirection = Vector2.Dot(rb.linearVelocity, direction);

        // limitting speed
        rb.AddForce(direction * force, ForceMode2D.Impulse);

        if (rb.linearVelocity.magnitude > maxSpeed)
        {
            rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
        }

        // sound effect
        SoundManager.instance.PlaySoundEffect(audioSource, PedalClips[Random.Range(0, PedalClips.Length)]);
    }
}
