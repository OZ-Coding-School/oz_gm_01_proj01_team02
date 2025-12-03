using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    

    // Update is called once per frame
    void Update()
    {
        Vector2 dir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;

        transform.position +=new Vector3(dir.x, 0, dir.y) * 5f * Time.deltaTime;
    }
}
