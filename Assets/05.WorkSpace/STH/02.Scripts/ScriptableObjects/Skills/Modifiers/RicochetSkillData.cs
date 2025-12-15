using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Characters.Player;
using STH.Combat.Modifiers;

namespace STH.ScriptableObjects.Skills.Modifiers
{
    /// <summary>
    /// 리코셰 스킬 데이터 - 총알이 적에게 튕기는 능력 추가
    /// </summary>
    [CreateAssetMenu(fileName = "NewRicochetSkill", menuName = "STH/Skills/Modifiers/Ricochet")]
    public class RicochetSkillData : SkillData
    {
        public override void Apply(PlayerController p)
        {

        }
    }
}
