using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] int damage = 30;
    bool isWorking;
    float coolTime = 2f;
    float currentCool = 0f;

    private void Start()
    {
        isWorking = true;
    }

    private void Update()
    {
        if (!isWorking)
        {
            currentCool += Time.deltaTime;
            if (currentCool >= coolTime)
            {
                isWorking = true;
                currentCool = 0f;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Player player = other.GetComponent<Player>();
            if (player != null && player.hp > 0 && isWorking)
            {
                player.TakeDamage(damage);
                isWorking = false;
                MeshRenderer mr = player.GetComponent<MeshRenderer>();
                if (mr != null)
                {
                    mr.material.color = Color.red;
                    StartCoroutine(ResetColor(mr));
                }
            }
        }
    }
    IEnumerator ResetColor(MeshRenderer mr)
    {
        yield return new WaitForSeconds(0.2f);
        if (mr != null) mr.material.color = Color.white;
    }
}
