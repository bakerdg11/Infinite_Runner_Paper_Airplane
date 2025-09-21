using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(RectTransform))]
public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Tooltip("Invoked the moment the button is pressed down.")]
    public UnityEvent onHoldStart;
    [Tooltip("Invoked when the press ends (finger up or pointer exits the button).")]
    public UnityEvent onHoldEnd;

    public bool IsHeld { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsHeld) return;
        IsHeld = true;
        onHoldStart?.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!IsHeld) return;
        IsHeld = false;
        onHoldEnd?.Invoke();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // If the finger drags off the button, consider that a release.
        if (!IsHeld) return;
        IsHeld = false;
        onHoldEnd?.Invoke();
    }
}