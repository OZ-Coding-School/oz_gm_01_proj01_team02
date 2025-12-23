using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using STH.Core;
using STH.Characters.Player;
using STH.Characters.Enemy;

namespace STH.Combat.Projectiles
{
    public enum TypeEnums
    {
        Player, Enemy
    }
    /// <summary>
    /// 총알 클래스 - Modifiers를 싣고 날아가는 운반체
    /// </summary>
    public class Bullet : MonoBehaviour
    {
        [SerializeField] private float speed = 10f;
        [SerializeField] private float lifeTime = 5f;
        [SerializeField] private TypeEnums bulletOwner = TypeEnums.Player;

        private float damage;
        private float critRate;
        private float critDamage;
        private List<IBulletModifier> modifiers = new List<IBulletModifier>();
        private int pierceCount = 0;

        public float Damage => damage;
        public int PierceCount { get => pierceCount; set => pierceCount = value; }

        /// <summary>
        /// 총알을 초기화합니다.
        /// </summary>
        /// <param name="dmg">데미지</param>
        /// <param name="mods">적용할 모디파이어 리스트</param>

        public void Initialize(float dmg)
        {
            damage = dmg;
            StartCoroutine(ReturnToPoolAfterTime(lifeTime));
        }

        public void Initialize(float dmg, List<IBulletModifier> mods)
        {
            Initialize(dmg);

            ApplyModifiers(mods);
        }

        public void Initialize(PlayerStatManager stats, List<IBulletModifier> mods)
        {
            Initialize(stats.attack);
            critRate = stats.critRate;
            critDamage = stats.critDamage;

            // 치명타 계산
            int rand = Random.Range(0, 100);
            if (rand < critRate * 100f)
            {
                damage *= critDamage;
            }

            ApplyModifiers(mods);
        }

        private void ApplyModifiers(List<IBulletModifier> mods)
        {
            if (mods != null)
            {
                modifiers = new List<IBulletModifier>(mods);
            }

        }

        private void Update()
        {
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        private void OnTriggerEnter(Collider other)
        {
            IDamageable target = other.GetComponent<IDamageable>();
            if (target == null) return;
            if (bulletOwner == TypeEnums.Player && other.GetComponent<PlayerController>()) return;
            if (bulletOwner == TypeEnums.Enemy && other.GetComponent<Characters.Enemy.EnemyController>()) return;

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
                StopAllCoroutines();
                GameManager.Pool.ReturnPool(this);
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

        IEnumerator ReturnToPoolAfterTime(float delay)
        {
            yield return new WaitForSeconds(delay);
            GameManager.Pool.ReturnPool(this);
        }
    }
}
