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
    public class PlayerController : MonoBehaviour
    {
        [Header("Stats")]
        [SerializeField] private PlayerStatManager stats;


        [Header("Test")]
        [SerializeField] private List<SkillData> testSkills;

        private List<SkillData> skills = new List<SkillData>();
        private List<IFireStrategy> strategies = new List<IFireStrategy>();
        private List<IBulletModifier> modifiers = new List<IBulletModifier>();

        private PlayerHealth playerHealth;
        private Animator animator;

        private float attackTimer;
        public bool IsDead;

        public List<SkillData> Skills => skills;
        public List<IFireStrategy> Strategies => strategies;
        public List<IBulletModifier> Modifiers => modifiers;
        public PlayerStatManager Stats => stats;
        public Animator Animator => animator;

        private void Awake()
        {
            playerHealth = GetComponent<PlayerHealth>();
            animator = GetComponent<Animator>();

            if (stats == null)
                stats = FindObjectOfType<PlayerStatManager>();
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
            if (IsDead) return;
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


    }
}
