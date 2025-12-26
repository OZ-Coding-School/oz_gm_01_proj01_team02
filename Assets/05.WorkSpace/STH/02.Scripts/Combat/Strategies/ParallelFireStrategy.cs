using System;
using UnityEngine;
using STH.Core;

namespace STH.Combat.Strategies
{
    /// <summary>
    /// 평행 멀티샷 전략 - 여러 발을 같은 방향으로 나란히 발사
    /// </summary>
    public class ParallelFireStrategy : IFireStrategy
    {
        private readonly int count;
        private readonly float spacing;

        /// <summary>
        /// 평행 멀티샷 발사 전략을 생성합니다.
        /// </summary>
        /// <param name="count">발사체 개수</param>
        /// <param name="spacing">발사체 간 좌우 간격</param>
        public ParallelFireStrategy(int count, float spacing)
        {
            this.count = count;
            this.spacing = spacing;
        }

        public void Fire(Transform origin, Action<Vector3, Quaternion> spawnCallback)
        {
            if (count <= 0) return;

            float totalWidth = spacing * (count - 1);
            float startOffset = -totalWidth / 2f;

            for (int i = 0; i < count; i++)
            {
                float offset = startOffset + (spacing * i);

                // 로컬 right 방향으로 오프셋 적용
                Vector3 position = origin.position + origin.right * offset;

                // 회전은 모두 동일 (정면)
                Quaternion rotation = origin.rotation;

                spawnCallback?.Invoke(position, rotation);
            }
        }
    }
}