using UnityEngine;
using Unity.Cinemachine;

public class CinemachineSmoothZoom : MonoBehaviour
{
    [Header("Cinemachine")]
    public CinemachineCamera virtualCamera;

    [Header("Zoom Settings")]
    public float targetOrthoSize = 5f;
    public float zoomSpeed = 1.5f;

    private float currentSize;

    void Start()
    {
        if (virtualCamera == null)
            virtualCamera = GetComponent<CinemachineCamera>();

        currentSize = virtualCamera.Lens.OrthographicSize;
    }

    void Update()
    {
        currentSize = Mathf.Lerp(currentSize, targetOrthoSize, Time.deltaTime * zoomSpeed);
        virtualCamera.Lens.OrthographicSize = currentSize;
    }

    public void SetZoom(float newSize)
    {
        targetOrthoSize = newSize;
    }

    public void ZoomBy(float amount)
    {
        targetOrthoSize += amount;
        targetOrthoSize = Mathf.Clamp(targetOrthoSize, 2f, 15f);
    }
}