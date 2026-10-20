using System;
using UnityEngine;

public class StoryLevelMenu : MonoBehaviour
{
    [SerializeField] Animator animator;
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
}
