using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestPlayer : MonoBehaviour
{
    private Rigidbody rb;
    public float moveSpeed;
    private Vector2 moveInput;
    private HUD hud;



    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    

    void Update()
    {
        moveInput.x = Input.GetAxisRaw("Horizontal");
        moveInput.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector3(moveInput.x, 0f, moveInput.y)*moveSpeed;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {

            Destroy(collision.gameObject);
        }

        if (collision.gameObject.CompareTag("Coin"))
        {
            TestGameManager.Instance.GetExp(10);
            TestGameManager.Instance.GetCoin(10);
            Destroy(collision.gameObject);
        }
    }

   
}
