using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShoot : MonoBehaviour
{
    [Header("프리팹/생성위치")]
    [SerializeField] private TestBullet bulletPrefab;
    [SerializeField] private Transform bulletPos;

    //[Header("투사체 발사 간격")]
    //[SerializeField] private float timer = 0.2f;
    //[SerializeField] private float spawnTime = 0.2f;

    [Header("공격 애니메이션 속도")]
    [SerializeField] private float startSpeed = 1.0f;
    [SerializeField] private float nextSpeed = 1.0f;


    private PlayerEnemySearch enemySearch;
    private PlayerMove player;

    private Rigidbody rb;
    private Animator anim;

    private AnimatorStateInfo stateInfo;

    private Vector3 playerVec;

    private bool isSpawning = false;

    //private float spawnTime = 0.2f;

    //애니메이션
    private static readonly int attackaHash = Animator.StringToHash("DoAttack");

    private void Awake()
    {
        enemySearch = GetComponent<PlayerEnemySearch>();
        player = GetComponent<PlayerMove>();

        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
    }
    private void Start()
    {
        GameManager.Pool.CreatePool(bulletPrefab, 50);
    }
    private void Update()
    {
        //timer += Time.deltaTime;
        //
        //if (timer >= spawnTime)
        //{
        //    ShootBullet();
        //    timer = 0.0f;
        //}

        stateInfo = anim.GetCurrentAnimatorStateInfo(0);

        if(stateInfo.IsName("Standing Draw Arrow") || stateInfo.IsName("Standing Aim Recoil"))
        {
            anim.speed = nextSpeed;
        }
        else
        {
            anim.speed = startSpeed;
        }

        ShootBullet();
    }
    public void ShootBullet()
    {
        if (enemySearch.CloseEnemy != null && rb.velocity.sqrMagnitude < 0.0001f)
        {
            //TestBullet bullet = GameManager.Pool.GetFromPool(bulletPrefab);
            //bullet.transform.localPosition = bulletPos.position;
            //bullet.transform.forward = player.EnemyDir;

            //anim.SetBool(attackaHash, true);
            //anim.SetTrigger(attackaHash);

            //Debug.Log("sss");

            StartCoroutine(ShootAnimation());
        }
    }
    private void BulletCreat()
    {
        TestBullet bullet = GameManager.Pool.GetFromPool(bulletPrefab);
        bullet.transform.localPosition = bulletPos.position;
        bullet.transform.forward = player.EnemyDir;
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

            BulletCreat();
        }

        

        isSpawning = false;
    }
}
