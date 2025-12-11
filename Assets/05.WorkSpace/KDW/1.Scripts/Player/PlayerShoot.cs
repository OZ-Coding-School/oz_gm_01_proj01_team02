using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("프리팹")]
    [SerializeField] private GameObject bulletPrefab;

    [Header("적 감지")]
    [SerializeField] private float checkRange;  //적과의 거리
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private LayerMask layer;

    private GameObject closeEnemy; //제일 가까운 적을 담아둘 변수
    private bool isEnemy = false;

    private Vector3 enemyDir;

    void Start()
    {
        
    }

    void Update()
    {
        EnemyCheck();
    }

    private void EnemyCheck()
    {
        Collider[] Colliders = Physics.OverlapSphere(transform.position, checkRange, enemyLayer);

        if (Colliders.Length == 0)
        {
            return;
        }

        float minDis = Mathf.Infinity;  //제일 가까운 거리 저장
        GameObject nearEnemy = null;
        Vector3 currentPos = transform.position;

        foreach (Collider collider in Colliders)
        {
            float dis = Vector3.Distance(collider.transform.position, currentPos);

            if (dis <  minDis)
            {
                minDis = dis;
                nearEnemy = collider.gameObject;
            }
        }
        closeEnemy = nearEnemy;

        EnemyLook(closeEnemy);

        if (closeEnemy != null && isEnemy == true)
        {
            Shoot();
        }
    }
    private bool EnemyLook(GameObject closeEnemy)
    {
        enemyDir = closeEnemy.transform.position - transform.position;
        //isEnemy = Physics.Raycast(transform.position, enemyDir, enemyLayer);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, enemyDir, out hit, enemyLayer))
        {
            isEnemy = true;
            //Debug.Log(hit.collider.name);
        }
        else if (Physics.Raycast(transform.position, enemyDir, out hit, layer))
        {
            isEnemy = false;
            //Debug.Log(hit.collider.name);
        }
        return isEnemy;
    }
    private void Shoot()
    {
        Debug.Log(closeEnemy.name);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, checkRange);

        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position, enemyDir);
    }
}
