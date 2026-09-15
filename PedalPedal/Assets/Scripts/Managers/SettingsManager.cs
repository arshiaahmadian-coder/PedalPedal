using UnityEngine;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public static SettingsManager instance;

    public float soundFxVolume = 1;
    public float musicSoundVolume = 1;

    private void Awake() { instance = this; }

    private void Start()
    {
        SoundManager.instance.UpdateSoundVolume();
    }
}
