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
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform firePoint;

        [Header("Test")]
        [SerializeField] private List<SkillData> skills;

        private List<IFireStrategy> strategies = new List<IFireStrategy>();
        private List<IBulletModifier> modifiers = new List<IBulletModifier>();

        private PlayerEnemySearch enemySearch;
        private PlayerMove player;
        private Rigidbody rb;

        private float attackTimer;
        private bool isDead;

        public GameStats Stats => stats;

        private void Awake()
        {
            enemySearch = GetComponent<PlayerEnemySearch>();
            player = GetComponent<PlayerMove>();

            rb = GetComponent<Rigidbody>();
        }


        void Start()
        {
            GameManager.Pool.CreatePool(bulletPrefab, 50);

            foreach (var skill in skills)
            {
                // 테스트용
                skill.Apply(this);
            }
            Attack();
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


        // public void ShootBullet()
        // {
        //     if (enemySearch.CloseEnemy != null && rb.velocity.sqrMagnitude < 0.0001f)
        //     {
        //         //GameObject bullet = Instantiate(bulletPrefab, bulletPos.position, Quaternion.identity);
        //         TestBullet bullet = GameManager.Pool.GetFromPool(bulletPrefab);
        //         bullet.transform.SetLocalPositionAndRotation(bulletPos.position, Quaternion.identity);
        //         bullet.transform.forward = player.EnemyDir;
        //     }
        // }

        private void Attack()
        {
            // 적이 없거나 움직이면 공격 안 함
            if (enemySearch.CloseEnemy != null && rb.velocity.sqrMagnitude < 0.0001f)
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
        }

        private void SpawnBulletCallback(Vector3 position, Quaternion rotation)
        {
            // TODO 생성하지말고 pool에서 꺼내기
            Bullet bullet = GameManager.Pool.GetFromPool(bulletPrefab);
            bullet.transform.SetLocalPositionAndRotation(position, rotation);

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

        // 총알 능력 추가
        public void AddModifier(IBulletModifier newModifier)
        {
            modifiers.Add(newModifier);
        }

        public void TakeDamage(float amount)
        {

        }

        public void Die()
        {

        }
    }
}
