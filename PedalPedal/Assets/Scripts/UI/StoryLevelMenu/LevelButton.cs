using ArabicTextSupport;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class LevelButton : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] Image levelBtnBackground;
    [SerializeField] Image[] StarImageList;
    [SerializeField] ArabicUIText levelNumber;

    [Header("Sprites")]
    [SerializeField] Sprite falseStar;
    [SerializeField] Sprite trueStar;
    [SerializeField] Sprite currentLevelBG;
    [SerializeField] Sprite lockedLevelBG;
    [SerializeField] Sprite passedLevelBG;

    public StoryLevelMenu storyLevelMenu;
    public int ID;
    public bool locked = true;

    private void OnEnable()
    {
        storyLevelMenu.OnPageChanged += UpdateUI;
    }

    private void OnDisable()
    {
        storyLevelMenu.OnPageChanged -= UpdateUI;
    }

    private int GetLevelID()
    {
        return ID * storyLevelMenu.currentPage;
    }

    public void StartLevel()
    {
        if (locked) return;
        
        NavigationAnimator.Instance.TransparentToDart("Level " + GetLevelID());
    }

    private void UpdateUI()
    {
        locked = false;

        if (storyLevelMenu.playerData.lastUnlockedLevelID < GetLevelID()) // locked
        {
            locked = true;
            levelBtnBackground.sprite = lockedLevelBG;
            // disable stars object
        } else if (storyLevelMenu.playerData.lastUnlockedLevelID == GetLevelID()) // curent level
        {
            levelBtnBackground.sprite = currentLevelBG;
        } else // passed
        {
            levelBtnBackground.sprite = passedLevelBG;
        }
    }
}
