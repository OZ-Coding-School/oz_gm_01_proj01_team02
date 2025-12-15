using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Characters.Player;
using STH.Combat.Modifiers;

namespace STH.ScriptableObjects.Skills.Modifiers
{
    /// <summary>
    /// 독 스킬 데이터 - 총알에 독 효과 추가
    /// </summary>
    [CreateAssetMenu(fileName = "NewPoisonSkill", menuName = "STH/Skills/Modifiers/Poison")]
    public class PoisonSkillData : SkillData
    {
        public override void Apply(PlayerController p)
        {

        }
    }
}
