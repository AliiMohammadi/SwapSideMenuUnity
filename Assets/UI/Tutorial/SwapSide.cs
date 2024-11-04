using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SwapSide : MonoBehaviour
{
    public float ScrollSpeed = 10;
    public int TargetIndex = 0;
    public int ContentDistance = 1080;
    public float CeilingValue = 0.5f;

    int ContentSize;
    float[] ContentDistances;

    public RectTransform Menulist;
    public RectTransform Center;
    public CostumScrollRect scrollrect;
    RectTransform[] Contents;

    private void Start()
    {
        scrollrect.OnEndDragScroll += SwapByTouchLength;
    }
    private void OnEnable()
    {
        Refresh();
    }
    private void Update()
    {
        if (!IsDragging())
        {
            FocusOnContent(TargetIndex);
        }
    }

    public void Refresh()
    {
        ContentSize = Menulist.transform.childCount;
        Contents = new RectTransform[ContentSize];
        ContentDistances = new float[ContentSize];

        ContentDistance = Camera.main.pixelWidth;

        GetContents();
        SetContentsPositions(ContentDistance);
    }

    void GetContents()
    {
        for (int i = 0; i < ContentSize; i++)
        {
            Contents[i] = Menulist.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
        }
    }
    void SetContentsPositions(int Distance)
    {
        for (int i = 0; i < Contents.Length; i++)
        {
            float X = Distance * i;
            Contents[i].anchoredPosition = new Vector2(X,Contents[i].anchoredPosition.y);
        }
    }

    public void SetTargetContent(int index)
    {
        if (index > ContentDistances.Length-1)
        {
            index = ContentDistances.Length - 1;
        }
        if (index < 0)
        {
            index = 0;
        }

        TargetIndex = index;
    }

    public bool IsDragging()
    {
        return scrollrect.IsDragging;
    }

    void SwapByTouchLength()
    {
        float Length = scrollrect.GetHorizontalTouchLength();
        float Ceiling = Mathf.Abs(CeilingValue);

        if (Mathf.Abs(Length) >= Ceiling)
        {
            if (Length < 0)
            {
                SetTargetContent(TargetIndex +1 );
            }
            else if (Length >= 0)
            {
                SetTargetContent(TargetIndex - 1);
            }
        }
    }

    public void FocusOnContent(int index)
    {
        if (index + 1 <= ContentDistances.Length && index >=0)
        {
            float targetX = 0.00f;
            float NewX = 0.00f;

            targetX = -(ContentDistance * index);
            NewX = Menulist.anchoredPosition.x;

            if ((int)NewX != (int)targetX)
            {
                Vector2 Targetpos;

                if (Mathf.Abs(targetX - NewX) <= 0.500f)
                {
                    Targetpos = new Vector2(targetX,Menulist.anchoredPosition.y);
                }
                else
                {
                    float speed = Time.deltaTime * ScrollSpeed;
                    Targetpos = new Vector2(Mathf.Lerp(NewX,targetX, speed), Menulist.anchoredPosition.y);
                }
                Menulist.anchoredPosition = Targetpos;
            }
        }
    }
}
