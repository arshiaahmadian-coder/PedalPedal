using UnityEngine;
using UnityEngine.Rendering;

public class StaticYSorter : MonoBehaviour
{
    public SortingGroup sortingGroup;

    void Awake()
    {
        sortingGroup.sortingOrder = Mathf.RoundToInt(-transform.position.y * 100);
    }
}