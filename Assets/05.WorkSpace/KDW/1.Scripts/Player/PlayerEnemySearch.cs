using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEnemySearch : MonoBehaviour
{
    [Header("�� ����")]
    [SerializeField] private float checkRange;  //������ �Ÿ�
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private TargetIndicator indicatorPrefab;
    private Transform indicatorInstance;

    public GameObject closeEnemy; //���� ����� ���� ��Ƶ� ����

    private Vector3 enemyDir;

    public GameObject CloseEnemy => closeEnemy;

    void Awake()
    {
        // 원을 하나만 생성해서 미리 꺼둠
        // indicatorInstance = Instantiate(indicatorPrefab).transform;
        // indicatorInstance.gameObject.SetActive(false);
    }

    void Update()
    {
        EnemyCheck();
    }

    void LateUpdate()
    {
        // 타겟이 있으면 원을 타겟 발밑으로 이동
        if (closeEnemy != null && indicatorInstance.gameObject.activeSelf)
        {
            indicatorInstance.position = closeEnemy.transform.position;
        }
    }

    private void EnemyCheck()
    {
        //���� ���� ���� �����ϴ� Enemy�� üũ
        Collider[] Colliders = Physics.OverlapSphere(transform.position, checkRange, enemyLayer);

        Debug.Log(Colliders.Length);

        //���� ���� ���� Enemy�� �ϳ��� ������ return 
        if (Colliders.Length == 0)
        {
            closeEnemy = null;
            indicatorInstance.gameObject.SetActive(false);
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
        // indicatorInstance.gameObject.SetActive(true);
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
