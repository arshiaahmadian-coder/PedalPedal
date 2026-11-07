using UnityEngine;

[CreateAssetMenu(fileName = "PlayerSkin", menuName = "Scriptable Objects/PlayerSkin")]
public class PlayerSkin : ScriptableObject
{
    public int ID;
    public Sprite leftCharacter;
    public Sprite rightCharacter;
}
