using System;
using UnityEngine;
using STH.Core;

namespace STH.Combat.Strategies
{
    /// <summary>
    /// 부채꼴 발사 전략 - 여러 발을 부채꼴 형태로 발사
    /// </summary>
    public class FanFireStrategy : IFireStrategy
    {
        private readonly int count;
        private readonly float spreadAngle;

        /// <summary>
        /// 부채꼴 발사 전략을 생성합니다.
        /// </summary>
        /// <param name="count">발사체 개수</param>
        /// <param name="spreadAngle">전체 퍼짐 각도</param>
        public FanFireStrategy(int count, float spreadAngle)
        {
            this.count = count;
            this.spreadAngle = spreadAngle;
        }

        public void Fire(Transform origin, Action<Vector3, Quaternion> spawnCallback)
        {
            if (count <= 0) return;

            float startAngle = -spreadAngle / 2f;
            float angleStep = count > 1 ? spreadAngle / (count - 1) : 0f;

            for (int i = 0; i < count; i++)
            {
                float currentAngle = startAngle + (angleStep * i);
                Quaternion rotation = origin.rotation * Quaternion.Euler(0f, currentAngle, 0f);
                Vector3 position = origin.position;

                spawnCallback?.Invoke(position, rotation);
            }
        }
    }
}
