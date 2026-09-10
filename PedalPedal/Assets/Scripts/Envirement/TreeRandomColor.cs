using UnityEngine;

public class TreeRandomColor : MonoBehaviour
{
    [SerializeField] GameObject[] Trees;

    private void Start()
    {
        Trees[Random.Range(0, Trees.Length)].SetActive(true);
    }
}
