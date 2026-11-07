using UnityEngine;

public class ShopManager : MonoBehaviour
{
    public PlayerSkinController playerSkinController;

    public void OnSkinCardClicked(int skinID)
    {
        playerSkinController.ChangeSkin(skinID);
    }
}
