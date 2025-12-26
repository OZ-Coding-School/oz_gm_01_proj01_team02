using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{

    RectTransform rect;
    public Vector3 screenOffset = new Vector3(0, 0, 0);
    private PlayerHealth player;

    void Awake()
    {
        rect = GetComponent<RectTransform>(); 
        player = FindObjectOfType<PlayerHealth>();   
        
    }

    void FixedUpdate()
    {
        rect.position = Camera.main.WorldToScreenPoint(player.transform.position) + screenOffset;       

    }
}
