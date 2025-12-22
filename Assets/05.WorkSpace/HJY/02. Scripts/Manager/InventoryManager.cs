
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Equipment")]
    public List<EquipmentData> ownedEquipments;
    public EquipmentData[] equippedItems = new EquipmentData[4];

    [Header("Talent")]
    public List<TalentData> talentPool;
    public List<TalentData> ownedTalents;

    private void Awake()
    {
        Instance = this;
    }

    // Àåºñ Âø¿ë
    public void EquipItem(EquipmentData item)
    {
        int index = (int)item.type;
        equippedItems[index] = item;
        PlayerStatManager.Instance.RecalculateStats();
    }

    // Àåºñ ¹ÌÂø¿ë
    public void UnequipItem(EquipmentData item)
    {
        int index = (int)item.type;
        if (equippedItems[index] == item)
        {
            equippedItems[index] = null;
            PlayerStatManager.Instance.RecalculateStats();
        }
    }

    public bool IsEquipped(EquipmentData item)
    {
        int index = (int)item.type;
        return equippedItems[index] == item;
    }



    public void AcquireTalent()
    {
        TalentData talent = GetRandomTalent();
        if (talent == null) return;

        ownedTalents.Add(talent);
        PlayerStatManager.Instance.RecalculateStats();
    }

    private TalentData GetRandomTalent()
    {
        List<TalentData> list = new List<TalentData>();
        foreach (var t in talentPool)
        {
            if (!ownedTalents.Contains(t))
            {
                list.Add(t);
            }
        }
            

        if (list.Count == 0) return null;
        return list[Random.Range(0, list.Count)];
    }
}

