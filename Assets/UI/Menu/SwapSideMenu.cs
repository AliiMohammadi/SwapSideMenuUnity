using System.Collections;
using System.Collections.Generic;
using UnityEngine; 
using UnityEngine.UI; 

namespace Menu
{
    /// <summary>
    /// کلاس صفحه کشیدنی که برای عوض کردن صفحه با کشیدن دست اجام میشه
    /// </summary>
    public class SwapSideMenu : MonoBehaviour
    {
        [SerializeField]
        private RectTransform MenuList;//لیستی که محتواها زیر مجموعه اون میشن
        [SerializeField]
        private RectTransform Center; //نقطه مرکزی که میخوایم صفحه از اون جا شروع به محاسبه فاصله صفحه هات بکنه
        [SerializeField]
        private ScrollRectElite Scrollrect; //اسکرول پرنت 

        /// <summary>
        /// سرعت حرکت جا به جایی بین صفحه ها
        /// </summary>
        public float ScrollSpeed = 10;//سرعت جا به جایی صفحه ها
        /// <summary>
        /// منوی هدفی که قراره تمرکز کنیم روش
        /// </summary>
        public int TargetContentIndex;
        /// <summary>
        /// فاصله هر محتوا نسبت به هم
        /// </summary>
        public uint ContentDistance = 100; //سایز به پیکسل
        /// <summary>
        /// حداقل اندازه طولی ای که کاربر باید بکشد تا صفحه عوض شود.
        /// توجه شود که بر اساس یک واحد طول یونیتی است
        /// </summary>
        public float CeilingValue = 1;

        RectTransform[] Contents; // منو ها 

        uint ContentsSize;

        float[] ContentDistancs;

        void Start()
        {
            Scrollrect.OnEndDragScroll += SwapByTouchLength;
        }
        void OnEnable()
        {
            Refresh();
        }
        void Update()
        {
            if (!IsDragging())
            {
                FocusOnContent(TargetContentIndex);
            }
        }
        //تابع گرفتن خودکار منو ها
        void GetContent()
        {
            for (int i = 0; i < ContentsSize; i++)
            {
                Contents[i] = MenuList.transform.GetChild(i).gameObject.GetComponent<RectTransform>();
            }
        }
        //فاصله های منو هارو بر اساس ترتیبشون مرتب میکنه
        void SetContentsPositions(uint distance)
        {
            for (int i = 1; i < Contents.Length; i++)
            {
                float X = Contents[i - 1].anchoredPosition.x + distance;
                Contents[i].anchoredPosition = new Vector2(X, Contents[i].anchoredPosition.y);
            }
        }

        /// <summary>
        /// روش محاسبه بر اساس اندازه کشیدن کاربر روی صفحه 
        /// </summary>
        void SwapByTouchLength()
        {
            float Length = Scrollrect.GetHorizontalTouchLength();
            float Ceiling = Mathf.Abs(CeilingValue);

            if (Mathf.Abs(Length) >= Ceiling)
            {
                if (Length < 0)
                {
                    SetTargetContent(TargetContentIndex + 1);
                }
                else if (Length >= 0)
                {
                    SetTargetContent(TargetContentIndex - 1);
                }
            }
        }

        /// <summary>
        /// به پنلی با یک اندیس حرکت میکند و اسکرول میشود
        /// </summary>
        /// <param name="Index">اندیس پنل</param>
        void FocusOnContent(int Index)
        {
            if (Index+1 <= ContentDistancs.Length && Index >= 0 )
            {
                float targetX = 0.00f;
                float NewX = 0.00f;

                targetX = -(ContentDistance * Index);
                NewX = MenuList.anchoredPosition.x;

                if ((int)NewX != (int)targetX)
                {
                    Vector2 TargetPos;

                    //این شرط اضافه شده برای این که چک کنه که دقیق مکان هدف و منو به هم دیگه برسه
                    if (Mathf.Abs(targetX - NewX) <= 0.500f)
                    {
                        TargetPos = new Vector2(targetX, MenuList.anchoredPosition.y);
                    }
                    else
                    {
                        TargetPos = new Vector2(Mathf.Lerp(NewX, targetX, Time.deltaTime * ScrollSpeed), MenuList.anchoredPosition.y);
                    }

                    MenuList.anchoredPosition = TargetPos;
                }
            }
        }

        /// <summary>
        /// برسی میکند که کاربر در حال کشیدن این اسکرول هست یا نه
        /// </summary>
        /// <returns></returns>
        bool IsDragging()
        {
            return Scrollrect.IsDragging;
        }

        /// <summary>
        /// تابع برای گزاشتن پنل هدف و تمرکز بر روی اون پنل
        /// </summary>
        /// <param name="index"></param>
        public void SetTargetContent(int index)
        {
            if (index > ContentDistancs.Length-1)
            {
                index = ContentDistancs.Length-1;
            }
            if (index < 0)
            {
                index = 0;
            }

            TargetContentIndex = index;

        }
        public void Refresh()
        {
            ContentsSize = (uint)MenuList.transform.childCount;
            Contents = new RectTransform[ContentsSize];
            ContentDistancs = new float[ContentsSize];

            GetContent();
            SetContentsPositions(ContentDistance);

        }
    }
}

//---------------- Ali Mohammadi 
