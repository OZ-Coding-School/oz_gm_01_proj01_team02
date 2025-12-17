using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    RectTransform rect;
    public Vector3 screenOffset = new Vector3(0, 0, 0);

    void Awake()
    {
        rect = GetComponent<RectTransform>();    
        
    }

    void FixedUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(TestGameManager.Instance.player.transform.position) + screenOffset;       

    }
}
