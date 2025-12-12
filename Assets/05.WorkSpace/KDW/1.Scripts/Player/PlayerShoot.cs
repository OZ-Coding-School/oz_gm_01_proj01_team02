using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("프리팹/생성위치")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform bulletPos;

    private PlayerEnemySearch enemySearch;
    private PlayerMove player;

    private Rigidbody rb;

    private Vector3 playerVec;

    private void Awake()
    {
        enemySearch = GetComponent<PlayerEnemySearch>();
        player = GetComponent<PlayerMove>();

        rb = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        ShootBullet();
    }

    public void ShootBullet()
    {
        if (enemySearch.CloseEnemy != null && rb.velocity.sqrMagnitude < 0.0001f)
        {
            GameObject bullet = Instantiate(bulletPrefab, bulletPos.position, Quaternion.identity);
            bullet.transform.forward = player.EnemyDir;
        }
    }
}
