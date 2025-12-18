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
        public STH.Core.Stats.StatType targetStat; // 인스펙터에서 Dropdown으로 선택 (MoveSpeed 등)
        public float percentage;    // 0.1 (10%)

        public override void Apply(PlayerController player)
        {
            // PlayerController는 CharacterStats를 가지고 있음
            player.Stats.IncreaseStat(targetStat, percentage);
            player.AddSkill(this);

            Debug.Log($"{targetStat} 스탯이 {percentage * 100}% 증가했습니다.");
        }
    }
}
