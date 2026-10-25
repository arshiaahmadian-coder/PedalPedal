using ArabicTextSupport;
using UnityEngine;

public class MainMenuModel : MonoBehaviour
{
    [SerializeField] private GameObject InstagramCoinMessage;
    [SerializeField] private GameObject settingsDialog;
    [SerializeField] private Animator modeMenuAnimator;
    [SerializeField] private ArabicUIText coinText;

    private void Start()
    {
        Database playerData = new Database();
        if (SaveLoadManager.CheckFileExists(SaveLoadManager.GetInstnce().DataFileName))
            SaveLoadManager.GetInstnce().Load(playerData, SaveLoadManager.GetInstnce().DataFileName);
        else 
            SaveLoadManager.GetInstnce().Save(playerData, SaveLoadManager.GetInstnce().DataFileName);

        coinText.Text = playerData.coinAmount.ToString();
        if (playerData.hasFollowed) InstagramCoinMessage.SetActive(false);
    }

    public bool IsSettingDialogActive() => settingsDialog.activeSelf;

    public void OnStartClicked()
    {
        OpenModeSelectDialog();
    }

    public void GoToShop() {
        NavigationAnimator.Instance.TransparentToDart("Shop");
    }

    public void GoToStoryMenu() {
        NavigationAnimator.Instance.TransparentToDart("StoryLevelMenu");
    }

    public void GoToEndlessLevel() {
        // NavigationAnimator.Instance.TransparentToDart("EndlessLevel");
        NavigationAnimator.Instance.TransparentToDart("SampleScene");
    }

    public void OpenSettingsDialog()
    {
        settingsDialog.SetActive(true);
    }

    public void CloseSettingsDialog()
    {
        SettingsManager.GetInstance().SaveData();
        settingsDialog.SetActive(false);
    }

    public void OpenModeSelectDialog()
    {
        modeMenuAnimator.SetBool("IsUp", true);
    }

    public void CloseModeSelectDialog()
    {
        modeMenuAnimator.SetBool("IsUp", false);
    }

    public void GoToOtherGames()
    {
        print("GoToOtherGames");
    }

    public void GoToInstagram()
    {
        InstagramCoinMessage.SetActive(false);
        Database playerData = new Database();
        if (SaveLoadManager.CheckFileExists(SaveLoadManager.GetInstnce().DataFileName))
            SaveLoadManager.GetInstnce().Load(playerData, SaveLoadManager.GetInstnce().DataFileName);

        Application.OpenURL("https://www.instagram.com/arshia.ahmadian.dev?cplk=cnU1aGZienNsMzI2");
        CloseSettingsDialog();

        if (playerData.hasFollowed == false)
        {
            playerData.hasFollowed = true;
            playerData.coinAmount += 100;
            coinText.Text = playerData.coinAmount.ToString();
            SaveLoadManager.GetInstnce().Save(playerData, SaveLoadManager.GetInstnce().DataFileName);
            Invoke("OpenNotif", 1);
        }
    }

    private void OpenNotif()
    {
        NotificationManager.GetInstance().ShowCoinNotification(100);
    }

    public void OtherGames() {
        CloseSettingsDialog();

        Application.OpenURL("https://cafebazaar.ir/developer/837019198109");
    }
}
