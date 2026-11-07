using System.Collections.Generic;
using UnityEngine;

public class PlayerSkinController : MonoBehaviour
{
    public List<PlayerSkin> playerSkins = new List<PlayerSkin>();

    [Header("Renderers")]
    [SerializeField] SpriteRenderer LeftCharRenderer1;
    [SerializeField] SpriteRenderer LeftCharRenderer2;
    [SerializeField] SpriteRenderer RightCharRenderer1;
    [SerializeField] SpriteRenderer RightCharRenderer2;

    public void ChangeSkin(int skinID)
    {
        foreach (PlayerSkin skin in playerSkins)
        {
            if (skin.ID == skinID)
            {
                LeftCharRenderer1.sprite = skin.leftCharacter;
                LeftCharRenderer2.sprite = skin.leftCharacter;
                RightCharRenderer1.sprite = skin.rightCharacter;
                RightCharRenderer2.sprite = skin.rightCharacter;
                break;
            }
        }
    }
}
