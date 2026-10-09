using UnityEngine;
using UnityEngine.SceneManagement;

public class BackToMainMenu : MonoBehaviour
{
    public void GoToMainMenu()
    {
        NavigationAnimator.Instance.TransparentToDart("MainMenu");
    }
}
