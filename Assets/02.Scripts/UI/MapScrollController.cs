

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MapScrollController : MonoBehaviour, IEndDragHandler
{
    public Scrollbar scrollbar;
    
    private float[] positions = { 0f, 0.5f, 1f };
    private bool isSnapping = false;
    private float target;
    

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
        float closet = positions[0];
        foreach (float p in positions)
        {
            if(Mathf.Abs(current - p) < Mathf.Abs(current - closet))
            {
                closet = p;
            }
        }

        target = closet;
        isSnapping = true;
    }

    
}