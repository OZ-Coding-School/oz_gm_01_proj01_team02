using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Characters.Player;

namespace STH.ScriptableObjects.Skills
{
    /// <summary>
    /// 스탯 스킬 데이터 - 플레이어 스탯을 증가시킴
    /// </summary>
    [CreateAssetMenu(fileName = "NewStatSkill", menuName = "STH/Skills/StatSkill")]
    public class StatSkillData : SkillData
    {
        public override void Apply(PlayerController p)
        {

        }
    }
}
