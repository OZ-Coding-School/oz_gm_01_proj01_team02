using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using STH.Characters.Player;
using STH.Combat.Projectiles;
using STH.Core;

namespace STH.Characters.Player
{
    public class PlayerShoot : MonoBehaviour
    {
        [Header("bullet")]
        [SerializeField] private Bullet bulletPrefab;
        [SerializeField] private Transform firePoint;

        //[Header("����ü �߻� ����")]
        //[SerializeField] private float timer = 0.2f;
        //[SerializeField] private float spawnTime = 0.2f;

        [Header("animation speed")]
        [SerializeField] private float startSpeed = 1.0f;
        [SerializeField] private float nextSpeed = 1.0f;


        private PlayerEnemySearch enemySearch;
        private PlayerMove playerMove;
        private PlayerController playerController;

        private Rigidbody rb;


        private AnimatorStateInfo stateInfo;

        private bool isSpawning = false;
        private Vector3 shootStartPosition;

        //private float spawnTime = 0.2f;

        //�ִϸ��̼�
        private static readonly int attackaHash = Animator.StringToHash("DoAttack");

        private void Awake()
        {
            enemySearch = GetComponent<PlayerEnemySearch>();
            playerMove = GetComponent<PlayerMove>();
            playerController = GetComponent<PlayerController>();

            rb = GetComponent<Rigidbody>();

        }

        private void Start()
        {
            GameManager.Pool.CreatePool(bulletPrefab, 50);
        }

        private void Update()
        {
            if (playerController.IsDead) return;

            stateInfo = playerController.Animator.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Standing Draw Arrow") || stateInfo.IsName("Standing Aim Recoil"))
            {
                playerController.Animator.speed = playerController.Stats.attackSpeed;
            }
            else
            {
                playerController.Animator.speed = playerController.Stats.attackSpeed;
            }
            ShootBullet();
        }
        public void ShootBullet()
        {

            if (enemySearch.CloseEnemy != null && rb.velocity.sqrMagnitude < 0.0001f)
            {
                shootStartPosition = transform.position;
                StartCoroutine(ShootAnimation());
            }
            else
            {
                StopCoroutine(ShootAnimation());
            }
        }

        private void CreateBullet()
        {
            // Debug.Log("Create Bullet");
            if (playerController.Strategies.Count == 0)
            {
                // 기본 단발 공격
                SpawnBulletCallback(firePoint.position, firePoint.rotation);
                return;
            }

            // 모든 전략 실행
            foreach (var strategy in playerController.Strategies)
            {
                strategy.Fire(firePoint, SpawnBulletCallback);
            }
        }

        private void SpawnBulletCallback(Vector3 position, Quaternion rotation)
        {
            Bullet bullet = GameManager.Pool.GetFromPool(bulletPrefab);

            if (bullet != null)
            {
                bullet.transform.SetLocalPositionAndRotation(position, rotation);
                bullet.Initialize(playerController.Stats, playerController.Modifiers);
            }
        }


        IEnumerator ShootAnimation()
        {
            if (isSpawning) yield break;
            isSpawning = true;

            playerController.Animator.SetTrigger(attackaHash);

            yield return null;

            if (stateInfo.IsName("Standing Draw Arrow"))
            {
                yield return new WaitForSeconds(playerController.Animator.GetCurrentAnimatorStateInfo(0).length);

                // 위치가 변하지 않았을 때만 발사
                if (transform.position == shootStartPosition)
                {
                    SoundManager.Instance.Play("Shoot");
                    CreateBullet();
                }
            }



            isSpawning = false;
        }
    }
}