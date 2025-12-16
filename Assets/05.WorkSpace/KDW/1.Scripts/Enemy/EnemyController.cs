using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private EnemyState currentState;

    [Header("이동/회전")]
    [SerializeField] private float moveSpeed = 3.0f;    //Enemy 이동속도
    [SerializeField] private float rotateSpeed = 8.0f;

    [Header("공격 거리/쿨타임")]
    [SerializeField] private float attackRange = 5.0f;  //공격 거리
    [SerializeField] private float attackDelay = 10.0f; //공격 시간 쿨타임
    [SerializeField] private float attackChargeTime = 3.0f;

    [Header("타겟")]
    [SerializeField] private Transform target;  //타겟(플레이어)
    [SerializeField] private LayerMask playerLayer;

    [Header("근거리 적 고속이동 속도")]
    [SerializeField] private float runSpeed = 10.0f;

    private Rigidbody rb;
    private NavMeshAgent nvAgent;

    private float nextAttackTime = 0.0f; //다음 공격 시간 쿨타임 저장용
    private bool isDead;
    private bool isAttack; //공격(돌진) 중일 때 확인
    private bool isWall;   //벽 충돌 확인

    private Vector3 beforeTargetPos;

    //상태 객체들
    public EnemyState ChaseState { get; private set; }
    public EnemyState AttackState { get; private set; }

    //현재 상태가 어떤 종류인지
    public StateEnums CurrentStateType { get; private set; }

    //외부에서 읽을 수 있도록 하는 프로퍼티
    public float AttackRange => attackRange;
    public float NextAttackTime => nextAttackTime;
    public Transform Target => target;

    public Vector3 BeforeTargetPos
    {
        get { return beforeTargetPos; }
        set { beforeTargetPos = value; }
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

        ChaseState = new ChaseState(this);
        AttackState = new AttackState(this);

        ChangeState(ChaseState);

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
    //플레이어까지의 거리
    public float DistToPlayer()
    {
        if (target == null) return Mathf.Infinity;

        return Vector3.Distance(transform.position, target.position);
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
    public void Attack()
    {
        if (Time.time < nextAttackTime) return;
        
        isAttack = true;

        nvAgent.enabled = false; //NebMesh는 기본적으로 매 프레임마다 타겟의 위치를 업데이트하기 때문에, Enemy를 멈추고 타겟의 이전위치를 저장하여 후에 그곳으로 이동할려면 .enable = false로 해야한다.
        nvAgent.velocity = Vector3.zero; //NavMeshAgent를 꺼도 남는 잔류 속도 초기화
        rb.velocity = Vector3.zero; //Rigidbody 기존 속도를 초기화 하여 돌진 직선 유지

        StartCoroutine(AttackCharge());

        isAttack = false;

        nextAttackTime = Time.time + attackDelay;
        

    }
    //현재 회전을 담당하는 LookTo는 쓰지않고 있음(NevMesh가 알아서 회전함)
    public void LookTo(Vector3 dir)
    {
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);

        Quaternion newRot = Quaternion.Slerp(rb.rotation, targetRot, rotateSpeed * Time.deltaTime);

        rb.MoveRotation(newRot);
    }
    IEnumerator AttackCharge()
    {
        //beforeTargetPos = target.position;
        //Vector3 attackDir = (beforeTargetPos - rb.position).normalized;
        //transform.forward = attackDir;

        Debug.Log("공격시작");
        yield return new WaitForSeconds(attackChargeTime); //공격 시작하면 실제로 돌진하기까지의 차지타임

        Debug.Log("발사!!!!!!!!");

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
        nvAgent.enabled = true;
        nvAgent.Warp(transform.position);
        //nvAgent.enabled = fals에서 NavMeshAgent는 Rigidbody로 이동시킨 위치를 따라가지 못함
        //그래서 그러한 어긋남을 해결하기 위해 .Warp로 Rigidbody로 이동시킨 위치로 이동

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawRay(transform.position, target.transform.position - transform.position);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Wall"))
        {
            isWall = true;
        }
    }

}
