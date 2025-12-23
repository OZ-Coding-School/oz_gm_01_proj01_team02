
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Talent")]
public class TalentData: ScriptableObject
{
    [Header("Info")]
    public TalentType talentType;
    public string talentName;

    [TextArea]
    public string description;

    public Sprite icon;

    // 최대 레벨
    [Header("MaxLevel")]
    public int maxLevel = 10; // 최대 레벨을 10으로 설정

    // 레벨업 시 얻을 스탯
    [Header("Stat")]
    public StatValue statPerLevel;
}

