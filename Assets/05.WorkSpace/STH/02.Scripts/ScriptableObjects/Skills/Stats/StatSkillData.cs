using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Characters.Player;
using STH.Core.Stats;
namespace STH.ScriptableObjects.Skills
{
    /// <summary>
    /// 스탯 스킬 데이터 - 플레이어 스탯을 증가시킴
    /// </summary>
    [CreateAssetMenu(fileName = "Stat_NewStat", menuName = "STH/Skills/StatSkill")]
    public class StatSkillData : SkillData
    {
        public StatValue statValue;

        public override void Apply(PlayerController player)
        {

            // PlayerController는 CharacterStats를 가지고 있음
            player.Stats.ApplyStat(statValue);
            player.AddSkill(this);
        }
    }
}
