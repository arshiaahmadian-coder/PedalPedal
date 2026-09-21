using UnityEngine;

public class RandomSpriteSelect : MonoBehaviour
{
    [SerializeField] SpriteRenderer spriteRenderer;
    [SerializeField] Sprite[] spriteList;
    

    private void Start()
    {
        spriteRenderer.sprite = spriteList[Random.Range(0, spriteList.Length)];
    }
}
