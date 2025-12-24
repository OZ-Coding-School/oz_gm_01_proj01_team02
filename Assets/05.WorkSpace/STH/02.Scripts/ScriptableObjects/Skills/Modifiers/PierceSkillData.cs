using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Characters.Player;
using STH.Combat.Modifiers;

namespace STH.ScriptableObjects.Skills.Modifiers
{
    /// <summary>
    /// 관통 스킬 데이터 - 총알에 관통 효과 추가
    /// </summary>
    [CreateAssetMenu(fileName = "Modifier_Pierce", menuName = "STH/Skills/Modifiers/Pierce")]
    public class PierceSkillData : SkillData
    {
        [Header("Modifier Settings")]
        public int pierceCount = 1;
        public float AttackMultiplier = -0.3f;

        public override void Apply(PlayerController player)
        {
            player.AddModifier(new PierceModifier(pierceCount, AttackMultiplier));
            player.AddSkill(this);
            Debug.Log("modifier apply");
        }
    }
}
