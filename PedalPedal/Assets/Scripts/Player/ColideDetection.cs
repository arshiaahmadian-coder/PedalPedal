using UnityEngine;
using UnityEngine.SceneManagement;

public class ColideDetection : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;
    [SerializeField] private ScreenShake screenShake;
    [SerializeField] private AudioClip impactClip;
    [SerializeField] private Rigidbody2D rb;

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 3 && playerController.canPaddle)
        {
            playerController.canPaddle = false;
            screenShake.Shake(1f, 1f, 0.2f);
            SoundManager.instance.PlaySoundEffect(impactClip, 0);
            animator.SetTrigger("Crack");
            rb.linearVelocity = Vector2.zero;
            Invoke("RestartScene", 2);
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
