using System.Collections.Generic;
using UnityEngine;
using STH.Core;
using STH.Combat.Projectiles;
using STH.ScriptableObjects.Base;


namespace STH.Characters.Player
{
    /// <summary>
    /// 플레이어 컨트롤러 - 전략 리스트와 능력 리스트를 관리
    /// </summary>
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [Header("Stats")]
        [SerializeField] private GameStats stats = new GameStats();

        [Header("Combat")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;

        [Header("Test")]
        [SerializeField] private List<SkillData> patterns;

        private List<IFireStrategy> strategies = new List<IFireStrategy>();
        private List<IBulletModifier> modifiers = new List<IBulletModifier>();

        private float attackTimer;
        private bool isDead;

        public GameStats Stats => stats;


        void Start()
        {
            foreach (var pattern in patterns)
            {
                // 테스트용
                pattern.Apply(this);
            }
        }


        private void Update()
        {
            if (isDead) return;

            attackTimer += Time.deltaTime;
            if (attackTimer >= 1 / stats.attackSpeed)
            {
                Attack();
                attackTimer = 0;
            }

        }

        private void Attack()
        {
            if (strategies.Count == 0)
            {
                // 기본 단발 공격
                SpawnBulletCallback(firePoint.position, firePoint.rotation);
                return;
            }

            // 모든 전략 실행
            foreach (var strategy in strategies)
            {
                strategy.Fire(firePoint, SpawnBulletCallback);
            }
        }

        private void SpawnBulletCallback(Vector3 position, Quaternion rotation)
        {
            // TODO 생성하지말고 pool에서 꺼내기
            GameObject go = Instantiate(bulletPrefab, position, rotation);

            Bullet bullet = go.GetComponent<Bullet>();
            if (bullet != null)
            {
                bullet.Initialize(stats.damage, modifiers);
            }
        }

        // 패턴 추가
        public void AddStrategy(IFireStrategy newStrategy)
        {
            strategies.Add(newStrategy);
        }

        public void TakeDamage(float amount)
        {

        }

        public void Die()
        {

        }
    }
}
