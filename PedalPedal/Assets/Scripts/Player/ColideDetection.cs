using UnityEngine;
using UnityEngine.SceneManagement;

public class ColideDetection : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;
    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.layer == 3)
        {
            playerController.canPaddle = false;
            Invoke("RestartScene", 1);
        }
    }

    private void RestartScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
