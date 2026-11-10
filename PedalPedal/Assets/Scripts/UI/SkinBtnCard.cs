using ArabicTextSupport;
using UnityEngine.UI;
using UnityEngine;

public class SkinBtnCard : MonoBehaviour
{
    public int skinID;

    [Header("Shopping Btn")]
    [SerializeField] RectTransform rectTransform;
    [SerializeField] Image skinCardBG;
    [SerializeField] Sprite[] BackroundSpriteList; // 0 not selected | 1 selected
    [SerializeField] Color[] skinCardColors; // 0 not owned | 1 owned
    [SerializeField] ArabicUIText ButtonText;

    private void Start()
    {
        UpdateUI();
    }

    private void OnEnable()
    {
        ShopManager.GetInstance().OnDataChanged += UpdateUI;
    }

    private void OnDisable()
    {
        if (ShopManager.GetInstance() != null)
            ShopManager.GetInstance().OnDataChanged -= UpdateUI;
    }

    private void UpdateUI()
    {
        if (ShopManager.GetInstance().HasThisSkin(skinID)) // have this skin
        {
            ButtonText.Text = "";
            skinCardBG.color = skinCardColors[1];

            if (ShopManager.GetInstance().playerData.selectedSkinID == skinID) 
                skinCardBG.sprite = BackroundSpriteList[1];
            else skinCardBG.sprite = BackroundSpriteList[0];

        } else // dont have this skin
        {
            ButtonText.Text = ShopManager.GetInstance().skinPrices[skinID].ToString();
            skinCardBG.color = skinCardColors[0];
        }
    }

}
