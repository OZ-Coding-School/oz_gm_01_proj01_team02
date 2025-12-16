
using UnityEngine;

public enum ArmorEffectType
{
    None,
    LightningSplash,
    FireAura,
    FreezeThorns,
    PoisonAllEnemies
}

[System.Serializable]
public struct ArmorEffect
{
    public ArmorEffectType effectType;
    public float value;
}

