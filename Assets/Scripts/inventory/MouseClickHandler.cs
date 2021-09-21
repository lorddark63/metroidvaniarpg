using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClickHandler : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public int pointerDown;
    public int pointerUp;

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        pointerDown = eventData.pointerId;

        if (pointerDown == -1)
        {
            Debug.Log("Left mouse click registered");
        }
        else if (pointerDown == -2)
        {
            Debug.Log("Right mouse click registered");
        }
        else if (pointerDown == -3)
        {
            Debug.Log("Center mouse click registered");
        }
        
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        pointerUp = eventData.pointerId;
        if (pointerUp == -1)
        {
            Debug.Log("Left mouse click registered");
        }
        else if (pointerUp == -2)
        {
            Debug.Log("Right mouse click registered");
        }
        else if (pointerUp == -3)
        {
            Debug.Log("Center mouse click registered");
        }
    }
}
