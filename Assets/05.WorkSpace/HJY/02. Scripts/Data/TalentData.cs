
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

    // 영구 재능
    [Header("Permanent Talent Cost")]
    public int baseCost;      // 처음에 필요한 비용
    public int costIncrease;  // 업그레이드 시 비용
}

