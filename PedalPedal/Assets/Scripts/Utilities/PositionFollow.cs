using UnityEngine;

public class PositionFollow : MonoBehaviour
{
    [SerializeField] private Transform target;

    private void LateUpdate()
    {
        transform.position = target.position;
    }
}
