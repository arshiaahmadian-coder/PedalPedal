using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager Instance;

    public float soundFxVolume = 1;
    public float musicSoundVolume = 1;

    private void Awake() 
    { 
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        SoundManager.instance.UpdateSoundVolume();
    }
}