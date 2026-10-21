using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    private static SettingsManager Instance;

    public float volumeMaster = 1;
    private Database playerData;

    private void Awake() 
    { 
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static SettingsManager GetInstance()
    {
        if (Instance == null) Instance = FindFirstObjectByType<SettingsManager>();

        return Instance;
    }

    private void Start()
    {
        LoadData();
        SoundManager.GetInstnce().UpdateSoundVolume();
    }

    public void SaveData()
    {
        if (SaveLoadManager.CheckFileExists(SaveLoadManager.GetInstnce().DataFileName))
            SaveLoadManager.GetInstnce().Load(playerData, SaveLoadManager.GetInstnce().DataFileName);

        UpdateSoundVolume(volumeMaster);
        playerData.masterSound = volumeMaster;
        
        SaveLoadManager.GetInstnce().Save(playerData, SaveLoadManager.GetInstnce().DataFileName);
    }

    private void LoadData()
    {
        playerData = new Database();
        if (SaveLoadManager.CheckFileExists(SaveLoadManager.GetInstnce().DataFileName))
            SaveLoadManager.GetInstnce().Load(playerData, SaveLoadManager.GetInstnce().DataFileName);
        else SaveLoadManager.GetInstnce().Save(playerData, SaveLoadManager.GetInstnce().DataFileName);
        volumeMaster = playerData.masterSound;
    }

    public void UpdateSoundVolume(float volume)
    {
        volumeMaster = volume;
        SoundManager.GetInstnce().UpdateSoundVolume();
    }
}
