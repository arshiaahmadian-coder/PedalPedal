using UnityEngine;

public class ChangeScene : MonoBehaviour
{
    public string sceneMane;

    public void OnClick()
    {
        NavigationAnimator.Instance.TransparentToDart(sceneMane);
    }
}
