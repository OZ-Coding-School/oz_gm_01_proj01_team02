using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[CreateAssetMenu(menuName = "Game/MapData")]
public class MapData : ScriptableObject
{
    public string mapName;
    public string sceneName;
    public Sprite mapSprite;
   
    [Header("Progress")]
    public int clearStage = 0;
}
