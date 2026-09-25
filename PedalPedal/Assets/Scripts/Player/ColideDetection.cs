using Unity.Cinemachine;
using UnityEngine;

public class ColideDetection : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    [SerializeField] private Animator animator;
    [SerializeField] private ScreenShake screenShake;
    [SerializeField] private AudioClip impactClip;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private CinemachineCamera cinemachineCamera;

    public int colideAmount;

    private Vector3 playerStartPos;

    private void Start()
    {
        playerStartPos = playerController.transform.position;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 3 && playerController.canPaddle)
        {
            playerController.canPaddle = false;
            screenShake.Shake(1.2f, 1.2f, 0.2f);
            SoundManager.instance.PlaySoundEffect(impactClip, 0);
            animator.SetTrigger("Crack");
            rb.linearVelocity = Vector2.zero;
            Invoke("RestartLevel", 2);
            colideAmount += 1;
        }
    }

    private void RestartLevel()
    {
        // Reset Player State
        animator.SetTrigger("Reset");
        playerController.canPaddle = true;
        // reposition player position
        playerController.transform.position = playerStartPos;
        LevelManager.instance.levelTimer.ResetTimer();
    }
}
