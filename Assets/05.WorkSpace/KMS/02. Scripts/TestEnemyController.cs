// using System.Collections;
// using UnityEngine;
// using UnityEngine.AI;

// public class TestEnemyController : MonoBehaviour
// {
//     [Header("Stats")]
//     [SerializeField] private float moveSpeed = 3f;
//     [SerializeField] private float meleeAttackRange = 2f;
//     [SerializeField] private float rangedAttackRange = 10f;
//     [SerializeField] private float attackDelay = 3f;
//     [SerializeField] private TextEnemyBullet enemyBullet;
//     [SerializeField] private Transform bulletPos;

//     private Transform target;
//     private NavMeshAgent agent;
//     private Rigidbody rb;
//     private bool isDead;
//     private float nextAttackTime;

//     public void Init(Transform playerTransform)
//     {
//         target = playerTransform;
//         agent = GetComponent<NavMeshAgent>();
//         rb = GetComponent<Rigidbody>();

//         agent.enabled = true;
//         agent.speed = moveSpeed;

//         if (enemyBullet != null)
//             GameManager.Pool.CreatePool(enemyBullet, 50);

//         isDead = false;
//         StartCoroutine(EnemyRoutine());
//     }

//     private IEnumerator EnemyRoutine()
//     {
//         while (!isDead)
//         {
//             if (target != null)
//             {
//                 float dist = Vector3.Distance(transform.position, target.position);

//                 // 근거리 공격
//                 if (dist <= meleeAttackRange && Time.time >= nextAttackTime)
//                 {
//                     StartCoroutine(MeleeAttack());
//                     nextAttackTime = Time.time + attackDelay;
//                 }
//                 // 원거리 공격
//                 else if (dist <= rangedAttackRange && enemyBullet != null && Time.time >= nextAttackTime)
//                 {
//                     RangedAttack();
//                     nextAttackTime = Time.time + attackDelay;
//                 }
//                 else
//                 {
//                     ChasePlayer();
//                 }
//             }
//             yield return null;
//         }
//     }

//     private void ChasePlayer()
//     {
//         if (agent.enabled && target != null)
//             agent.SetDestination(target.position);
//     }

//     private IEnumerator MeleeAttack()
//     {
//         agent.enabled = false;
//         rb.velocity = Vector3.zero;

//         float timer = 0f;
//         float attackDuration = 0.5f;
//         while (timer < attackDuration)
//         {
//             rb.MovePosition(rb.position + transform.forward * moveSpeed * Time.fixedDeltaTime);
//             timer += Time.fixedDeltaTime;
//             yield return new WaitForFixedUpdate();
//         }

//         agent.enabled = true;
//     }

//     private void RangedAttack()
//     {
//         var bullet = GameManager.Pool.GetFromPool(enemyBullet);
//         bullet.transform.SetPositionAndRotation(bulletPos.position, Quaternion.identity);
//         bullet.transform.forward = transform.forward;
//     }

//     public void ReturnToPool()
//     {
//         isDead = true;
//         agent.enabled = false;
//         rb.velocity = Vector3.zero;
//         GameManager.Pool.ReturnToPool(this);
//     }

//     public void DieAfterSeconds(float seconds = 3f)
//     {
//         StartCoroutine(DieRoutine(seconds));
//     }

//     private IEnumerator DieRoutine(float sec)
//     {
//         yield return new WaitForSeconds(sec);
//         ReturnToPool();
//     }
// }
