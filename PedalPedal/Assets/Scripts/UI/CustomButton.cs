using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private bool OnButtonDown;
    [SerializeField] private UnityEvent onClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!OnButtonDown) return;
        onClick?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnButtonDown) return;
        onClick?.Invoke();
    }
}
