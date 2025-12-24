using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Characters.Player;
using STH.Combat.Modifiers;

namespace STH.ScriptableObjects.Skills.Modifiers
{
    /// <summary>
    /// 리코셰 스킬 데이터 - 총알이 적에게 튕기는 능력 추가
    /// </summary>
    [CreateAssetMenu(fileName = "Modifier_Ricochet", menuName = "STH/Skills/Modifiers/Ricochet")]
    public class RicochetSkillData : SkillData
    {
        [Header("Modifier Settings")]
        public int bounceCount = 2;
        public float searchRadius = 15f;
        public float attackMultiplier = -0.35f;

        public override void Apply(PlayerController player)
        {
            player.AddModifier(new RicochetModifier(bounceCount, searchRadius, attackMultiplier));
            player.AddSkill(this);
            Debug.Log("modifier apply");
        }
    }
}
