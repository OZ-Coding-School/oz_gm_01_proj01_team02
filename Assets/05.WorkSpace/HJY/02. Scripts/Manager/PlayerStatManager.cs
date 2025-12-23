
using UnityEngine;
using System;



public class PlayerStatManager : MonoBehaviour
{
    public static PlayerStatManager Instance;
    public static event Action OnStatChanged;

    // �⺻ ����
    [Header("Base Stats")]
    public float baseMaxHp = 1000; // �ִ� ü��
    public float baseAttack = 150; // ���ݷ�
    public float baseAttackSpeed = 1; // ���� �ӵ� (�д� ����) 
    public float baseCritRate = 0.05f; // ġ��Ÿ Ȯ��
    public float baseCritDamage = 2f; // ġ��Ÿ �����
    public float baseMoveSpeed = 5; // �̵� �ӵ�
    public float baseSuperTime = 0.5f; // �ǰ� ���� �ð� (���� �ǰ� ����)

    // ���� ����
    [Header("Current Stats")]
    public float maxHp;
    public float attack;
    public float attackSpeed;
    public float critRate;
    public float critDamage;
    public float moveSpeed;
    public float superTime;

    public float dodgeRate; // ȸ�� Ȯ��
    public float coinBonus; // ���� ���ʽ�

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

    // �ֽ� ���� ���
    public void RecalculateStats()
    {
        ResetStat();

        // foreach (var item in InventoryManager.Instance.equippedItems)
        // {
        //     if (item == null) continue;
        //     ApplyStats(item.stats);
        // }

        // foreach (var talent in InventoryManager.Instance.ownedTalents)
        // {
        //     if (talent == null) continue;
        //     ApplyStat(talent.stat);
        // }
        // OnStatChanged?.Invoke();
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

