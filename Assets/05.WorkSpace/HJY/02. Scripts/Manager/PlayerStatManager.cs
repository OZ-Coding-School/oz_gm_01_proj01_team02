
using UnityEngine;
using System;

public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager Instance;
    public static event Action OnStatChanged;

    // 기본 스탯  
    [Header("Base Stats")]
    public float baseMaxHp = 1000; // 최대 체력
    public float baseAttack = 150;    // 공격력
    public float baseAttackSpeed = 1; // 공격 속도
    public float baseCritRate = 0.05f; // 치명타 확률
    public float baseCritDamage = 2f; // 치명타 대미지
    public float baseMoveSpeed = 5; // 이동 속도
    public float baseSuperTime = 0.5f; // 피격 무적 시간 -> 연속 피격 방지를 위함

    // 영구 재능 스탯
    [Header("Permanent Bonus Stats")]
    public float permanentmaxHpBonus;
    public float permanentAttackBonus;


    // 현재 스탯
    [Header("Current Stats")]
    public float maxHp;
    public float attack;
    public float attackSpeed;
    public float critRate;
    public float critDamage;
    public float moveSpeed;
    public float superTime;

    public float dodgeRate; // 회피율
    public float coinBonus; // 코인 보너스

    private void Awake()
    {
        Instance = this;
        ResetStat();        // 기본값 세팅
        RecalculateStats(); // 재계산한 현재 스탯 반영
    }

    // 초기화
    public void ResetStat()
    {
        maxHp = baseMaxHp;
        attack = baseAttack;
        attackSpeed = baseAttackSpeed;
        critRate = baseCritRate;
        critDamage = baseCritDamage;
        moveSpeed = baseMoveSpeed;
        superTime = baseSuperTime;

        dodgeRate = 0;
        coinBonus = 0;
    }

    // 계산
    public void RecalculateStats()
    {
        ResetStat();

        // 영구 재능
        maxHp += permanentmaxHpBonus;
        attack += permanentAttackBonus;
    

        // InventoryManager가 준비 안 됐으면 여기서 종료!
        if (InventoryManager.Instance == null)
        {
            OnStatChanged?.Invoke();
            return;
        }

        var inv = InventoryManager.Instance;

        // 장비 스탯
        if (inv.equippedWeapon != null)
            ApplyStats(inv.equippedWeapon.stats);

        if (inv.equippedArmor != null)
            ApplyStats(inv.equippedArmor.stats);

        if (inv.equippedRings != null)
        {
            foreach (var ring in inv.equippedRings)
            {
                if (ring == null) continue;
                ApplyStats(ring.stats);
            }
        }


        // 인게임 재능 스탯
        if (inv.ownedTalents != null)
        {
            foreach (var talent in inv.ownedTalents)
            {
                if (talent == null || talent.level <= 0) continue;

                StatValue stat = talent.data.statPerLevel;
                float totalValue = stat.value * talent.level;

                ApplyStat(new StatValue { statType = stat.statType, value = totalValue });
            }
        }

        // 스탯이 바뀐 걸 알림!
        OnStatChanged?.Invoke();
    }

    // 호출
    private void ApplyStats(StatValue[] stats)
    {
        foreach (var stat in stats)
        {
            ApplyStat(stat);
        }
    }

    // 스탯 적용
    public void ApplyStat(StatValue stat)
    {
        switch (stat.statType)
        {
            case StatType.MaxHP:
                maxHp += stat.value; // 최대 체력에 값 더하기
                break;

            case StatType.MaxHpPercent:
                maxHp *= (1 + stat.value); // 최대 체력에 비율 곱하기
                break;

            case StatType.Attack:
                attack += stat.value; // 공격력 증가
                break;

            case StatType.AttackSpeed:
                attackSpeed += stat.value; // 공격 속도 증가
                break;

            case StatType.AttackSpeedPercent:
                attackSpeed *= (1 + stat.value); // 공격 속도 비율 증가
                break;

            case StatType.CritRate: 
                critRate += stat.value; // 치명타 확률 증가
                break;

            case StatType.CritDamage: 
                critDamage += stat.value; // 치명타 대미지 증가
                break;

            case StatType.MoveSpeed: 
                moveSpeed += stat.value; // 이동 속도 증가
                break;

            case StatType.SuperTime: 
                superTime += stat.value; // 피격 무적 시간 증가
                break;

            case StatType.DodgeRate: 
                dodgeRate += stat.value; // 회피율 증가
                break;

            case StatType.CoinBonus: 
                coinBonus += stat.value; // 코인 보너스 증가
                break;
        }
    }
}
