using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemySearch : MonoBehaviour
{
    [Header("�� ����")]
    [SerializeField] private float checkRange;  //������ �Ÿ�
    [SerializeField] private LayerMask enemyLayer;

    public GameObject closeEnemy; //���� ����� ���� ��Ƶ� ����

    private Vector3 enemyDir;

    public GameObject CloseEnemy => closeEnemy;

    
    void Update()
    {
        EnemyCheck();
    }

    private void EnemyCheck()
    {

        //���� ���� ���� �����ϴ� Enemy�� üũ
        Collider[] Colliders = Physics.OverlapSphere(transform.position, checkRange, enemyLayer);

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
