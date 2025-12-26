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


        [Header("Test")]
        [SerializeField] private List<SkillData> testSkills;

        private List<SkillData> skills = new List<SkillData>();
        private List<IFireStrategy> strategies = new List<IFireStrategy>();
        private List<IBulletModifier> modifiers = new List<IBulletModifier>();

        private PlayerShoot playerShoot;

        private float attackTimer;
        private bool isDead;

        public List<SkillData> Skills => skills;
        public List<IFireStrategy> Strategies => strategies;
        public List<IBulletModifier> Modifiers => modifiers;
        public PlayerStatManager Stats => stats;

        private void Awake()
        {
            playerShoot = GetComponent<PlayerShoot>();
        }


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
            // attackTimer += Time.deltaTime;
            // if (attackTimer >= 1 / stats.attackSpeed)
            // {
            //     Attack();
            //     attackTimer = 0;
            // }

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

        public void TakeDamage(float amount, bool isCritical = false)
        {
            Debug.Log($"Player Take Damage: {amount}, Critical: {isCritical}");
        }

        public void Die()
        {

        }
    }
}
