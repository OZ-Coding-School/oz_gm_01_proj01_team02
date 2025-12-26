using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Characters.Player;
using STH.Combat.Strategies;

namespace STH.ScriptableObjects.Skills
{

    /// <summary>
    /// 패턴 스킬 데이터 - 발사 패턴을 추가함
    /// </summary>
    [CreateAssetMenu(fileName = "Pattern_ParallelFire", menuName = "STH/Skills/ParallelFireSkill")]
    public class ParalleSkillData : SkillData
    {
        [Header("Pattern Settings")]
        public int count = 3;
        public float spacing = 30f;

        public override void Apply(PlayerController player)
        {
            player.AddStrategy(new ParallelFireStrategy(count, spacing));
            player.AddSkill(this);
        }
    }
}
