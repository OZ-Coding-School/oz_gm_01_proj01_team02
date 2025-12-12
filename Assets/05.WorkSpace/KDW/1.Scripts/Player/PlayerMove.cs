using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private JoyStick joystick;

    private Rigidbody rb;
    private PlayerEnemySearch enemySearch;

    private Vector3 enemyDir;

    public Vector3 EnemyDir => enemyDir;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        enemySearch = GetComponent<PlayerEnemySearch>();
    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        if (joystick.JoyVector.x != 0 || joystick.JoyVector.y != 0)
        {
            rb.velocity = new Vector3(joystick.JoyVector.x * moveSpeed, rb.velocity.y, joystick.JoyVector.y * moveSpeed);
            //rb.rotation = Quaternion.LookRotation(new Vector3(0, joystick.JoyVector.y, 0));

            transform.forward = rb.velocity;
        }
        else 
        {
            rb.velocity = Vector3.zero;

            if (enemySearch.CloseEnemy != null)
            {
                enemyDir = (enemySearch.CloseEnemy.transform.position - transform.position).normalized;
                transform.forward = enemyDir;
            }
        }
    }
}
