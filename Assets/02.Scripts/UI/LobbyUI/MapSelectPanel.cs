using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapSelectPanel : MonoBehaviour, IEndDragHandler
{
    public Scrollbar scrollbar;
    public MapData[] mapCard;
    
    private float[] positions = { 0f, 0.5f, 1f };
    public int mapIndex;
    private bool isSnapping = false;
    private float target;

    // public int currentSelectedIndex;

    [Header("UI 여닫용")]
    public GameObject mapSelectPanel;
    public GameObject tab;
    public GameObject[] otherPanels;
    

    void Update()
    {
        if (isSnapping)
        {
            scrollbar.value = Mathf.Lerp(scrollbar.value, target, Time.deltaTime * 8f);

            if (Mathf.Abs(scrollbar.value - target) < 0.001f)
            {
                scrollbar.value = target;
                isSnapping = false;
            }
        }
    }

    

    public void OnEndDrag(PointerEventData eventData)
    {
        float current = scrollbar.value;
        int closetIndex = 0;
        float minDistance = Mathf.Abs(current - positions[0]);

        for (int i = 0; i < positions.Length; i++)
        {
            float dist = Mathf.Abs(current - positions[i]);
            if(dist < minDistance)
            {
                minDistance = dist;
                closetIndex = i;
            }
        }

        target = positions[closetIndex];
        mapIndex = closetIndex;
        isSnapping = true;
    }

    public void OnScrollValueChanged(float value)
    {
        for (int i = 0; i < positions.Length; i++)

        if (Mathf.Abs(value - positions[i]) < 0.001f)
            {
                mapIndex = i;
                break;
            }
    }

    
     public void CloseMapSelectPanel()
    {

        foreach (var panel in otherPanels)
        panel.SetActive(true);
        
        tab.SetActive(true);
        mapSelectPanel.SetActive(false);

    }

    
}