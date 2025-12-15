using System.Collections.Generic;
using UnityEngine;
using STH.Core;

namespace STH.Combat.Projectiles
{
    /// <summary>
    /// 총알 클래스 - Modifiers를 싣고 날아가는 운반체
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifeTime = 5f;

        private float damage;
        private List<IBulletModifier> modifiers = new List<IBulletModifier>();
        private int pierceCount = 0;

        public float Damage => damage;
        public int PierceCount { get => pierceCount; set => pierceCount = value; }

        /// <summary>
        /// 총알을 초기화합니다.
        /// </summary>
        /// <param name="dmg">데미지</param>
        /// <param name="mods">적용할 모디파이어 리스트</param>
        public void Initialize(float dmg, List<IBulletModifier> mods)
        {
            damage = dmg;
            if (mods != null)
            {
                modifiers = new List<IBulletModifier>(mods);
            }
            Destroy(gameObject, lifeTime);
        }

        private void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable target = other.GetComponent<IDamageable>();
            if (target == null) return;

            // 기본 데미지 적용
            target.TakeDamage(damage);

            // 모든 모디파이어 효과 발동
            foreach (var modifier in modifiers)
            {
                modifier.OnHit(this, target);
            }

            // 관통력이 없으면 파괴
            if (pierceCount <= 0)
            {
                Destroy(gameObject);
            }
            else
            {
                pierceCount--;
            }
        }

        /// <summary>
        /// 총알의 방향을 변경합니다 (리코셰 등에서 사용).
        /// </summary>
        public void SetDirection(Vector3 direction)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }
    }
}
