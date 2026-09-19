using UnityEngine;

public class RandomlyEnableOneObject : MonoBehaviour
{
    [SerializeField] GameObject[] objectList;

    private void Start()
    {
        objectList[Random.Range(0, objectList.Length)].SetActive(true);
    }
}
