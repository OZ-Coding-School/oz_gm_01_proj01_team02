using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Core;
using STH.Combat.Strategies;

namespace STH.ScriptableObjects.Patterns
{
    /// <summary>
    /// 부채꼴 패턴 SO - 몬스터용 부채꼴 발사 패턴
    /// </summary>
    [CreateAssetMenu(fileName = "NewFanPattern", menuName = "STH/Patterns/FanPattern")]
    public class FanPatternSO : AttackPatternSO
    {
        [Header("Fan Settings")]
        public int count = 3;
        public float angle = 45f;

        public override IFireStrategy CreateStrategy()
        {
            return new FanFireStrategy(count, angle);
        }
    }
}
