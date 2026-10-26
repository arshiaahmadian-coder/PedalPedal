using UnityEngine;

public class SounEffectPlayer : MonoBehaviour
{
    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float pitchRatio;

    public void PlaySoundEffect()
    {
        int index = Random.Range(0, audioClips.Length);
        SoundManager.GetInstnce().PlaySoundEffect(audioClips[index], pitchRatio);
    }
}
