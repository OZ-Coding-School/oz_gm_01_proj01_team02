using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Core;
using STH.Combat.Strategies;

namespace STH.ScriptableObjects.Patterns
{
    /// <summary>
    /// 원형 패턴 SO - 몬스터용 원형 발사 패턴
    /// </summary>
    [CreateAssetMenu(fileName = "Pattern_Circle", menuName = "STH/Patterns/CirclePattern")]
    public class CirclePatternSO : AttackPatternSO
    {
        [Header("Circle Settings")]
        public int count = 3;

        public override IFireStrategy CreateStrategy()
        {
            return new CircleFireStrategy(count);
        }
    }
}
