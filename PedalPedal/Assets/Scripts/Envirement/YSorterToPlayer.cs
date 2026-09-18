using UnityEngine;

public class YSorterToPlayer : MonoBehaviour
{
    private PlayerController playerController;
    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    
    private void LateUpdate()
    {
        if (transform.position.y > playerController.transform.position.y) 
            spriteRenderer.sortingOrder = -1;
        else spriteRenderer.sortingOrder = 20;
    }
}
