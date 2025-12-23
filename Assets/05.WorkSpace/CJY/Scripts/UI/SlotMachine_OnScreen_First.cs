using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotMachine_OnScreen_First : MonoBehaviour
{
    private Canvas canvas;
    private GraphicRaycaster graphicRaycaster;

    public int highSortingOrder = 999;
    public int lowSortingOrder = 0;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        if(canvas == null ) canvas = gameObject.AddComponent<Canvas>();

        graphicRaycaster = GetComponent<GraphicRaycaster>();
        if( graphicRaycaster == null ) graphicRaycaster = gameObject.AddComponent<GraphicRaycaster>();
    }

    private void OnEnable()
    {
        if( canvas != null)
        {
            canvas.overrideSorting = true;
            canvas.sortingOrder = highSortingOrder;
        }
    }

    private void OnDisable()
    {
        if (canvas != null)
        {
            canvas.overrideSorting = false;
            canvas.sortingOrder = lowSortingOrder;
        }
    }
}
