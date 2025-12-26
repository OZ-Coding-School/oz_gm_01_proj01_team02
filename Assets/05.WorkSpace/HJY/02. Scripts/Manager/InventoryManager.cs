
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Equipment")]
    public List<EquipmentData> ownedEquipments;                    // 플레이어가 소유한 장비 모든 목록
    public EquipmentData[] equippedItems = new EquipmentData[4];   // 플레이어가 착용한 장비 목록 4개

    [Header("Talent")]
    public List<TalentData> talentPool;          // 게임 안에 존재하는 전체 재능 풀
    public List<TalentState> ownedTalents;       // 플레이어가 현재 가진 재능 상태

    private void Awake()
    {
        Instance = this;
    }


    // ===================== 장비 =====================


    // 장비 장착
    public void EquipItem(EquipmentData item) 
    {
        int index = (int)item.type;
        equippedItems[index] = item;
        PlayerStatManager.Instance.RecalculateStats();
    }

    // 장비 해제
    public void UnequipItem(EquipmentData item)
    {
        int index = (int)item.type;
        if (equippedItems[index] == item)
        {
            equippedItems[index] = null;
            PlayerStatManager.Instance.RecalculateStats();
        }
    }

    // 장착 여부 확인
    public bool IsEquipped(EquipmentData item)
    {
        int index = (int)item.type;
        return equippedItems[index] == item; // 있으면 반환
    }


    // ===================== 재능 =====================


    // 재능 획득
    public void AcquireTalent()
    {
        TalentData talent = GetRandomTalent(); // 랜덤 재능 선택
        if (talent == null) return;            // 없으면 종료

        // 재능 소유 여부 확인
        TalentState state = ownedTalents.Find(t => t.data == talent);

        if (state == null)
        {
            ownedTalents.Add(new TalentState
            {
                data = talent,
                level = 1 // 처음 얻으면 레벨 1로 시작
            });
        }
        else
        {
            if (!state.IsMaxLevel)
                state.level++; // 레벨 상승
        }

        PlayerStatManager.Instance.RecalculateStats(); // 스탯 재계산
    }

    // 재능 랜덤 선택 기능
    private TalentData GetRandomTalent()
    {
        List<TalentData> list = new();

        foreach (var t in talentPool)
        {
            TalentState state = ownedTalents.Find(s => s.data == t);

            if (state == null || !state.IsMaxLevel)
                list.Add(t);
        }

        if (list.Count == 0) return null; // 없으면 null 반환
        return list[Random.Range(0, list.Count)];
    }
}
