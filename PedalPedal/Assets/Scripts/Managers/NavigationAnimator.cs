using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NavigationAnimator : MonoBehaviour
{
    public static NavigationAnimator Instance;

    private string targetSceneName;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        DartToTransparent();
    }

    private void ChangeScene() => SceneManager.LoadScene(targetSceneName);

    public void DartToTransparent() {
        GetComponent<Animator>().SetBool("IsTransparent", true);
    }

    public void TransparentToDart() {
        GetComponent<Animator>().SetBool("IsTransparent", false);
    }

    public void TransparentToDart(Action onComplete) {
        GetComponent<Animator>().SetBool("IsTransparent", false);
        onComplete?.Invoke();
    }

    public void TransparentToDart(string sceneName) {
        GetComponent<Animator>().SetBool("IsTransparent", false);
        targetSceneName = sceneName;
        Invoke(nameof(ChangeScene), 0.5f);
    }
}
