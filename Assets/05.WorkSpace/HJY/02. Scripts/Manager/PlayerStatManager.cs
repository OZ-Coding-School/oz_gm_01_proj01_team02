
using UnityEngine;
using System;



public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager Instance;
    public static event Action OnStatChanged;

    // 기본 스탯
    [Header("Base Stats")]
    public float baseMaxHp = 1000; // 최대 체력
    public float baseAttack = 150; // 공격력
    public float baseAttackSpeed = 1; // 공격 속도 (분당 공속) 
    public float baseCritRate = 0.05f; // 치명타 확률
    public float baseCritDamage = 2f; // 치명타 대미지
    public float baseMoveSpeed = 5; // 이동 속도
    public float baseSuperTime = 0.5f; // 피격 무적 시간 (연속 피격 방지)

    // 현재 스탯
    [Header("Current Stats")]
    public float maxHp;
    public float attack;
    public float attackSpeed;
    public float critRate;
    public float critDamage;
    public float moveSpeed;
    public float superTime;

    public float dodgeRate; // 회피 확률
    public float coinBonus; // 코인 보너스

    private void Awake()
    {
        Instance = this;
        RecalculateStats();
    }

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

    // 최신 스탯 계산
    public void RecalculateStats()
    {
        ResetStat();

        foreach (var item in InventoryManager.Instance.equippedItems)
        {
            if (item == null) continue;
            ApplyStats(item.stats);
        }

        foreach (var talent in InventoryManager.Instance.ownedTalents)
        {
            ApplyStat(talent.stat);
        }
        OnStatChanged?.Invoke();
    }

    private void ApplyStats(StatValue[] stats)
    {
        foreach (var stat in stats)
        {
            ApplyStat(stat);
        }
            
    }

    public void ApplyStat(StatValue stat)
    {
        switch (stat.statType)
        {
            case StatType.MaxHP: maxHp += stat.value; break;
            case StatType.MaxHpPercent: maxHp *= (1 + stat.value); break;
            case StatType.Attack: attack += stat.value; break;
            case StatType.AttackSpeed: attackSpeed += stat.value; break;
            case StatType.AttackSpeedPercent: attackSpeed *= (1 + stat.value); break;
            case StatType.CritRate: critRate += stat.value; break;
            case StatType.CritDamage: critDamage += stat.value; break;
            case StatType.MoveSpeed: moveSpeed += stat.value; break;
            case StatType.SuperTime: superTime += stat.value; break;
            case StatType.DodgeRate: dodgeRate += stat.value; break;
            case StatType.CoinBonus: coinBonus += stat.value; break;
        }
    }
}

