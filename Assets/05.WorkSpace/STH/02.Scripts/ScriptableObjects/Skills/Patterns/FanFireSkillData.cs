using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Characters.Player;
using STH.Combat.Strategies;

namespace STH.ScriptableObjects.Skills
{

    /// <summary>
    /// 패턴 스킬 데이터 - 발사 패턴을 추가함
    /// </summary>
    [CreateAssetMenu(fileName = "NewFanFireSkill", menuName = "STH/Skills/FanFireSkill")]
    public class FanFireSkillData : SkillData
    {
        [Header("Pattern Settings")]
        public int count = 3;
        public float angle = 30f;

        public override void Apply(PlayerController player)
        {
            player.AddStrategy(new FanFireStrategy(count, angle));
            Debug.Log("pattern apply");

        }
    }
}
