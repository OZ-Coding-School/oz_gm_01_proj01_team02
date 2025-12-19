using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private EnemyState currentState;

    [Header("적 타입")]
    [SerializeField] private TypeEnums type;  //근거리, 원거리 등 적의 타입

    [Header("이동/회전")]
    [SerializeField] private float moveSpeed = 3.0f;    //Enemy 이동속도
    [SerializeField] private float rotateSpeed = 8.0f;

    [Header("근거리 공격 거리/쿨타임/돌진 속도")]
    [SerializeField] private float meleeIdleRange = 1.0f;   //타겟의 근처에 가면 멈추는 거리
    [SerializeField] private float meleeAttackRange = 5.0f; //근거리 공격거리
    [SerializeField] private float attackDelay = 10.0f;      //근거리 공격 쿨타임
    [SerializeField] private float attackChargeTime = 3.0f;
    [SerializeField] private float runSpeed = 10.0f;

    [Header("원거리 공격 거리/쿨타임")]
    [SerializeField] private float rangedAttackRange = 12.0f; //원거리 공격거리
    [SerializeField] private float spawnTime = 1.0f;  //원거리 공격 쿨타임
    private float rangedTimer = 0.0f; //쿨타임 까지 흐르는 시간

    [Header("타겟")]
    [SerializeField] private Transform target;  //타겟(플레이어)
    [SerializeField] private LayerMask playerLayer;

    [Header("원거리 공격 프리팹/위치")]
    [SerializeField] private TextEnemyBullet enemyBullet;
    [SerializeField] protected Transform bulletPos;

    private Rigidbody rb;
    private NavMeshAgent nvAgent;

    private float nextAttackTime = 0.0f; //다음 공격 시간 쿨타임 저장용
    private bool isDead;
    private bool isAttack; //공격(돌진) 중일 때 확인
    private bool isWall;   //벽 충돌 확인

    private Vector3 beforeTargetPos;

    //상태 객체들
    public EnemyState IdleState { get; private set; }
    public EnemyState ChaseState { get; private set; }
    public EnemyState AttackState { get; private set; }

    //현재 상태가 어떤 종류인지
    public StateEnums CurrentStateType { get; private set; }

    //외부에서 읽을 수 있도록 하는 프로퍼티
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
        StartCoroutine(Die());
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        nvAgent = GetComponent<NavMeshAgent>();

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
    public void ChangeState(EnemyState newState)
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
        switch(type)
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
    //플레이어까지의 거리
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
    public void MeleeAttack()
    {
       if (Time.time < nextAttackTime) return;
        
        isAttack = true;

        nvAgent.enabled = false; //NebMesh는 기본적으로 매 프레임마다 타겟의 위치를 업데이트하기 때문에, Enemy를 멈추고 타겟의 이전위치를 저장하여 후에 그곳으로 이동할려면 .enable = false로 해야한다.
       
        StartCoroutine(AttackCharge());

        //isAttack = false;

        nextAttackTime = Time.time + attackDelay;
        

    }
    public void RangedAttack()
    {
        nvAgent.enabled = false;
        nvAgent.velocity = Vector3.zero; 
        rb.velocity = Vector3.zero; 

        rangedTimer += Time.deltaTime;

        //공격 쿨타임이 되면 프리팹 발사
        if (rangedTimer >= spawnTime)
        {
            TextEnemyBullet enemyBullet = GameManager.Pool.GetFromPool(this.enemyBullet);
            enemyBullet.transform.SetLocalPositionAndRotation(bulletPos.position, Quaternion.identity);
            enemyBullet.transform.forward = transform.forward;

            rangedTimer = 0.0f; //타이머 초기화
        }

        nvAgent.enabled = true;
        nvAgent.Warp(transform.position);
    }
    //방향 회전
    public void LookTo()
    {
        Vector3 dir = (target.position - transform.position).normalized;

        Quaternion targetRot = Quaternion.LookRotation(dir);

        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, rotateSpeed * Time.deltaTime);
    }
    IEnumerator AttackCharge()
    {
        //Debug.Log("공격시작");
        yield return new WaitForSeconds(attackChargeTime); //공격 시작하면 실제로 돌진하기까지의 차지타임

        //Debug.Log("발사!!!!!!!!");

        nvAgent.velocity = Vector3.zero; //NavMeshAgent를 꺼도 남는 잔류 속도 초기화
        rb.velocity = Vector3.zero; //Rigidbody 기존 속도를 초기화 하여 돌진 직선 유지

        float timer = 0.0f;         //돌진하고 있는 시간(타이머)
        float attackRunTime = 0.5f; //돌진 총 시간
        rb.isKinematic = false;     //Kinematic이 true이면 미끄럼은 방지되나 벽을 뚫음 그래서 일시적으로 false로 함
        while (timer < attackRunTime)
        {
            rb.MovePosition(rb.position + transform.forward * runSpeed * Time.fixedDeltaTime);
            timer += Time.fixedDeltaTime;
            yield return new WaitForFixedUpdate();

            if (isWall)
            {
                Debug.Log("벽이당");
                break;
            }
        }

        isWall = false;

        rb.isKinematic = true;
        isAttack = false;
        nvAgent.enabled = true;
        nvAgent.Warp(transform.position);
        //nvAgent.enabled = fals에서 NavMeshAgent는 Rigidbody로 이동시킨 위치를 따라가지 못함
        //그래서 그러한 어긋남을 해결하기 위해 .Warp로 Rigidbody로 이동시킨 위치로 이동

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

    IEnumerator Die()
    {
        yield return new WaitForSeconds(3);
        transform.position = startPos;
        nvAgent.enabled = false;
        ReturnPool();
    }

}
