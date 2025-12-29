using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinBlade : MonoBehaviour
{
    [SerializeField] GameObject rail;
    [SerializeField] float moveSpeed = 5f;
    [SerializeField] int damage = 50;

    // ���� ��ǥ ������ �������� ����
    private Vector3 startPos;
    private Vector3 endPos;
    private Vector3 currentTarget;

    private PlayerHealth player;

    void Start()
    {
        float railLength = rail.transform.localScale.x;
        float halfLength = railLength / 2f;

        startPos = new Vector3(-halfLength, -0.5f, 0);
        endPos = new Vector3(halfLength, -0.5f, 0);

        transform.localPosition = startPos;
        currentTarget = endPos;
    }

    void Update()
    {
        MoveBlade();
    }

    private void MoveBlade()
    {
        transform.localPosition = Vector3.MoveTowards(transform.localPosition, currentTarget, moveSpeed * Time.deltaTime);

        if (Vector3.Distance(transform.localPosition, currentTarget) < 0.01f)
        {

            if (currentTarget == endPos) currentTarget = startPos;
            else currentTarget = endPos;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            player = other.GetComponent<PlayerHealth>();
            if (player != null)
            {
                player.TakeDamage(damage);
            }
        }
    }
}