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
        [SerializeField] private CharacterStats stats = new CharacterStats();

        [Header("Combat")]
        [SerializeField] private GameObject bulletPrefab;
        [SerializeField] private Transform firePoint;

        [Header("Test")]
        [SerializeField] private List<SkillData> testSkills;

        private List<SkillData> skills = new List<SkillData>();
        private List<IFireStrategy> strategies = new List<IFireStrategy>();
        private List<IBulletModifier> modifiers = new List<IBulletModifier>();

        private float attackTimer;
        private bool isDead;

        public CharacterStats Stats => stats;
        public List<SkillData> Skills => skills;


        void Start()
        {
            foreach (var skill in testSkills)
            {
                // 테스트용
                skill.Apply(this);
            }
            // Attack();
        }


        private void Update()
        {
            if (isDead) return;

            attackTimer += Time.deltaTime;
            if (attackTimer >= 1 / stats.GetStat(STH.Core.Stats.StatType.AttackSpeed))
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
