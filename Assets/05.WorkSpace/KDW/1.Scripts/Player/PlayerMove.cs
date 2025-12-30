using UnityEngine;
using STH.Characters.Player;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private JoyStick joystick;

    private Rigidbody rb;
    private PlayerController playerController;
    private Animator anim;
    private PlayerEnemySearch enemySearch;

    private Vector3 enemyDir;

    public Vector3 EnemyDir => enemyDir;

    //�ִϸ��̼�
    private static readonly int moveHash = Animator.StringToHash("IsMoving");

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        enemySearch = GetComponent<PlayerEnemySearch>();
        playerController = GetComponent<PlayerController>();

        if (joystick == null)
        {
            joystick = FindObjectOfType<JoyStick>();
        }
    }

    private void FixedUpdate()
    {
        if (!playerController.IsDead && (joystick.JoyVector.x != 0 || joystick.JoyVector.y != 0))
        {
            rb.velocity = new Vector3(joystick.JoyVector.x * moveSpeed, rb.velocity.y, joystick.JoyVector.y * moveSpeed);
            rb.rotation = Quaternion.LookRotation(new Vector3(0, joystick.JoyVector.y, 0));

            transform.forward = rb.velocity;

            anim.SetBool(moveHash, true);
        }
        else
        {
            rb.velocity = Vector3.zero;

            if (enemySearch.CloseEnemy != null)
            {
                enemyDir = (enemySearch.CloseEnemy.transform.position - transform.position).normalized;
                transform.forward = enemyDir;
            }

            anim.SetBool(moveHash, false);
        }
    }

    private void LateUpdate()
    {
        transform.position = new Vector3(transform.position.x, 0f, transform.position.z);
    }
}
