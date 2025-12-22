using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("프리팹/생성위치")]
    [SerializeField] private TestBullet bulletPrefab;
    [SerializeField] private Transform bulletPos;

    private PlayerEnemySearch enemySearch;
    private PlayerMove player;

    private Rigidbody rb;

    private Vector3 playerVec;

    private float spawnTime = 0.2f;

    private void Awake()
    {
        enemySearch = GetComponent<PlayerEnemySearch>();
        player = GetComponent<PlayerMove>();

        rb = GetComponent<Rigidbody>();
    }
    private void Start()
    {
        GameManager.Pool.CreatePool(bulletPrefab, 50);
    }
    private void Update()
    {
        spawnTime += Time.deltaTime;

        if (spawnTime >= 0.2f)
        {
            ShootBullet();
            spawnTime = 0.0f;
        }
    }

    public void ShootBullet()
    {
        if (enemySearch.CloseEnemy != null && rb.velocity.sqrMagnitude < 0.0001f)
        {
            //GameObject bullet = Instantiate(bulletPrefab, bulletPos.position, Quaternion.identity);
            TestBullet bullet = GameManager.Pool.GetFromPool(bulletPrefab);
            bullet.transform.localPosition = bulletPos.position;
            bullet.transform.forward = player.EnemyDir;
        }
    }
}
