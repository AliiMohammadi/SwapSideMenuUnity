using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CostumScrollRect : ScrollRect
{
    public bool IsDragging;
    public delegate void Method();
    public event Method OnEndDragScroll;

    Vector2 TouchStart;
    Vector2 TouchEnd;

    public float GetHorizontalTouchLength()
    {
        return (TouchEnd.x - TouchStart.x);
    }
    public override void OnBeginDrag(PointerEventData eventData)
    {
        base.OnBeginDrag(eventData);
        TouchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    public override void OnDrag(PointerEventData eventData)
    {
        IsDragging = true;
        base.OnDrag(eventData);
    }
    public override void OnEndDrag(PointerEventData eventData)
    {
        base.OnEndDrag(eventData);
        TouchEnd = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        IsDragging = false;

        try
        {
            OnEndDragScroll();
        }
        catch 
        {

        }
    }
}
