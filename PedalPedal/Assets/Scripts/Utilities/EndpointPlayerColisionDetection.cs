using Unity.VisualScripting;
using UnityEngine;

public class EndpointPlayerColisionDetection : MonoBehaviour
{
    public bool hasReached = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (hasReached) return;

        if (other.tag == "Player")
        {
            hasReached = true;
            LevelManager.instance.PlayerReachedEndPoint();
        }
    }
}
