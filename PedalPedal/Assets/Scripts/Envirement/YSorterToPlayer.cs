using UnityEngine;
using UnityEngine.Rendering;

public class YSorterToPlayer : MonoBehaviour
{
    public SortingGroup sortingGroup;
    public GameObject pivote;
    // public SpriteRenderer spriteRenderer;
    
    private void LateUpdate()
    {
        if (pivote.transform.position.y > PlayerController.instance.transform.position.y) 
            sortingGroup.sortingOrder = -1;
        else sortingGroup.sortingOrder = 20;
    }
}
