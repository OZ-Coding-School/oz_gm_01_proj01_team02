using UnityEngine;
using STH.ScriptableObjects.Base;
using STH.Core;
using STH.Combat.Strategies;

namespace STH.ScriptableObjects.Patterns
{
    /// <summary>
    /// 평행 패턴 SO - 몬스터용 평행 발사 패턴
    /// </summary>
    [CreateAssetMenu(fileName = "Pattern_Parallel", menuName = "STH/Patterns/ParallelPattern")]
    public class ParallelPatternSO : AttackPatternSO
    {
        [Header("Parallel Settings")]
        public int count = 2;
        public float spacing = 0.3f;

        public override IFireStrategy CreateStrategy()
        {
            return new ParallelFireStrategy(count, spacing);
        }
    }
}
