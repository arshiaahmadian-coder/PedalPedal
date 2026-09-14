using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class CustomButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private bool OnButtonDown;
    [SerializeField] private UnityEvent onClick;
    [SerializeField] private Animator animator;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (!OnButtonDown) return;
        onClick?.Invoke();
        if (animator != null)
            animator.SetTrigger("Action");
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (OnButtonDown) return;
        onClick?.Invoke();
        if (animator != null)
            animator.SetTrigger("Action");
    }
}