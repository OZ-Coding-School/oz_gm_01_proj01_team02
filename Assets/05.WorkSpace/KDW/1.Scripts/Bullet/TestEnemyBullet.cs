using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TextEnemyBullet : MonoBehaviour
{
    [SerializeField] private float bulletSpeed = 10.0f;
    [SerializeField] float lifeTime = 3.0f; //살아 있는 시간

    private float spawnTime;

    private void OnEnable()
    {
        spawnTime = Time.time;
    }

    void Update()
    {
        transform.position += transform.forward * bulletSpeed * Time.deltaTime;

        if (Time.time - spawnTime >= lifeTime)
        {
            ReturnPool();
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
            Debug.Log("벽이당");
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
