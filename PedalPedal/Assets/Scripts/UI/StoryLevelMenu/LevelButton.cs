using ArabicTextSupport;
using UnityEngine;
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
        return ID + ((storyLevelMenu.currentPage - 1) * 5);
    }

    public void StartLevel()
    {
        if (locked) return;
        
        NavigationAnimator.Instance.TransparentToDart("Level " + GetLevelID());
    }

    private void UpdateUI()
    {
        locked = false;

        int starNumber = storyLevelMenu.playerData.levelData[GetLevelID()];
        StarImageList[0].sprite = starNumber >= 1 ? trueStar : falseStar;
        StarImageList[1].sprite = starNumber >= 2 ? trueStar : falseStar;
        StarImageList[2].sprite = starNumber >= 3 ? trueStar : falseStar;
        StarImageList[0].gameObject.SetActive(true);
        StarImageList[1].gameObject.SetActive(true);
        StarImageList[2].gameObject.SetActive(true);

        levelNumber.Text = GetLevelID().ToString();

        if (storyLevelMenu.playerData.lastUnlockedLevelID < GetLevelID()) // locked
        {
            locked = true;
            levelBtnBackground.sprite = lockedLevelBG;
            StarImageList[0].gameObject.SetActive(false);
            StarImageList[1].gameObject.SetActive(false);
            StarImageList[2].gameObject.SetActive(false);
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
