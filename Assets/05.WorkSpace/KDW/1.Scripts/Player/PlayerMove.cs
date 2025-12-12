using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 5.0f;
    [SerializeField] private JoyStick joystick;

    private Rigidbody rb;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        float moveX = Input.GetAxis("Horizontal");
        float moveZ = Input.GetAxis("Vertical");

        //rb.velocity = new Vector3(moveX * moveSpeed, rb.velocity.y, moveZ * moveSpeed);

        if (joystick.JoyVector.x != 0 || joystick.JoyVector.y != 0)
        {
            rb.velocity = new Vector3(joystick.JoyVector.x * moveSpeed, rb.velocity.y, joystick.JoyVector.y * moveSpeed);
            //rb.rotation = Quaternion.LookRotation(new Vector3(0, joystick.JoyVector.y, 0));

            transform.forward = rb.velocity;
        }
        else 
        {
            rb.velocity = Vector3.zero;
        }
    }
}
