using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Image))]
public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    [Tooltip("Invoked the moment the button is pressed down.")]
    public UnityEvent onHoldStart = new UnityEvent();

    [Tooltip("Invoked when the press ends (finger up or pointer exits the button).")]
    public UnityEvent onHoldEnd = new UnityEvent();

    /// <summary>True while the pointer is pressing this button.</summary>
    public bool IsHeld { get; private set; }

    void OnEnable()
    {
        // Safety: ensure we start in a clean state
        IsHeld = false;

        // Make sure the Image is set to receive raycasts
        var img = GetComponent<Image>();
        if (img != null) img.raycastTarget = true;
    }

    void OnDisable()
    {
        // If disabled while held, emit end once to keep state consistent
        if (IsHeld)
        {
            IsHeld = false;
            onHoldEnd.Invoke();
        }
    }

    public void OnPointerDown(PointerEventData e)
    {
        if (IsHeld) return;
        IsHeld = true;
        onHoldStart.Invoke();
        Debug.Log($"[HoldButton] DOWN on {name}");
    }
    public void OnPointerUp(PointerEventData e)
    {
        if (!IsHeld) return;
        IsHeld = false;
        onHoldEnd.Invoke();
        Debug.Log($"[HoldButton] UP on {name}");
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // If the finger drags off the button, treat as release
        if (!IsHeld) return;
        IsHeld = false;
        onHoldEnd.Invoke();
    }
}