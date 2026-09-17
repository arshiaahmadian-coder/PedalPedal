using UnityEngine;

[CreateAssetMenu(fileName = "DialogData", menuName = "Scriptable Objects/DialogData")]
public class DialogData : ScriptableObject
{
    public bool multySprites = false;
    public bool multyVoiceLines = false;
    public Sprite[] dialogCharacterList;
    public string[] dialogTextList;
    public AudioClip[] dialogVoiceList;
}
