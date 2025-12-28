using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapCardView : MonoBehaviour
{
    [SerializeField] private Image mapImage;
    [SerializeField] private Text mapNameText;
    public MapData data;


    private void Awake()
    {
        Set(data);
    }

    public void Set(MapData mapData)
    {
        if (mapData == null) return;
        data = mapData;
        Refresh(mapData);
    }

    public void Refresh(MapData mapData)
    {
        
        if (mapData == null) return;
        
        data = mapData;
        mapImage.sprite = data.mapSprite;
        mapNameText.text = data.mapName;
    }


    public MapData GetData() => data;
}
