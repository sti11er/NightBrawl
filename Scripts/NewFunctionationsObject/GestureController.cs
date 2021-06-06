using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class GestureController : MonoBehaviour, IBeginDragHandler, IDragHandler, IPointerClickHandler
{
    public static string dir;

    public void OnPointerClick(PointerEventData eventData) { dir = null; }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y))
        {
            if (eventData.delta.x > 0){
                dir = "Right";
                Debug.Log("RIGHT");
            }
            else{
                dir = "Left";
                Debug.Log("LEFT");
            }
        }
        else
        { 
            if (eventData.delta.y > 0){
                dir = "Up";
                Debug.Log("UP");
            }
            else{
                dir = "Attack";
                Debug.Log("DOWN");
            }
        }
    }
    public void OnDrag(PointerEventData eventData){}
}
