using UnityEngine;
using STH.Characters.Player;

namespace STH.ScriptableObjects.Base
{
    /// <summary>
    /// 스킬 데이터 추상 클래스 - 모든 스킬의 부모
    /// </summary>
    public abstract class SkillData : ScriptableObject
    {
        public string skillName;
        public Sprite icon;

        public abstract void Apply(PlayerController p);
    }
}
