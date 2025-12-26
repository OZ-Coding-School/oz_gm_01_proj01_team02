using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Characters.Player;
using STH.Combat.Modifiers;

namespace STH.ScriptableObjects.Skills.Modifiers
{
    /// <summary>
    /// 화상 스킬 데이터 - 총알에 화상 효과 추가
    /// </summary>
    [CreateAssetMenu(fileName = "Modifier_Fire", menuName = "STH/Skills/Modifiers/Fire")]
    public class FireSkillData : SkillData
    {
        [Header("Modifier Settings")]
        public float AttackMultiplier = 0.2f; // 공격력 계수
        public float duration = 3f;
        public float tickInterval = 0.5f;

        public override void Apply(PlayerController player)
        {
            float damagePerTick = player.Stats.attack * AttackMultiplier;
            player.AddModifier(new PoisonModifier(damagePerTick, duration, tickInterval));
            player.AddSkill(this);
            Debug.Log("modifier apply");
        }
    }
}
