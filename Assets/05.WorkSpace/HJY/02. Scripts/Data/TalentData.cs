
using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Talent")]
public class TalentData: ScriptableObject
{
    [Header("Info")]
    public string talentName;

    [Header("Stat")]
    public StatValue stat;
}

