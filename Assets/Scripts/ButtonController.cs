using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonController : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public UnityEvent onHold;

    private bool isHolding = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isHolding = true;
        Invoke("TriggerOnHold", 0.5f);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isHolding = false;
        CancelInvoke("TriggerOnHold");
    }

    private void TriggerOnHold()
    {
        if (isHolding)
        {
            onHold.Invoke();
            Invoke("TriggerOnHold", 0.05f);
        }
    }

}