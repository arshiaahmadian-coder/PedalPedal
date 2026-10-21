using ArabicTextSupport;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuModel : MonoBehaviour
{
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
}
