using System;
using System.Collections.Generic;
using ArabicTextSupport;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public Database playerData;
    public PlayerSkinController playerSkinController;

    public event Action OnDataChanged;

    [SerializeField] private AudioClip[] audioClips;
    [SerializeField] private float pitchRatio;

    public List<int> skinPrices = new List<int>();

    public int selectedSkinID;

    [Header("Shopping Btn")]
    [SerializeField] Image ButtonIcon;
    [SerializeField] Sprite[] ButtonIconList;
    [SerializeField] Image ButtonBG;
    [SerializeField] ArabicUIText ButtonText;
    [SerializeField] ArabicUIText coinAmountText;

    private static ShopManager Instance;

    public static ShopManager GetInstance()
    {
        if (Instance == null) Instance = FindFirstObjectByType<ShopManager>();

        return Instance;
    }

    private void Start()
    {
        LoadData();

        OnSkinCardClicked(playerData.selectedSkinID);

        UpdateCoinAmount();
    }

    public void OnSkinCardClicked(int skinID)
    {
        playerSkinController.ChangeSkin(skinID);
        selectedSkinID = skinID;

        OnDataChanged?.Invoke();

        UpdateActionButton();
    }

    private void UpdateActionButton()
    {
        if (selectedSkinID == playerData.selectedSkinID) // this is selected skin
        {
            ButtonText.Text = "انتخاب شده";
            ButtonIcon.sprite = ButtonIconList[0];
        } else if (playerData.ownedSkins.Contains(selectedSkinID)) // player have this skin but not owned
        {
            ButtonText.Text = "انتخاب";
            ButtonIcon.sprite = ButtonIconList[1];
        } else // player dont have this skin
        {
            ButtonText.Text = skinPrices[selectedSkinID].ToString();

            if (playerData.coinAmount >= skinPrices[selectedSkinID]) // can buy it
            {
                ButtonIcon.sprite = ButtonIconList[2];
            } else // cant buy it
            {
                ButtonIcon.sprite = ButtonIconList[3];
            }
        }
    }

    public void OnActionButtonClicked()
    {
        if (playerData.ownedSkins.Contains(selectedSkinID)) // player have this skin but not selected
        {
            SelectSkin(selectedSkinID);
        } else // player dont have this skin
        {
            if (playerData.coinAmount >= skinPrices[selectedSkinID]) // can buy it
            {
                BuySkin(selectedSkinID);
            } else // cant buy it
            {
                CantBuySkin();
            }
        }
    }

    private void BuySkin(int skinID)
    {
        playerData.coinAmount -= skinPrices[skinID];
        playerData.ownedSkins.Add(skinID);
        // auto select item
        playerData.selectedSkinID = skinID;

        SaveData();
        UpdateCoinAmount();
        OnDataChanged?.Invoke();
        UpdateActionButton();

        SoundManager.GetInstnce().PlaySoundEffect(audioClips[0], pitchRatio);
    }

    private void SelectSkin(int skinID)
    {
        playerData.selectedSkinID = skinID;
        
        SaveData();
        OnDataChanged?.Invoke();
        UpdateActionButton();

        SoundManager.GetInstnce().PlaySoundEffect(audioClips[1], pitchRatio);
    }

    private void CantBuySkin()
    {
        SoundManager.GetInstnce().PlaySoundEffect(audioClips[2], pitchRatio);
    }

    private void UpdateCoinAmount()
    {
        coinAmountText.Text = playerData.coinAmount.ToString();
    }

    public bool HasThisSkin(int _skinID)
    {
        return playerData.ownedSkins.Contains(_skinID);
    }

    private void SaveData()
    {
        if (!SaveLoadManager.CheckFileExists(SaveLoadManager.GetInstnce().DataFileName))
            SaveLoadManager.GetInstnce().Load(playerData, SaveLoadManager.GetInstnce().DataFileName);

        SaveLoadManager.GetInstnce().Save(playerData, SaveLoadManager.GetInstnce().DataFileName);
    }

    private void LoadData()
    {
        playerData = new Database();
        if (SaveLoadManager.CheckFileExists(SaveLoadManager.GetInstnce().DataFileName))
            SaveLoadManager.GetInstnce().Load(playerData, SaveLoadManager.GetInstnce().DataFileName);
        else SaveLoadManager.GetInstnce().Save(playerData, SaveLoadManager.GetInstnce().DataFileName);
    }
}
