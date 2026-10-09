using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuModel : MonoBehaviour
{
    [SerializeField] private GameObject settingsDialog;
    [SerializeField] private GameObject MainMenuPannel;
    [SerializeField] private GameObject modeSelectDialog;

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
        MainMenuPannel.SetActive(false);
    }
    
    public void CloseSettingsDialog()
    {
        settingsDialog.SetActive(false);
        MainMenuPannel.SetActive(true);
    }

    public void OpenModeSelectDialog()
    {
        modeSelectDialog.SetActive(true);
        MainMenuPannel.SetActive(false);
    }
    
    public void CloseModeSelectDialog()
    {
        modeSelectDialog.SetActive(false);
        MainMenuPannel.SetActive(true);
    }

    public void GoToOtherGames()
    {
        print("GoToOtherGames");
    }
}
