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
        private Animator anim;

        private AnimatorStateInfo stateInfo;

        private bool isSpawning = false;

        //private float spawnTime = 0.2f;

        //�ִϸ��̼�
        private static readonly int attackaHash = Animator.StringToHash("DoAttack");

        private void Awake()
        {
            enemySearch = GetComponent<PlayerEnemySearch>();
            playerMove = GetComponent<PlayerMove>();
            playerController = GetComponent<PlayerController>();

            rb = GetComponent<Rigidbody>();
            anim = GetComponent<Animator>();
        }

        private void Start()
        {
            GameManager.Pool.CreatePool(bulletPrefab, 50);
        }

        private void Update()
        {
            stateInfo = anim.GetCurrentAnimatorStateInfo(0);

            if (stateInfo.IsName("Standing Draw Arrow") || stateInfo.IsName("Standing Aim Recoil"))
            {
                anim.speed = playerController.Stats.attackSpeed;
            }
            else
            {
                anim.speed = playerController.Stats.attackSpeed;
            }
            ShootBullet();
        }
        public void ShootBullet()
        {

            if (enemySearch.CloseEnemy != null && rb.velocity.sqrMagnitude < 0.0001f)
            {
                //TestBullet bullet = GameManager.Pool.GetFromPool(bullet);
                //bullet.transform.localPosition = bulletPos.position;
                //bullet.transform.forward = player.EnemyDir;

                //anim.SetBool(attackaHash, true);
                //anim.SetTrigger(attackaHash);

                //Debug.Log("sss");

                StartCoroutine(ShootAnimation());
            }
        }

        private void CreateBullet()
        {
            Debug.Log("Create Bullet");
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
                Vector3 direction = playerMove.EnemyDir;
                direction.y = 0;
                bullet.transform.forward = direction;
                bullet.Initialize(playerController.Stats, playerController.Modifiers);
            }
        }


        IEnumerator ShootAnimation()
        {
            if (isSpawning) yield break;
            isSpawning = true;

            anim.SetTrigger(attackaHash);

            yield return null;

            if (stateInfo.IsName("Standing Draw Arrow"))
            {
                yield return new WaitForSeconds(anim.GetCurrentAnimatorStateInfo(0).length);

                CreateBullet();
            }



            isSpawning = false;
        }
    }
}