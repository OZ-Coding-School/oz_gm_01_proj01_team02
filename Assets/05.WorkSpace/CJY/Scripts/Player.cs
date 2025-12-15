using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int hp;

    private void Start()
    {
        hp = 500;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        transform.position += new Vector3(dir.x, 0, dir.y) * 5f * Time.deltaTime;
    }

    public void TakeDamage(int damage)
    {
        hp -= damage;
        Debug.Log("Player HP: " + hp);
        if (hp <= 0)
        {
            Debug.Log("Player Dead");
            gameObject.SetActive(false);
        }
    }
}
