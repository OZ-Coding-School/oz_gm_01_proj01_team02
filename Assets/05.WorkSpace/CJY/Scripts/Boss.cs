using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public abstract class Boss : MonoBehaviour
{
    [Header("Special Attack Settings")]
    [SerializeField] protected float specialAttackCooldown = 10.0f;

    protected float lastSpecialAttackTime = -Mathf.Infinity;
    public bool CanSpecialAttack = false;

    public bool IsSpecialAttackReady => Time.time >= lastSpecialAttackTime + specialAttackCooldown;

    protected void StartSpecialAttackCooldown()
    {
        lastSpecialAttackTime = Time.time;
    }

    public void BossDie()
    {
        Debug.Log("Boss Die");
        // Boss death logic here
    }

    public abstract void SpecialAttack();
}

