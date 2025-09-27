using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(RectTransform))]
public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerExitHandler
{
    public PlayerController playerController;

    [Tooltip("Invoked the moment the button is pressed down.")]
    public UnityEvent onHoldStart;
    [Tooltip("Invoked when the press ends (finger up or pointer exits the button).")]
    public UnityEvent onHoldEnd;

    public bool IsHeld { get; private set; }

    void OnEnable()
    {
        StartCoroutine(FindAirplaneWhenReady());
    }

    private IEnumerator FindAirplaneWhenReady()
    {
        while (playerController == null)
        {
            playerController = FindFirstObjectByType<PlayerController>();
            yield return null;
        }

        Debug.Log("Airplane found and ready for HUD interaction");
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        if (IsHeld) return;
        IsHeld = true;
        onHoldStart?.Invoke();

        playerController.DepleteEnergy();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!IsHeld) return;
        IsHeld = false;
        onHoldEnd?.Invoke();

        playerController.PauseDepleteEnergy();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // If the finger drags off the button, consider that a release.
        if (!IsHeld) return;
        IsHeld = false;
        onHoldEnd?.Invoke();

        playerController.PauseDepleteEnergy();
    }
}