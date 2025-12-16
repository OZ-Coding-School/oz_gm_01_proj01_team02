
using UnityEngine;

public enum StatType
{
    // 기본
    MaxHP,
    Attack,
    AttackSpeed,
    CritRate,
    CritDamage,
    MoveSpeed,
    SuperTime,

    // 방어 및 보조
    DodgeRate,
    DamageReduce,
    ProjectileReduce,
    CollisionReduce,
    HealBonus,

    // 반지 및 재능
    CoinBonus,
    AttackSpeedPercent,
    MaxHpPercent
}

[System.Serializable]
public struct StatValue
{
    public StatType statType;
    public float value;
}

