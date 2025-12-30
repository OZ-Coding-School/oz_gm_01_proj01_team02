using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STH.Combat.Projectiles;

public class DandelionBullet : MonoBehaviour
{
    [Header("�ӵ�/���� �ð�/����ִ� �ð�")]
    [SerializeField] private float bulletSpeed = 5.0f;
    [SerializeField] private float lifeTime = 3.0f; //��� �ִ� �ð�

    [Header("������/�ѹ��� �߻� ��/������ �Ѱ���/���ݺ�ȯ �ð�")]
    [SerializeField] private Bullet childBullet;
    [SerializeField] private float childSpawnTime = 1.0f;
    [SerializeField] private float childSecondTime = 1.0f;
    [SerializeField] private int bulletCount = 8;       //�ѹ��� �߻��� ��
    [SerializeField] private float spreadAngle = 300.0f; //�߻� �� �����հ��� �Ѱ���
    [SerializeField] private float dandelionTime = 3.0f;
    private float timer = 0.0f;

    private WaitForSeconds waitForDandelion;

    private float childReturnTime;
    private int dandelionCount = 1;

    private void Awake()
    {
        GameManager.Pool.CreatePool(childBullet, 100);

        waitForDandelion = new WaitForSeconds(dandelionTime);
    }
    private void OnEnable()
    {
        childReturnTime = Time.time;
    }

    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;

        StartCoroutine(DandelionShot());
        //DandelionShot();

        if (Time.time - childReturnTime >= lifeTime)
        {
            PoolManager.pool_instance.ReturnPool(this);
        }
    }

    IEnumerator DandelionShot()
    {
        //yield return waitForDandelion;
        yield return null;

        float angleStep = spreadAngle / (bulletCount - 1);
        float startAngle = -spreadAngle / 2;

        timer += Time.deltaTime;

        if (timer >= childSpawnTime)
        {
            DandelionCreat(angleStep, startAngle);
            dandelionCount++;
            childSpawnTime = childSecondTime;
            timer = 0.0f;
        }
    }
    private void DandelionCreat(float angleStep, float startAngle)
    {
        for (int i = 0; i < bulletCount; i++)
        {
            Bullet childBullet = GameManager.Pool.GetFromPool(this.childBullet);

            float currentAngle = startAngle + (angleStep * i); //���� ������ ����(������� ���� ���)

            Quaternion rotation = transform.rotation * Quaternion.Euler(0, currentAngle, 0);

            childBullet.transform.SetLocalPositionAndRotation(transform.position, rotation);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PoolManager.pool_instance.ReturnPool(this);
        }

        if (other.CompareTag("Wall"))
        {
            PoolManager.pool_instance.ReturnPool(this);
        }
    }
}

