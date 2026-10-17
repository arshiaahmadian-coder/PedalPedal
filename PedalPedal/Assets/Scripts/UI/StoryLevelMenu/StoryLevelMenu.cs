using System;
using UnityEngine;

public class StoryLevelMenu : MonoBehaviour
{
    [SerializeField] GameObject PrevBtn;
    [SerializeField] GameObject NextBtn;

    public int currentPage = 1;
    public int maxPage;

    public event Action OnPageChanged;
    [HideInInspector]
    public Database playerData = new Database();

    private void Start()
    {
        if (SaveLoadManager.CheckFileExists(SaveLoadManager.GetInstnce().DataFileName))
            SaveLoadManager.GetInstnce().Load(playerData, SaveLoadManager.GetInstnce().DataFileName);
        else 
            SaveLoadManager.GetInstnce().Save(playerData, SaveLoadManager.GetInstnce().DataFileName);

        OnPageChanged();
    }

    public void OnNextPage() 
    {
        currentPage += 1;

        OnPageChanged();
        PrevBtn.SetActive(true);

        if (currentPage == maxPage) NextBtn.SetActive(false);
    }

    public void OnPrevPage()
    {
        currentPage -= 1;

        OnPageChanged();
        NextBtn.SetActive(true);

        if (currentPage == 1) PrevBtn.SetActive(false);
    }
}
