using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestPlayerEnemySearch : MonoBehaviour
{
    [Header("�� ����")]
    [SerializeField] private float checkRange;  //������ �Ÿ�
    [SerializeField] private LayerMask enemyLayer;

    public GameObject closeEnemy; //���� ����� ���� ��Ƶ� ����

    private Vector3 enemyDir;

    public GameObject CloseEnemy => closeEnemy;


    [Header("감지된 적들")]
    public Collider[] detectedEnemies;

    void Update()
    {
        EnemyCheck();
    }

    private void EnemyCheck()
    {
        detectedEnemies = Physics.OverlapSphere(transform.position, checkRange, enemyLayer);

        //���� ���� ���� �����ϴ� Enemy�� üũ
        Collider[] Colliders = Physics.OverlapSphere(transform.position, checkRange, enemyLayer);


        Debug.Log($"detectedEnemies.Length = {detectedEnemies.Length}");
        Debug.Log($"Colliders.Length = {Colliders.Length}");
        //���� ���� ���� Enemy�� �ϳ��� ������ return 
        if (Colliders.Length == 0)
        {
            closeEnemy = null;
            return;
        }

        float minDis = Mathf.Infinity;  //���� ����� �Ÿ� ����
        GameObject nearEnemy = null;    //���� ����� �� ���ſ� ���� ����
        Vector3 currentPos = transform.position;  //���� �÷��̾��� ��ġ

        foreach (Collider collider in Colliders)
        {
            float dis = Vector3.Distance(collider.transform.position, currentPos);

            RaycastHit hit;
            enemyDir = collider.transform.position - transform.position;
            bool isHit = Physics.Raycast(transform.position, enemyDir, out hit, checkRange, enemyLayer);
            Debug.Log($"{isHit}");

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
