using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private EnemyState currentState;

    [Header("이동/회전")]
    [SerializeField] private float moveSpeed = 3.0f;
    [SerializeField] private float rotateSpeed = 8.0f;

    [Header("공격 거리/쿨타임")]
    [SerializeField] private float attackRange = 5.0f;
    [SerializeField] private float attackDelay = 10.0f;
    [SerializeField] private float attackChargeTime = 3.0f;

    [Header("타겟")]
    [SerializeField] private Transform target;
    [SerializeField] private LayerMask playerLayer;

    [Header("근거리 적 고속이동 속도")]
    [SerializeField] private float runSpeed = 10.0f;

    private Rigidbody rb;
    private NavMeshAgent nvAgent;

    private float nextAttackTime = 0.0f;
    private bool isDead;
    private bool isAttack;

    private float currentChargeTime;

    //상태 객체들
    public EnemyState ChaseState { get; private set; }
    public EnemyState AttackState { get; private set; }

    //현재 상태가 어떤 종류인지
    public StateEnums CurrentStateType { get; private set; }

    //외부에서 읽을 수 있도록 하는 프로퍼티
    public float AttackRange => attackRange;
    public float NextAttackTime => nextAttackTime;
    public Transform Target => target;
    public bool IsAttack => isAttack;
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
        nvAgent.SetDestination(target.transform.position);
    }
    public void Attack()
    {
        if (Time.time < nextAttackTime)
        {
            isAttack = false;
        }
        else isAttack = true;

        if (isAttack)
        {
            nvAgent.isStopped = true;
            nvAgent.velocity = Vector3.zero;

            StartCoroutine(Attackcharge());

            nvAgent.isStopped = false;

            nvAgent.speed = moveSpeed;

            nextAttackTime = Time.time + attackDelay;
        }

    }
    public void LookTo(Vector3 dir)
    {
        if (dir.sqrMagnitude < 0.0001f) return;

        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);

        Quaternion newRot = Quaternion.Slerp(rb.rotation, targetRot, rotateSpeed * Time.deltaTime);

        rb.MoveRotation(newRot);
    }
    IEnumerator Attackcharge()
    {
        Debug.Log("공격시작");
        yield return new WaitForSeconds(attackChargeTime);

        Vector3 dir = (target.transform.position - transform.position).normalized;

        rb.AddForce(dir * runSpeed, ForceMode.VelocityChange);

        rb.velocity = Vector3.zero;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position, attackRange);
    }

}
