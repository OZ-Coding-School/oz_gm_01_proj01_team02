using UnityEngine;
using STH.Core;
using System.Collections;
using STH.Combat.Projectiles;
using STH.ScriptableObjects.Base;
using UnityEngine.AI;


namespace STH.Characters.Enemy
{
    /// <summary>
    /// 적 컨트롤러 - SO에서 전략 하나를 받아와서 사용
    /// </summary>
    public class EnemyController : MonoBehaviour, IDamageable
    {
        private EnemyState currentState;

        [Header("�� Ÿ��")]
        [SerializeField] private TypeEnums type;  //�ٰŸ�, ���Ÿ� �� ���� Ÿ��

        [Header("�̵�/ȸ��")]
        [SerializeField] private float moveSpeed = 3.0f;    //Enemy �̵��ӵ�
        [SerializeField] private float rotateSpeed = 8.0f;
        [SerializeField] private float maxHp = 450;
        private float currentHp = 450;


        [Header("�ٰŸ� ���� �Ÿ�/��Ÿ��/���� �ӵ�")]
        [SerializeField] private float meleeIdleRange = 1.0f;   //Ÿ���� ��ó�� ���� ���ߴ� �Ÿ�
        [SerializeField] private float meleeAttackRange = 5.0f; //�ٰŸ� ���ݰŸ�
        [SerializeField] private float attackDelay = 10.0f;      //�ٰŸ� ���� ��Ÿ��
        [SerializeField] private float attackChargeTime = 3.0f;
        [SerializeField] private float runSpeed = 10.0f;

        [Header("���Ÿ� ���� �Ÿ�/��Ÿ��")]
        [SerializeField] private float rangedAttackRange = 12.0f; //���Ÿ� ���ݰŸ�
        [SerializeField] private float spawnTime = 1.0f;  //���Ÿ� ���� ��Ÿ��
        private float rangedTimer = 0.0f; //��Ÿ�� ���� �帣�� �ð�

        [Header("Ÿ��")]
        [SerializeField] private Transform target;  //Ÿ��(�÷��̾�)
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private float damage = 200;

        [Header("bullet")]
        [SerializeField] private Bullet enemyBullet;
        [SerializeField] protected Transform bulletPos;

        private Rigidbody rb;
        private NavMeshAgent nvAgent;
        private Animator animator;

        private float nextAttackTime = 0.0f; //���� ���� �ð� ��Ÿ�� �����
        private bool isDead;
        private bool isAttack; //����(����) ���� �� Ȯ��
        private bool isWall;   //�� �浹 Ȯ��

        private Vector3 beforeTargetPos;

        //���� ��ü��
        public EnemyState IdleState { get; private set; }
        public EnemyState ChaseState { get; private set; }
        public EnemyState AttackState { get; private set; }

        //���� ���°� � ��������
        public StateEnums CurrentStateType { get; private set; }

        //�ܺο��� ���� �� �ֵ��� �ϴ� ������Ƽ
        public float MeleeIdleRange => meleeIdleRange;
        public float MeleeAttackRange => meleeAttackRange;
        public float RangedAttackRange => rangedAttackRange;
        public float NextAttackTime => nextAttackTime;
        public Transform Target => target;
        public TypeEnums Type => type;
        public bool IsAttack => isAttack;

        public Vector3 BeforeTargetPos
        {
            get { return beforeTargetPos; }
            set { beforeTargetPos = value; }
        }

        Vector3 startPos;

        private void OnEnable()
        {
            startPos = transform.position;
        }

        private void Awake()
        {
            rb = GetComponent<Rigidbody>();
            nvAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();

            if (target == null)
            {
                GameObject go = GameObject.FindGameObjectWithTag("Player");
                if (go != null) target = go.transform;
            }

            nvAgent.speed = moveSpeed;

            //IdleState = new IdleState(this);
            //ChaseState = new ChaseState(this);
            //AttackState = new AttackState(this);

            EnemyType();

            ChangeState(ChaseState);
        }
        private void Start()
        {
            if (type == TypeEnums.Ranged)
            {
                GameManager.Pool.CreatePool(enemyBullet, 50);
            }
        }

