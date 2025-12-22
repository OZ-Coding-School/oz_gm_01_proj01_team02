using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapData : MonoBehaviour
{
    public string mapName;
    public string sceneName;
    public Sprite mapSprite;

    [Header("UI Components")]
    [SerializeField] private Text mapNameText;

    private void Awake()
    {
        Image imge = GetComponent<Image>();
        if (imge != null && mapSprite != null)
        {
            imge.sprite = mapSprite;
        }

        UpdateMapNameText();
    }

    public void UpdateMapNameText()
    {
        if (mapNameText != null)
        {
            mapNameText.text = mapName;
        }   

    }    
    
}
