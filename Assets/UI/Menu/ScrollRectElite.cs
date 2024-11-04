using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace Menu
{
    /// <summary>
    /// کلاس اسکرول رکت سفارشی که جهت رفع مشکل لیست عمودی داخل یک پنل وجود داشت , تشکیل شد
    /// </summary>
    public class ScrollRectElite : ScrollRect
    {
        public ScrollRect parentScroll;

        public bool HasScollParent = true;
        /// <summary>
        /// چک کردن این که ایا اسکرول اصلی داره کشیده میشه یا خیر
        /// </summary>
        public bool IsDragging;

        public delegate void Method();
        public event Method OnEndDragScroll;
        
        bool routeToParent;

        Vector2 TouchStart;
        Vector2 TouchEnd;

        /// <summary>
        /// بزرگی فاصله اسکرول افقی را برمیگرداند
        /// </summary>
        /// <returns>اگر کاربر به سمت بالا اسکرول کرده باشد مقدار برگشتی کوچیکتر از صفر خواهد بود و اگر کاربر به سمت پایین اسکرول کند
        /// مقدار برگشتی بزرگ تر از صفر خواهد بود</returns>
        public float GetHorizontalTouchLength()
        {
            return (TouchEnd.x - TouchStart.x);
        }

        public override void OnBeginDrag(PointerEventData eventData)
        {
            if (!horizontal && Mathf.Abs(eventData.delta.x) > Mathf.Abs(eventData.delta.y) * 1.5f)
                routeToParent = true;
            else if (!vertical && Mathf.Abs(eventData.delta.x) < Mathf.Abs(eventData.delta.y))
                routeToParent = true;
            else
                routeToParent = false;

            if (routeToParent)
            {
                TouchStart = Camera.main.ScreenToWorldPoint(Input.mousePosition); 
                parentScroll.OnBeginDrag(eventData);//اسکرول اصلی
            }
            else
                base.OnBeginDrag(eventData);//اسکرول پنل

        }
        public override void OnDrag(PointerEventData eventData)
        {
            if (routeToParent)
            {
                parentScroll.OnDrag(eventData);//اسکرول اصلی
            }
            else
                base.OnDrag(eventData);//اسکرول پنل

            IsDragging = true;
        }
        public override void OnEndDrag(PointerEventData eventData)
        {
            if (routeToParent)
            {
                parentScroll.OnEndDrag(eventData);//اسکرول اصلی
            }
            else
                base.OnEndDrag(eventData);//اسکرول پنل

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
}