        void Update()
        {
            if (isDead) return;

            //���� ���¿� �´� UpdateState�� ����
            currentState.UpdateState();
        }
        private void FixedUpdate()
        {
            if (isDead) return;

            if (!nvAgent.enabled) return;
            currentState.FixedUpdateState();
        }
        //���� ���¸� �ٲ��ִ� �޼���
        public void ChangeState(EnemyState newState)
        {
            if (currentState == newState) return;

            currentState?.Exit();

            currentState = newState;

            CurrentStateType = newState.StateType;

            currentState.Enter();
        }
        //Enemy Ÿ�� ����
        public void EnemyType()
        {
            switch (type)
            {
                case TypeEnums.Melee:
                    IdleState = new MeleeIdle(this);
                    ChaseState = new MeleeChase(this);
                    AttackState = new MeleeAttack(this);
                    break;
                case TypeEnums.Ranged:
                    ChaseState = new RangedChase(this);
                    AttackState = new RangedAttack(this);
                    break;
            }

        }
        //�÷��̾������ �Ÿ�
        public float DistToPlayer()
        {
            if (target == null) return Mathf.Infinity;

            return Vector3.Distance(transform.position, target.position);
        }
        public void Idle()
        {
            nvAgent.enabled = false;
            nvAgent.velocity = Vector3.zero;

            nvAgent.enabled = true;
            nvAgent.Warp(transform.position);
        }
        //�÷��̾� ����
        public void Chase()
        {
            if (!nvAgent.enabled) return;

            //nvAgent.speed = moveSpeed;
            if (!isAttack || nvAgent.enabled == true)
            {
                nvAgent.speed = moveSpeed;
                nvAgent.SetDestination(target.transform.position);
            }
        }
        public void MeleeAttack()
        {
            if (Time.time < nextAttackTime) return;

            isAttack = true;

            nvAgent.enabled = false; //NebMesh�� �⺻������ �� �����Ӹ��� Ÿ���� ��ġ�� ������Ʈ�ϱ� ������, Enemy�� ���߰� Ÿ���� ������ġ�� �����Ͽ� �Ŀ� �װ����� �̵��ҷ��� .enable = false�� �ؾ��Ѵ�.

            StartCoroutine(AttackCharge());

            //isAttack = false;

            nextAttackTime = Time.time + attackDelay;


        }
        public void RangedAttack()
        {
            nvAgent.enabled = false;
            // nvAgent.velocity = Vector3.zero;
            // rb.velocity = Vector3.zero;

            rangedTimer += Time.deltaTime;

            //���� ��Ÿ���� �Ǹ� ������ �߻�
            if (rangedTimer >= spawnTime)
            {
                Bullet enemyBullet = GameManager.Pool.GetFromPool(this.enemyBullet);
                enemyBullet.transform.SetLocalPositionAndRotation(bulletPos.position, bulletPos.rotation);
                enemyBullet.Initialize(damage);

                rangedTimer = 0.0f; //Ÿ�̸� �ʱ�ȭ
                animator.SetTrigger("DoAttack");
            }

            nvAgent.enabled = true;
            nvAgent.Warp(transform.position);
        }
        //���� ȸ��
        public void LookTo()
        {
            Vector3 dir = (target.position - transform.position).normalized;

            Quaternion targetRot = Quaternion.LookRotation(dir);

            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
        }
        IEnumerator AttackCharge()
        {
            //Debug.Log("���ݽ���");
            yield return new WaitForSeconds(attackChargeTime); //���� �����ϸ� ������ �����ϱ������ ����Ÿ��

            //Debug.Log("�߻�!!!!!!!!");

            nvAgent.velocity = Vector3.zero; //NavMeshAgent�� ���� ���� �ܷ� �ӵ� �ʱ�ȭ
            rb.velocity = Vector3.zero; //Rigidbody ���� �ӵ��� �ʱ�ȭ �Ͽ� ���� ���� ����

            float timer = 0.0f;         //�����ϰ� �ִ� �ð�(Ÿ�̸�)
            float attackRunTime = 0.5f; //���� �� �ð�
            rb.isKinematic = false;     //Kinematic�� true�̸� �̲����� �����ǳ� ���� ���� �׷��� �Ͻ������� false�� ��
            while (timer < attackRunTime)
            {
                rb.MovePosition(rb.position + transform.forward * runSpeed * Time.fixedDeltaTime);
                timer += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();

                if (isWall)
                {
                    Debug.Log("���̴�");
                    break;
                }
            }

            isWall = false;

            rb.isKinematic = true;
            isAttack = false;
            nvAgent.enabled = true;
            nvAgent.Warp(transform.position);
            //nvAgent.enabled = fals���� NavMeshAgent�� Rigidbody�� �̵���Ų ��ġ�� ������ ����
            //�׷��� �׷��� ��߳��� �ذ��ϱ� ���� .Warp�� Rigidbody�� �̵���Ų ��ġ�� �̵�

        }
        private void OnDrawGizmos()
        {
            if (type == TypeEnums.Melee)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, meleeAttackRange);

                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(transform.position, meleeIdleRange);
            }

            if (type == TypeEnums.Ranged)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawWireSphere(transform.position, rangedAttackRange);
            }
        }
        private void OnCollisionEnter(Collision collision)
        {
            if (collision.gameObject.CompareTag("Wall"))
            {
                isWall = true;
            }
        }

        public void ReturnPool()
        {
            if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
        }

        IEnumerator DieCo()
        {
            yield return new WaitForSeconds(3);
            transform.position = startPos;
            nvAgent.enabled = false;
            ReturnPool();
        }

        public void TakeDamage(float amount)
        {
            if (currentHp <= 0) return;

            Debug.Log($"Enemy take damage {amount}");
            currentHp -= amount;

            if (currentHp <= 0)
            {
                currentHp = 0;
                Die();
            }
        }

        public void Die()
        {
            isDead = true;
            animator.SetTrigger("Die");
        }
    }


}
