
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Equipment")]
public class EquipmentData: ScriptableObject
{
    [Header("Info")]
    public string equipmentName;
    public EquipmentType type;
    public Sprite icon;

    [Header("Stats")]
    public StatValue[] stats;
    
    [Header("Armor Effect")]
    public ArmorEffect armorEffect;
    
    [Header("Ring Bonus")]
    public RingDamageBonus ringBonus;
}

