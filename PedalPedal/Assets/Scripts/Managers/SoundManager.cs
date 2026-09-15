using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    [SerializeField] AudioSource effectsAudioSource;
    [SerializeField] AudioSource musicAudioSource;
    [SerializeField] AudioClip clickSound;

    private IEnumerator Start()
    {
        yield return null; 
        musicAudioSource.volume = SettingsManager.instance.musicSoundVolume;
    }

    public static SoundManager instance;
    private void Awake() { instance = this; }

    public void UpdateSoundVolume()
    {
        musicAudioSource.volume = SettingsManager.instance.musicSoundVolume;
    }

    public void PlaySoundEffect(AudioSource audioSource, AudioClip clip, float pitchChangeRatio = 0.05f)
    {
        audioSource.volume = SettingsManager.instance.soundFxVolume;
        audioSource.pitch = Random.Range(1 - pitchChangeRatio, 1 + pitchChangeRatio);
        audioSource.PlayOneShot(clip);
    }

    public void PlaySoundEffect(AudioClip clip, float pitchChangeRatio = 0.05f)
    {
        effectsAudioSource.volume = SettingsManager.instance.soundFxVolume;
        effectsAudioSource.pitch = Random.Range(1 - pitchChangeRatio, 1 + pitchChangeRatio);
        effectsAudioSource.PlayOneShot(clip);
    }

    public void PlayClickSound()
    {
        PlaySoundEffect(clickSound);
    }

    public IEnumerator StopMusicSlowly(float fadeDuration = 1f)
    {
        float startVolume = musicAudioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            musicAudioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            yield return null;
        }

        musicAudioSource.volume = 0f;
        musicAudioSource.Stop();
    }

    public IEnumerator StopMusicSlowly(AudioSource audioSource, float fadeDuration = 1f)
    {
        float startVolume = audioSource.volume;
        float elapsed = 0f;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, elapsed / fadeDuration);
            yield return null;
        }

        audioSource.volume = 0f;
        audioSource.Stop();
    }
}
