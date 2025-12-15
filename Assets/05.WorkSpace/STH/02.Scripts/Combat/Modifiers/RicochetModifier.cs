using UnityEngine;
using STH.Core;
using STH.Combat.Projectiles;

namespace STH.Combat.Modifiers
{
    /// <summary>
    /// 리코셰 모디파이어 - 총알이 다른 적에게 튕김
    /// </summary>
    public class RicochetModifier : IBulletModifier
    {
        private readonly int bounceCount;
        private readonly float searchRadius;
        private int remainingBounces;

        /// <summary>
        /// 리코셰 모디파이어를 생성합니다.
        /// </summary>
        /// <param name="bounceCount">튕기는 횟수</param>
        /// <param name="searchRadius">다음 타겟 탐색 범위</param>
        public RicochetModifier(int bounceCount, float searchRadius = 10f)
        {
            this.bounceCount = bounceCount;
            this.searchRadius = searchRadius;
            this.remainingBounces = bounceCount;
        }

        public void OnHit(Bullet bullet, IDamageable target)
        {
            if (remainingBounces <= 0) return;

            // 다음 타겟 찾기
            Transform nextTarget = FindNextTarget(bullet.transform.position, target);
            if (nextTarget != null)
            {
                Vector3 direction = (nextTarget.position - bullet.transform.position).normalized;
                bullet.SetDirection(direction);
                bullet.PierceCount++; // 튕길 때는 파괴되지 않도록
                remainingBounces--;
            }
        }

        private Transform FindNextTarget(Vector3 position, IDamageable excludeTarget)
        {
            Collider[] colliders = Physics.OverlapSphere(position, searchRadius);
            Transform nearestTarget = null;
            float nearestDistance = float.MaxValue;

            foreach (var collider in colliders)
            {
                IDamageable damageable = collider.GetComponent<IDamageable>();
                if (damageable == null || damageable == excludeTarget) continue;

                float distance = Vector3.Distance(position, collider.transform.position);
                if (distance < nearestDistance)
                {
                    nearestDistance = distance;
                    nearestTarget = collider.transform;
                }
            }

            return nearestTarget;
        }
    }
}
