
using UnityEngine;


[CreateAssetMenu(menuName = "Inventory/Equipment")]
public class EquipmentData: ScriptableObject
{
    [Header("Info")]
    public string equipmentName; // 영문 이름
    public string displayName; // 패널에 보여줄 한글 이름
    public EquipmentType type; 
    public Sprite icon; // UI로 크기 조정할 것!

    [TextArea(1, 3)]
    public string description; // 장비 설명 텍스트

    [TextArea(2, 5)]
    public string abilityDescription; // 장비의 효과 설명 텍스트
    
    [Header("Stats")]
    public StatValue[] stats;
    
    [Header("Armor Effect")]
    public ArmorEffect armorEffect;
    
    [Header("Ring Bonus")]
    public RingDamageBonus ringBonus;

}

