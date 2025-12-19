using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AngelBuffData", menuName = "Angel/Buff Data")]


public class AngelBuffData : ScriptableObject
{
    [Header("Type")]
    public AngelBuffType buffType;

    [Header("UI")]
    public string displayName;
    public Sprite icon;

    [Header("Balance (나중에 사용)")]
    public float value;

}
