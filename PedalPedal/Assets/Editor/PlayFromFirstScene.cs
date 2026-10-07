using UnityEditor;
using UnityEngine.SceneManagement;

[InitializeOnLoad]
public class PlayFromFirstScene
{
    static PlayFromFirstScene()
    {
        EditorApplication.playModeStateChanged += LoadFirstScene;
    }

    private static void LoadFirstScene(PlayModeStateChange state)
    {
        if (state == PlayModeStateChange.EnteredPlayMode)
        {
            // 0 is main menu scene index
            SceneManager.LoadScene(0);
        }
    }
}
