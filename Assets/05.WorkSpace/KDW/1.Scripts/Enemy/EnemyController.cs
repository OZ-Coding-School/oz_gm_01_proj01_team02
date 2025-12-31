using STH.Combat.Projectiles;
using STH.Core;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;
using STH.ScriptableObjects.Base;
using STH.Characters.Player;
using System.Collections.Generic;

public class EnemyController : MonoBehaviour, IDamageable
{
    private EnemyState currentState;

    [Header("�� Ÿ��")]
    [SerializeField] private TypeEnums type;  //�ٰŸ�, ���Ÿ� �� ���� Ÿ��

    [Header("�̵�/ȸ��")]
    [SerializeField] private float moveSpeed = 3.0f;    //Enemy �̵��ӵ�
    [SerializeField] private float rotateSpeed = 8.0f;
    public float maxHp = 450;
    public float currentHp;


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
    [SerializeField] private PlayerController target;  //Ÿ(÷̾)
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float damage = 200;
    private Vector3 targetDir;

    [Header("bullet")]
    [SerializeField] private Bullet enemyBullet;
    [SerializeField] protected Transform bulletPos;
    [SerializeField] private List<AttackPatternSO> bulletPatterns;
    private List<IFireStrategy> fireStrategies = new List<IFireStrategy>();
    private Bullet patternBulletPrefab;

    [SerializeField] private PoolableParticle deathEffect;

    private Rigidbody rb;
    private NavMeshAgent nvAgent;
    private Animator animator;
    private Collider col;
    private HitEffect hitEffect;
    private Boss bossComponent;

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
    public PlayerController Target => target;
    public TypeEnums Type => type;
    public bool IsAttack => isAttack;
    public Animator Animator => animator;
    public bool IsDead => isDead;

    private static readonly int attackHash = Animator.StringToHash("DoAttack");
    private static readonly int getHitHash = Animator.StringToHash("GetHit");
    private static readonly int isMovingHash = Animator.StringToHash("IsMoving");
    private AnimatorStateInfo stateInfo;

    [SerializeField] private DmgText dmgTextPrefab;
    [SerializeField] private Transform dmgTextPosition;
    [SerializeField] private RectTransform damageCanvas;

    public Vector3 BeforeTargetPos
    {
        get { return beforeTargetPos; }
        set { beforeTargetPos = value; }
    }

    Vector3 startPos;

    private void OnEnable()
    {
        startPos = transform.position;
        isDead = false;
        currentHp = maxHp;
        isAttack = false;
        isWall = false;
        rangedTimer = 0.0f;
        nvAgent.enabled = true;

        if (target == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) target = go.transform.GetComponent<PlayerController>();

            if (target == null)
            {
                Debug.LogError("EnemyController: Target (Player) not found!");
            }
        }

