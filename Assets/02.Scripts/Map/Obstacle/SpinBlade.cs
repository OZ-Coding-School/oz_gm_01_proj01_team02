using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBlade : MonoBehaviour
{
    private float railLength;
    [SerializeField] GameObject rail;
    [SerializeField] float rotateSpeed = 5f;
    private Vector3 startPos;
    private Vector3 endPos;

    void Start()
    {
        railLength = rail.transform.localScale.x;
        transform.position = transform.parent.position - new Vector3(railLength/2, 0.5f, 0);
        startPos = transform.position;
        endPos = transform.position + new Vector3(railLength, 0, 0);
        Debug.Log(transform.parent.position);
        Debug.Log(transform.position);
    }

    void Update()
    {
        Rotate();
        if (Vector3.Distance(transform.position, endPos) < 0.1f || Vector3.Distance(transform.position, startPos) < 0.1f)
        {
            rotateSpeed = -rotateSpeed;
        }
        if(transform.position.x - endPos.x > 0.5f)
        {
            transform.position = endPos;
        }
        if (Mathf.Abs(transform.position.x) - Mathf.Abs(startPos.x) > 0.5f)
        {
            transform.position = startPos;
        }
    }

    private void Rotate()
    {
        transform.Translate(Vector3.right * rotateSpeed * Time.deltaTime, Space.World);
    }

    private void OnTriggerEnter(Collider other)
    {
        //닿을 시 플레이어에 데미지 입히기
        //if (other.CompareTag("Player"))
        //{
        //    Player player = other.GetComponent<Player>();
        //    if (player.hp != null)
        //    {
        //        player.TakeDamage(50);
        //    }
        //}
    }
}
