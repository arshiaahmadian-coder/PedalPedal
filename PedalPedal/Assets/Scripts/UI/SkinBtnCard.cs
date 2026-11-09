using UnityEngine;

public class SkinBtnCard : MonoBehaviour
{
    public int skinID;
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
        // change UI
    }
}