        if (rb != null)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
            rb.isKinematic = true;
        }

        col = GetComponent<Collider>();
        if (col != null)
        {
            col.enabled = true;
        }
        if (ChaseState != null) ChangeState(ChaseState);

        bossComponent = GetComponent<Boss>();

        Debug.Log($"bossComponent {bossComponent}");
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nvAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        hitEffect = GetComponent<HitEffect>();



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
        GameManager.Pool.CreatePool(deathEffect, 20);
        currentHp = maxHp;

        bulletPatterns.ForEach(pattern =>
        {
            IFireStrategy strategy = pattern.CreateStrategy();
            fireStrategies.Add(strategy);
            GameManager.Pool.CreatePool(pattern.BulletPrefab, 50);
        });
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

        // move animation
        if (newState == ChaseState && target != null)
        {
            animator.SetBool(isMovingHash, true);
        }
        else if (newState == IdleState || newState == AttackState)
        {
            animator.SetBool(isMovingHash, false);
        }

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

        return Vector3.Distance(transform.position, target.transform.position);
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

        if (bossComponent != null && bossComponent.CanSpecialAttack && bossComponent.IsSpecialAttackReady)
        {
            animator.ResetTrigger(getHitHash);
            animator.SetTrigger(attackHash);
            bossComponent.SpecialAttack();
        }

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
        if (rangedTimer - 0.5f >= spawnTime)
        {
            animator.SetBool("IsAttacking", true);
        }

        if (rangedTimer >= spawnTime)
        {
            if (bossComponent != null && bossComponent.CanSpecialAttack && bossComponent.IsSpecialAttackReady)
            {
                bossComponent.SpecialAttack();
            }
            else
            {
                if (fireStrategies.Count == 0)
                {
                    patternBulletPrefab = enemyBullet;
                    // 기본 공격
                    SpawnBulletCallback(bulletPos.position, bulletPos.rotation);
                }
                else
                {
                    for (int i = 0; i < fireStrategies.Count; i++)
                    {
                        patternBulletPrefab = bulletPatterns[i].BulletPrefab;
                        fireStrategies[i].Fire(bulletPos, SpawnBulletCallback);
                    }
                }
            }


            rangedTimer = 0.0f;

            animator.ResetTrigger(getHitHash);
            animator.SetTrigger(attackHash);
        }

        nvAgent.enabled = true;
        nvAgent.Warp(transform.position);
    }

    private void SpawnBulletCallback(Vector3 position, Quaternion rotation)
    {
        Bullet bullet = GameManager.Pool.GetFromPool(patternBulletPrefab);
        if (bullet != null)
        {
            bullet.transform.SetLocalPositionAndRotation(position, rotation);
            bullet.Initialize(damage);
        }
    }

    //���� ȸ��
    public void LookTo()
    {
        targetDir = (target.transform.position - transform.position).normalized;

        Quaternion targetRot = Quaternion.LookRotation(targetDir);

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

            if (isWall || isDead)
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.root.CompareTag("Player"))
        {
            IDamageable player = other.GetComponent<IDamageable>();
            if (player != null)
            {
                player.TakeDamage(damage, false);
            }
        }
    }

    public void ReturnPool()
    {
        bool hasBoss = gameObject.TryGetComponent<Boss>(out _);
        if (hasBoss)
        {
            if (GameManager.Stage.currentStage >= GameManager.Stage.Select("finish"))
            {
                GameManager.ClearChapter();
            }
        }
        GameManager.Pool.ReturnPool(this);
    }

    IEnumerator DieCo()
    {
        yield return new WaitForSeconds(1);
        // 사라지는 이펙트
        if (deathEffect != null)
        {
            PoolableParticle ga = GameManager.Pool.GetFromPool(deathEffect);
            if (ga != null) ga.transform.position = transform.position;
        }

        ReturnPool();
    }

    public void TakeDamage(float amount, bool isCritical = false)
    {
        if (currentHp <= 0) return;

        Debug.Log($"Enemy take damage {amount}");
        currentHp -= amount;

        if (hitEffect != null) hitEffect.PlayHitEffect(isCritical);

        if (!animator.GetBool("IsAttacking"))
        {
            animator.SetTrigger(getHitHash);
            Debug.Log("trigger hit reaction");
        }

        // Boss HP 70% can use special attack
        if (bossComponent != null)
        {


            if (currentHp / maxHp <= 0.7f && !bossComponent.CanSpecialAttack)
            {
                // Debug.Log("Boss can use special attack now");
                animator.SetTrigger("ChangePhase");

                // reset attack timer
                nextAttackTime = Time.time + attackDelay;
                rangedTimer = 0.0f;

                animator.ResetTrigger(getHitHash);
                animator.ResetTrigger(attackHash);

                bossComponent.CanSpecialAttack = true;
            }
        }

        if (dmgTextPrefab != null)
        {
            DmgText dmgText = PoolManager.pool_instance.GetFromPool(dmgTextPrefab);
            
            if (dmgText != null)
            {
                dmgText.canvasTransform = damageCanvas;
                
                Vector3 headPos = transform.position + Vector3.up * 2.0f;
                Vector3 screenPos = Camera.main.WorldToScreenPoint(headPos);

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                damageCanvas,
                screenPos,
                null, // Overlay 모드라면 Camera는 null
                out Vector2 localPos
                
            );


            Debug.Log($"localPos on Canvas: {localPos}");
                dmgText.transform.SetParent(damageCanvas, false);
                dmgText.GetComponent<RectTransform>().localPosition = localPos;
                dmgText.gameObject.SetActive(true);
                dmgText.Play(amount, localPos, isCritical);
                Debug.Log($"Enemy headPos: {headPos}");
            }
        }
        
        if (currentHp <= 0)
        {
            currentHp = 0;
            Die();
        }
    }

    public void Die()
    {
        // Debug.Log(this.name + " 사망");
        isDead = true;
        col.enabled = false;
        rb.velocity = Vector3.zero;
        nvAgent.enabled = false;
        animator.SetTrigger("Die");
        StartCoroutine(DieCo());

    }

    public void TargetLost()
    {
        target = null;
    }
}

