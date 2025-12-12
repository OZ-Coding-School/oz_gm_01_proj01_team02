using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ScrollManager : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Scrollbar scrollbar;
    public Slider tabSlider;

    public RectTransform[] BtnRect, BtnImageRect;
    const int Size = 4;
    float[] pos = new float[Size];
    float distance, curPos, targetPos;
    bool isDrag;
    int targetIndex;

    void Start()
    {
        distance = 1f / (Size - 1);
        for (int i = 0; i < Size; i++) pos[i] = distance * i;
    }


    public void OnBeginDrag(PointerEventData eventData)
    {
        for (int i = 0; i < Size; i++)
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                curPos = SetPos();
            }
    }

    float SetPos()
    {
        for (int i = 0; i < Size; i++)
            if (scrollbar.value < pos[i] + distance * 0.5f && scrollbar.value > pos[i] - distance * 0.5f)
            {
                targetIndex = i;
                return pos[i];
            }
        return 0;
    }

    public void OnDrag(PointerEventData eventData)
    {
        isDrag = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDrag = false;

        targetPos = SetPos();

        print(curPos + "/" + targetPos + "/" + targetIndex);
    }


    void Update()
    {
        

        tabSlider.value = scrollbar.value;

        if(!isDrag)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, targetPos, 0.1f);

            for (int i = 0; i < Size; i++) BtnRect[i].sizeDelta = new Vector2(i == targetIndex ? 360 : 180, BtnRect[i].sizeDelta.y);
        }

        if (Time.time < 0.1f) return;
        for (int i = 0; i <Size; i++)
        {
            Vector3 BtnTargetPos = BtnRect[i].anchoredPosition3D;

            if (i == targetIndex)
            {
                BtnTargetPos.y = -23f;
            }
            BtnImageRect[i].anchoredPosition3D = Vector3.Lerp(BtnImageRect[i].anchoredPosition3D, BtnTargetPos, 0.25f);
        }
    }

    public void TabClick(int n)
    {
        targetIndex = n;
        targetPos = pos[n];
    }

    
}
