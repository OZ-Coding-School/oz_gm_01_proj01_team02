
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Equipment")]
    public List<EquipmentData> ownedEquipments;
    public EquipmentData[] equippedItems = new EquipmentData[4];

    [Header("Talent")]
    public List<TalentData> talentPool;          // 전체 재능 풀
    public List<TalentState> ownedTalents;       // 플레이어가 가진 재능

    private void Awake()
    {
        Instance = this;
    }


    // ===================== 장비 =====================

    public void EquipItem(EquipmentData item)
    {
        int index = (int)item.type;
        equippedItems[index] = item;
        PlayerStatManager.Instance.RecalculateStats();
    }

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


    // ===================== 재능 =====================

    public void AcquireTalent()
    {
        TalentData talent = GetRandomTalent();
        if (talent == null) return;

        TalentState state = ownedTalents.Find(t => t.data == talent);

        if (state == null)
        {
            ownedTalents.Add(new TalentState
            {
                data = talent,
                level = 1
            });
        }
        else
        {
            if (!state.IsMaxLevel)
                state.level++;
        }

        PlayerStatManager.Instance.RecalculateStats();
    }

    private TalentData GetRandomTalent()
    {
        List<TalentData> list = new();

        foreach (var t in talentPool)
        {
            TalentState state = ownedTalents.Find(s => s.data == t);

            if (state == null || !state.IsMaxLevel)
                list.Add(t);
        }

        if (list.Count == 0) return null;
        return list[Random.Range(0, list.Count)];
    }
}
