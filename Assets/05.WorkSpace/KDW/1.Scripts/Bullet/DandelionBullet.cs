using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows.Speech;

public class DandelionBullet : MonoBehaviour
{
    [Header("속도/생성 시간/살아있는 시간")]
    [SerializeField] private float bulletSpeed = 5.0f;
    [SerializeField] private float lifeTime = 3.0f; //살아 있는 시간

    [Header("프리팹/한번에 발사 수/프리팹 총각도/공격변환 시간")]
    [SerializeField] private TextEnemyBullet childBullet;
    [SerializeField] private float childSpawnTime = 1.0f;
    [SerializeField] private float childSecondTime = 1.0f;
    [SerializeField] private int bulletCount = 8;       //한번에 발사할 수
    [SerializeField] private float spreadAngle = 300.0f; //발사 시 프리팹과의 총각도
    [SerializeField] private float dandelionTime = 3.0f;
    private float timer = 0.0f;

    private WaitForSeconds waitForDandelion;

    private float childReturnTime;
    private int dandelionCount = 1;

    private void Awake()
    {
        GameManager.Pool.CreatePool(childBullet, 50);

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
            ReturnPool();
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
            TextEnemyBullet childBullet = GameManager.Pool.GetFromPool(this.childBullet);

            float currentAngle = startAngle + (angleStep * i); //현재 프리팹 간격(순서대로 간격 계산)

            Quaternion rotation = transform.rotation * Quaternion.Euler(0, currentAngle, 0);

            childBullet.transform.SetLocalPositionAndRotation(transform.position, rotation);

        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            ReturnPool();
        }

        if (other.CompareTag("Wall"))
        {
            ReturnPool();
        }
    }
    private void ReturnPool()
    {
        if (PoolManager.pool_instance != null)
        {
            PoolManager.pool_instance.ReturnPool(this);
        }
    }
}
