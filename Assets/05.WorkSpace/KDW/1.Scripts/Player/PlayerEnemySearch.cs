using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemySearch : MonoBehaviour
{
    [Header("적 감지")]
    [SerializeField] private float checkRange;  //적과의 거리
    [SerializeField] private LayerMask enemyLayer;

    private GameObject closeEnemy; //제일 가까운 적을 담아둘 변수

    private Vector3 enemyDir;

    public GameObject CloseEnemy => closeEnemy;

    void Update()
    {
        EnemyCheck();
    }

    private void EnemyCheck()
    {
        //원형 범위 내에 존재하는 Enemy들 체크
        Collider[] Colliders = Physics.OverlapSphere(transform.position, checkRange, enemyLayer);

        //원형 범위 내에 Enemy가 하나도 없으면 return 
        if (Colliders.Length == 0)
        {
            closeEnemy = null;
            return;
        }

        float minDis = Mathf.Infinity;  //제일 가까운 거리 저장
        GameObject nearEnemy = null;    //제일 가까운 적 갱신용 지역 변수
        Vector3 currentPos = transform.position;  //현재 플레이어의 위치

        foreach (Collider collider in Colliders)
        {
            float dis = Vector3.Distance(collider.transform.position, currentPos);

            RaycastHit hit;
            enemyDir = collider.transform.position - transform.position;
            bool isHit = Physics.Raycast(transform.position, enemyDir, out hit, enemyLayer);

            if (isHit && hit.transform.CompareTag("Enemy"))
            {
                if (dis < minDis)
                {
                    minDis = dis;
                    nearEnemy = collider.gameObject;
                }
            }
        }
        closeEnemy = nearEnemy;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRange);

        if (closeEnemy == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, closeEnemy.transform.position - transform.position);
    }
}
