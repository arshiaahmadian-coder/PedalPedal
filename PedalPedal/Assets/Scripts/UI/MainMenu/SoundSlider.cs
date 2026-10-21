using UnityEngine;
using UnityEngine.UI;

public class SoundSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private void OnEnable()
    {
        slider.value = SettingsManager.GetInstance().volumeMaster;
    }

    public void UpdateVolume()
    {
        SettingsManager.GetInstance().UpdateSoundVolume(slider.value);
    }
}
