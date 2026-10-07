using UnityEngine;

public class YSorter : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    public bool justSortInStart;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void LateUpdate()
    {
        spriteRenderer.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}