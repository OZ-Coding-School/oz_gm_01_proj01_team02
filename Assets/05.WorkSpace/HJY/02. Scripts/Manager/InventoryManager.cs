
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Equipment")]
    public List<EquipmentData> ownedEquipments = new();          // 플레이어가 소유한 장비 모든 목록

    [Header("Equipped Items")]
    // 장착 슬롯
    public EquipmentData equippedWeapon;                          // 무기 1개
    public EquipmentData equippedArmor;                           // 방어구 1개
    public EquipmentData[] equippedRings = new EquipmentData[2];  // 반지 2개

    [Header("Talent")]
    public List<TalentData> talentPool = new();          // 게임 안에 존재하는 전체 재능 풀
    public List<TalentState> ownedTalents = new();       // 플레이어가 현재 가진 재능 상태

    // 이벤트 처리 -> UI 패널들이 구독해서 갱신
    public event Action OnEquipmentChanged;
    public event Action OnTalentChanged;


    private void Start()
    {
        ImportDataManager(); // 게임 시작 시 DataManager 불러오기
    }

    private void Awake()
    {
        // 싱글톤 초기화
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // 씬 전환 이벤트 등록
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 씬이 바뀔 때마다 DataManager에서 아이템 가져오기
        ImportDataManager();
    }


    // ===================== 장비 =====================

    public void ImportDataManager()
    {
        if (DataManager.instance == null) return; // 안전장치

        var collected = DataManager.instance.collectedItem;

        foreach (var kvp in collected)
        {
            string itemName = kvp.Key;
            int count = kvp.Value;

            EquipmentData data = EquipmentDatabase.Instance.GetEquipmentByName(itemName);

            if (data == null) continue;

            for (int i = 0; i < count; i++)
            {
                AddEquipment(data);
            }
        }

        OnEquipmentChanged?.Invoke();
    }





    // 장비 획득
    public void AddEquipment(EquipmentData item)
    {
        if (!ownedEquipments.Contains(item))
        {
            ownedEquipments.Add(item);
            OnEquipmentChanged?.Invoke(); // UI 갱신 이벤트 호출
        }
    }


    // 장비 장착
    public void EquipItem(EquipmentData item) 
    {
        switch (item.type)
        {
            case EquipmentType.Weapon:
                equippedWeapon = item;
                break;

            case EquipmentType.Armor:
                equippedArmor = item;
                break;

            case EquipmentType.Ring:
                // 이미 같은 반지가 장착되어 있으면 무시
                if ((equippedRings[0] != null && equippedRings[0] == item) || (equippedRings[1] != null && equippedRings[1] == item))
                return;
                

                // 빈 슬롯에 장착
                if (equippedRings[0] == null)
                    equippedRings[0] = item;
                else if (equippedRings[1] == null)
                    equippedRings[1] = item;
                else
                {
                    // 두 슬롯 다 차 있으면 첫 번째를 교체
                    equippedRings[0] = item;
                }
                break;
        }

        PlayerStatManager.Instance.RecalculateStats();
        OnEquipmentChanged?.Invoke();

    }

    // 장비 해제
    public void UnequipItem(EquipmentData item)
    {
        switch (item.type)
        {
            case EquipmentType.Weapon:
                if (equippedWeapon == item) equippedWeapon = null;
                break;

            case EquipmentType.Armor:
                if (equippedArmor == item) equippedArmor = null;
                break;

            case EquipmentType.Ring:
                if (equippedRings[0] == item) equippedRings[0] = null;
                else if (equippedRings[1] == item) equippedRings[1] = null;
                break;
        }

        PlayerStatManager.Instance.RecalculateStats();
        OnEquipmentChanged?.Invoke();

    }

    // 장착 여부 확인
    public bool IsEquipped(EquipmentData item)
    {
        switch (item.type)
        {
            case EquipmentType.Weapon: return equippedWeapon == item;
            case EquipmentType.Armor: return equippedArmor == item;
            case EquipmentType.Ring: return equippedRings[0] == item || equippedRings[1] == item;
        }
        return false;
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
        OnTalentChanged?.Invoke(); // UI 갱신 이벤트 호출
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
        return list[UnityEngine.Random.Range(0, list.Count)];
    }
}
