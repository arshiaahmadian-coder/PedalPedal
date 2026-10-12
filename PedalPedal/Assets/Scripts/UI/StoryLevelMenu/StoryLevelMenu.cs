using UnityEngine;

public class StoryLevelMenu : MonoBehaviour
{
    public int currentPage = 1;
    public int maxPage;

    [SerializeField] GameObject PrevBtn;
    [SerializeField] GameObject NextBtn;

    public void OnNextPage() 
    {
        currentPage += 1;

        PrevBtn.SetActive(true);

        if (currentPage == maxPage) NextBtn.SetActive(false);
    }

    public void OnPrevPage()
    {
        currentPage -= 1;

        NextBtn.SetActive(true);

        if (currentPage == 1) PrevBtn.SetActive(false);
    }
}
