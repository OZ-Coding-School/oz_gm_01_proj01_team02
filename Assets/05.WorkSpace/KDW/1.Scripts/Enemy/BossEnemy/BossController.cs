using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BossController : MonoBehaviour
{
    private BossState currentState;

    [Header("자신의 타입,위치")]
    [SerializeField] private TypeEnums type;         //보스 몬스터 타입
    [SerializeField] private Transform target;       //타겟(플레이어)

    [Header("타겟의 콜라이더,레이어/벽 레이어,감지 길이")]
    [SerializeField] private Collider targetCollider;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private LayerMask wallLayer;
    [SerializeField] private float walllRange;      //벽 감지용 선 길이

    [Header("이동/회전")]
    [SerializeField] private float moveSpeed = 3.0f;    //Enemy 이동속도
    [SerializeField] private float rotateSpeed = 8.0f;

    [Header("타겟에 가까이 가면 멈추는거리")]
    [SerializeField] private float idleRange = 1.0f;      //타겟의 근처에 가면 멈추는 거리

    [Header("대쉬 보스 공격 거리/쿨타임/돌진 속도,시간")]
    [SerializeField] private float dashAttackRange = 5.0f;    //근거리 공격거리
    [SerializeField] private float attackDelay = 10.0f;      //공격 패턴에 들어가는 시간
    [SerializeField] private float attackChargeTime = 3.0f;  //공격 패턴에 들어오면 돌진하기 까지의 차지 시간
    [SerializeField] private float runSpeed = 10.0f;         //돌진 속도
    [SerializeField] private float runTime = 0.5f;           //돌진 시간

    [Header("원거리 공격 거리/쿨타임/발사 수")]
    [SerializeField] private float rangedAttackRange = 12.0f; //원거리 공격거리
    [SerializeField] private float spawnTime = 1.0f;    //원거리 공격 쿨타임
    [SerializeField] private int bulletCount = 5;       //한번에 발사할 수
    [SerializeField] private float spreadAngle = 45.0f; //발사 시 프리팹과의 총각도
    private float rangedTimer = 0.0f; //쿨타임 까지 흐르는 시간

    [Header("원거리 공격 프리팹/위치")]
    [SerializeField] private DandelionBullet dandelionBullet;
    [SerializeField] protected Transform bulletPos;

    private Rigidbody rb;
    private NavMeshAgent nvAgent;
    private Collider inCollider; 

    private float nextAttackTime = 0.0f; //다음 공격 시간 쿨타임 저장용
    private bool isDead;
    private bool isAttack; //공격(돌진) 중일 때 확인
    private bool isWall;   //벽 충돌 확인

    private Vector3 beforeTargetPos;

    //상태 객체들
    public BossState IdleState { get; private set; }
    public BossState ChaseState { get; private set; }
    public BossState AttackState { get; private set; }

    //현재 상태가 어떤 종류인지
    public StateEnums CurrentStateType { get; private set; }

    //외부에서 읽을 수 있도록 하는 프로퍼티
    public float IdleRange => idleRange;
    public float DashAttackRange => dashAttackRange;
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
        inCollider = GetComponent<Collider>();

        if (target == null)
        {
            GameObject go = GameObject.FindGameObjectWithTag("Player");
            if (go != null) target = go.transform;
        }

        nvAgent.speed = moveSpeed;

        EnemyType();

        ChangeState(ChaseState);
    }
    private void Start()
    {
        if (type == TypeEnums.DandelionBoss)
        {
            GameManager.Pool.CreatePool(dandelionBullet, 50);
        }

        rb.freezeRotation = true;
    }

    void Update()
    {
        if (isDead) return;

        //현재 상태에 맞는 UpdateState를 실행
        currentState.UpdateState();
    }
    private void FixedUpdate()
    {
        if (isDead) return;

        if (!nvAgent.enabled) return;
        currentState.FixedUpdateState();
    }
    //현재 상태를 바꿔주는 메서드
    public void ChangeState(BossState newState)
    {
        if (currentState == newState) return;

        currentState?.Exit();

        currentState = newState;

        CurrentStateType = newState.StateType;

        currentState.Enter();
    }
    //Enemy 타입 결정
    public void EnemyType()
    {
        switch (type)
        {
            case TypeEnums.DashBoss:
                IdleState = new DashBossIdle(this);
                ChaseState = new DashBossChase(this);
                AttackState = new DashBossAttack(this);
                break;
            case TypeEnums.DandelionBoss:
                ChaseState = new DandelionBossChase(this);
                AttackState = new DandelionBossAttack(this);
                break;
        }

    }
    //플레이어까지의 거리
    public float DistToPlayer()
    {
        if (target == null) return Mathf.Infinity;

        //타겟의 표면 중 나와 가장 가까운 지점 찾기
        Vector3 closestOnTarget = targetCollider.ClosestPoint(inCollider.bounds.center);

        //나의 표면 중 타겟과 가장 가까운 지점 찾기
        Vector3 closestOnBoss = inCollider.ClosestPoint(closestOnTarget);

        return Vector3.Distance(closestOnBoss, closestOnTarget);
    }
    //방향 회전
    public void LookTo()
    {
        Vector3 dir = (target.position - transform.position).normalized;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
    }
    public void Idle()
    {
        nvAgent.enabled = false;
        nvAgent.velocity = Vector3.zero;

        nvAgent.enabled = true;
        nvAgent.Warp(transform.position);
    }
    //플레이어 추적
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
    //대쉬 보스 공격
    public void DashAttack()
    {
        if (Time.time < nextAttackTime) return;

        isAttack = true;

        nvAgent.enabled = false; //NebMesh는 기본적으로 매 프레임마다 타겟의 위치를 업데이트하기 때문에, Enemy를 멈추고 타겟의 이전위치를 저장하여 후에 그곳으로 이동할려면 .enable = false로 해야한다.

        StartCoroutine(DashAttackCharge());

        //isAttack = false;

        nextAttackTime = Time.time + attackDelay;


    }
    public void DandelionShotAttack()
    {
        nvAgent.enabled = false;
        nvAgent.velocity = Vector3.zero;
        rb.velocity = Vector3.zero;

        float angleStep = spreadAngle / (bulletCount - 1); //총 간격/bulletCount만큼 생기는 간격 개수 = 총알 사이 간격
        float startAngle = -spreadAngle / 2;

        rangedTimer += Time.deltaTime;

        if (bulletCount <= 1)
        {
            angleStep = 0f;
            startAngle = 0f;  
        }

        //공격 쿨타임이 되면 프리팹 발사
        if (rangedTimer >= spawnTime)
        {
            //프리팹 여러개 동시 발사
            for(int i = 0; i < bulletCount; i++)
            {
                DandelionBullet dandelionBullet = GameManager.Pool.GetFromPool(this.dandelionBullet);

                float currentAngle = startAngle + (angleStep * i); //현재 프리팹 간격(순서대로 간격 계산)

                Quaternion rotation = transform.rotation * Quaternion.Euler(0, currentAngle, 0);


                dandelionBullet.transform.SetLocalPositionAndRotation(bulletPos.position, rotation);
                //dandelionBullet.transform.forward = transform.forward;

                Debug.Log(i);
            }
            rangedTimer = 0.0f; //타이머 초기화
        }

        nvAgent.enabled = true;
        nvAgent.Warp(transform.position);
    }
    //대쉬 보스 공격의 삼단대쉬
    IEnumerator DashAttackCharge()
    {
        //Debug.Log("공격시작");
        yield return new WaitForSeconds(attackChargeTime); //공격 시작하면 실제로 돌진하기까지의 차지타임

        nvAgent.velocity = Vector3.zero; //NavMeshAgent를 꺼도 남는 잔류 속도 초기화
        rb.velocity = Vector3.zero; //Rigidbody 기존 속도를 초기화 하여 돌진 직선 유지

        float timer = 0.0f;         //돌진하고 있는 시간(타이머)
        rb.isKinematic = false;     //Kinematic이 true이면 미끄럼은 방지되나 벽을 뚫음 그래서 일시적으로 false로 함

        for (int i = 0; i < 3; i++)
        {
            Debug.Log("발사!!!!!!!!");
            Debug.Log("다음 대쉬 : " + i);

            timer = 0.0f;

            while (timer < runTime)
            {
                rb.MovePosition(rb.position + transform.forward * runSpeed * Time.fixedDeltaTime);
                timer += Time.fixedDeltaTime;

                Collider[] hits = Physics.OverlapSphere(transform.position, walllRange, wallLayer);
                bool wall = hits.Length > 0;  //벽 감지

                //벽 연속 감지 오류로 공격패턴이 끝나는 것을 방지
                if (timer < 0.2f)
                {
                    yield return new WaitForFixedUpdate();
                    continue;
                }

                if (wall || DistToPlayer() <= IdleRange)
                {
                   //rb.velocity = Vector3.zero;
                   //rb.angularVelocity = Vector3.zero;
                   //rb.drag = 0;
                   //rb.angularDrag = 0;
                    break;
                }

                yield return new WaitForFixedUpdate();
            }

            Vector3 dashDir = (target.position - transform.position).normalized;
            dashDir.y = 0;

            Quaternion lookRotation = Quaternion.LookRotation(dashDir);
            transform.rotation = lookRotation;
        }
        rb.isKinematic = true;
        isAttack = false;
        nvAgent.enabled = true;
        nvAgent.Warp(transform.position);
        //nvAgent.enabled = fals에서 NavMeshAgent는 Rigidbody로 이동시킨 위치를 따라가지 못함
        //그래서 그러한 어긋남을 해결하기 위해 .Warp로 Rigidbody로 이동시킨 위치로 이동

    }
    private void OnDrawGizmos()
    {
        if (type == TypeEnums.DashBoss)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, dashAttackRange);

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(transform.position, idleRange);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, walllRange);
        }

        if (type == TypeEnums.DandelionBoss)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, rangedAttackRange);
        }
    }
    public void ReturnPool()
    {
        if (PoolManager.pool_instance != null) PoolManager.pool_instance.ReturnPool(this);
    }
}
