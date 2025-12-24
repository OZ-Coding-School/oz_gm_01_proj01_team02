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
        [SerializeField] private LayerMask targetLayer;

        private float critRate = 0f;
        private float critDamage = 2f;
        private List<IBulletModifier> modifiers = new List<IBulletModifier>();
        private int pierceCount = 0;

        public float damage;
        public int PierceCount { get => pierceCount; set => pierceCount = value; }

        /// <summary>
        /// 총알을 초기화합니다.
        /// </summary>
        /// <param name="dmg">데미지</param>
        /// <param name="mods">적용할 모디파이어 리스트</param>

        // 적 공격 - 치명타 x, mods x
        public void Initialize(float dmg)
        {
            SetDamageWithCritical(dmg);
            StartCoroutine(ReturnToPoolAfterTime(lifeTime));
        }

        // 플레이어 공격 - 치명타 o, mods o
        public void Initialize(PlayerStatManager stats, List<IBulletModifier> mods)
        {
            critRate = stats.critRate;
            critDamage = stats.critDamage;
            Initialize(stats.attack);

            ApplyModifiers(mods);
        }

        // 치명타 확인, 데미지 설정
        public void SetDamageWithCritical(float dmg)
        {
            int rand = Random.Range(0, 100);
            if (rand < critRate * 100f)
            {
                damage = dmg * critDamage;
            }
            else
            {
                damage = dmg;
            }
        }

        private void ApplyModifiers(List<IBulletModifier> mods)
        {
            modifiers.Clear();

            if (mods == null) return;

            // Stateful modifier는 새 인스턴스 생성, Stateless는 참조 공유
            foreach (var mod in mods)
            {
                if (mod is IStatefulModifier stateful)
                {
                    modifiers.Add(stateful.CreateInstance());
                }
                else
                {
                    modifiers.Add(mod);
                }
            }

            // 모든 modifier 초기화
            foreach (var mod in modifiers)
            {
                mod.OnInitialize(this);
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
                modifier.OnHit(this, target, targetLayer);
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
