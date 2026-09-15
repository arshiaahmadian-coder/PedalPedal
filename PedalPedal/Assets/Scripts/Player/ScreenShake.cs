using UnityEngine;
using Unity.Cinemachine;

public class ScreenShake : MonoBehaviour
{
    public CinemachineCamera virtualCam;
    private CinemachineBasicMultiChannelPerlin noise;
    private float shakeTimer;

    void Start()
    {
        noise = virtualCam.GetComponent<CinemachineBasicMultiChannelPerlin>();
    }

    public void Shake(float amplitude, float frequency, float duration)
    {
        noise.AmplitudeGain = amplitude;
        noise.FrequencyGain = frequency;
        shakeTimer = duration;
    }

    void Update()
    {
        if (shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
            if (shakeTimer <= 0)
            {
                noise.AmplitudeGain = 0;
                noise.FrequencyGain = 0;
            }
        }
    }
}
