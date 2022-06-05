using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class HoldButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [HideInInspector] public bool isPressed;
    public UnityEvent onPointerDown;
    public UnityEvent onPointerUp;

    void Awake()
    {
        isPressed = false;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPressed = true;
        onPointerDown.Invoke();
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isPressed = false;
        onPointerUp.Invoke();
    }
}
