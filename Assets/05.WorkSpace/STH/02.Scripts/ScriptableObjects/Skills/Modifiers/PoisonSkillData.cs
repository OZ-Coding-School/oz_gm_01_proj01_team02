using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Characters.Player;
using STH.Combat.Modifiers;

namespace STH.ScriptableObjects.Skills.Modifiers
{
    /// <summary>
    /// 독 스킬 데이터 - 총알에 독 효과 추가
    /// </summary>
    [CreateAssetMenu(fileName = "Modifier_Poison", menuName = "STH/Skills/Modifiers/Poison")]
    public class PoisonSkillData : SkillData
    {
        [Header("Modifier Settings")]
        public float AttackMultiplier = 0.35f; // 공격력 계수
        public float duration = 5f;
        public float tickInterval = 1f;

        public override void Apply(PlayerController player)
        {
            float damagePerTick = player.Stats.attack * AttackMultiplier;
            player.AddModifier(new PoisonModifier(damagePerTick, duration, tickInterval));
            player.AddSkill(this);
            Debug.Log("modifier apply");
        }
    }
}
