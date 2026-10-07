using UnityEngine;
using UnityEngine.Rendering;

public class StaticYSorter : MonoBehaviour
{
    public SortingGroup sortingGroup;
    public SpriteRenderer spriteRenderer;

    void Awake()
    {
        if (spriteRenderer != null)
            spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);

        if (sortingGroup != null)
            sortingGroup.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}