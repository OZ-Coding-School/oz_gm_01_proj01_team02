using System;
using UnityEngine;
using STH.Core;

namespace STH.Combat.Strategies
{
    /// <summary>
    /// 원형 발사 전략 - 360도 전방향으로 발사
    /// </summary>
    public class CircleFireStrategy : IFireStrategy
    {
        private readonly int count;

        /// <summary>
        /// 원형 발사 전략을 생성합니다.
        /// </summary>
        /// <param name="count">발사체 개수</param>
        public CircleFireStrategy(int count)
        {
            this.count = count;
        }

        public void Fire(Transform origin, Action<Vector3, Quaternion> spawnCallback)
        {
            if (count <= 0) return;

            float angleStep = 360f / count;

            for (int i = 0; i < count; i++)
            {
                float currentAngle = angleStep * i;
                Quaternion rotation = origin.rotation * Quaternion.Euler(0f, currentAngle, 0f);
                Vector3 position = origin.position;

                spawnCallback?.Invoke(position, rotation);
            }
        }
    }
}
