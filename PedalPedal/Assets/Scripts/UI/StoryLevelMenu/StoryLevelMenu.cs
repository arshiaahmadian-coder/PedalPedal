using ArabicTextSupport;
using System;
using UnityEngine;

public class StoryLevelMenu : MonoBehaviour
{
    [SerializeField] Animator animator;
    [SerializeField] private ArabicUIText coinText;
    public int currentPage = 1;
    public int maxPage;

    public event Action OnPageChanged;
    public Database playerData;

    private void Start()
    {
        playerData = new Database();
        if (SaveLoadManager.CheckFileExists(SaveLoadManager.GetInstnce().DataFileName))
            SaveLoadManager.GetInstnce().Load(playerData, SaveLoadManager.GetInstnce().DataFileName);
        else 
            SaveLoadManager.GetInstnce().Save(playerData, SaveLoadManager.GetInstnce().DataFileName);

        currentPage = GetPageNumber(playerData.lastUnlockedLevelID);
        coinText.Text = playerData.coinAmount.ToString();
        OnPageChanged();
    }

    public void OnNextPage() 
    {
        if (currentPage == maxPage)
        {
            return;
            // currentPage = 1;
        } else
        {
            currentPage += 1;
        }

        OnPageChanged();

        animator.SetTrigger("NextPage");
    }

    public void OnPrevPage()
    {
        if (currentPage == 1)
        {
            return;
            // currentPage = maxPage;
        } else
        {
            currentPage -= 1;
        }

        OnPageChanged();

        animator.SetTrigger("NextPage");
    }

    private int GetPageNumber(int levelNumber)
    {
        if (levelNumber <= 5) return 1;
        else if (levelNumber <= 10) return 2;
        else if (levelNumber <= 15) return 3;
        else if (levelNumber <= 20) return 4;
        else if (levelNumber <= 25) return 5;
        return 1;
    }
}
