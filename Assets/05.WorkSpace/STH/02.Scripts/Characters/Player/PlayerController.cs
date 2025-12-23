 using System.Collections.Generic;
using UnityEngine;
using STH.Core;
using STH.Core.Stats;
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
        [SerializeField] private PlayerStatManager stats;

        [Header("Combat")]
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform firePoint;

        [Header("Test")]
        [SerializeField] private List<SkillData> testSkills;

        private List<SkillData> skills = new List<SkillData>();
        public List<IFireStrategy> strategies = new List<IFireStrategy>();
        private List<IBulletModifier> modifiers = new List<IBulletModifier>();

        private PlayerEnemySearch enemySearch;
        private PlayerMove player;
        private Rigidbody rb;

        private float attackTimer;
        private bool isDead;

        public PlayerStatManager Stats => stats;
        public List<SkillData> Skills => skills;

        private void Awake()
        {
            enemySearch = GetComponent<PlayerEnemySearch>();
            player = GetComponent<PlayerMove>();

            rb = GetComponent<Rigidbody>();
        }


        void Start()
        {

             if (bulletPrefab == null)
    {
        Debug.LogError("❌ bulletPrefab이 Inspector에 안 들어가 있음");
    }
    else
    {
        Debug.Log($"✅ CreatePool prefab name: {bulletPrefab.name}");
    }
            GameManager.Pool.CreatePool(bulletPrefab, 50);

            foreach (var skill in testSkills)
            {
                // 테스트용
                skill.Apply(this);
            }
            // Attack();
            // SpawnBulletCallback(firePoint.position, firePoint.rotation);
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
            Debug.Log($"strategies.Count = {strategies.Count}");
            // 적이 없거나 움직이면 공격 안 함
            if (enemySearch.CloseEnemy != null && rb.velocity.sqrMagnitude < 10f)
            {
                Debug.Log(rb.velocity.sqrMagnitude);

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
            Debug.Log("🔥 SpawnBulletCallback 호출됨");
            Debug.Log($"bullet active = {bulletPrefab.gameObject.activeSelf}");
            Debug.Log($"Bullet World Pos: {bulletPrefab.transform.position}");
            Debug.Log($"Bullet Parent: {bulletPrefab.transform.parent?.name}");
            Debug.Log($"Bullet Scale: {bulletPrefab.transform.localScale}");

            // TODO 생성하지말고 pool에서 꺼내기
            Bullet bullet = GameManager.Pool.GetFromPool(bulletPrefab);

            if (bullet == null)
            {
                // Debug.LogError("❌ bullet == null (풀에서 못 꺼냄)");
                return;
            }   

            if (bullet != null)
            {
                bullet.transform.SetLocalPositionAndRotation(position, rotation);
                bullet.Initialize(stats, modifiers);
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

        public void AddSkill(SkillData newSkill)
        {
            skills.Add(newSkill);
        }

        public void TakeDamage(float amount)
        {

        }

        public void Die()
        {

        }
    }
}
