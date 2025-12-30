using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class BossDragon : Boss
{

    [Header("Dandelion Attack Settings")]
    [SerializeField] private int bulletCount = 5;
    [SerializeField] private float spreadAngle = 45.0f;

    [Header("bullet")]
    [SerializeField] private DandelionBullet dandelionBullet;
    [SerializeField] protected Transform bulletPos;


    void Awake()
    {
        GameManager.Pool.CreatePool(dandelionBullet, 50);
    }

    public override void SpecialAttack()
    {
        StartSpecialAttackCooldown();
        DandelionShotAttack();
    }

    public void DandelionShotAttack()
    {

        float angleStep = spreadAngle / (bulletCount - 1); //�� ����/bulletCount��ŭ ����� ���� ���� = �Ѿ� ���� ����
        float startAngle = -spreadAngle / 2;

        if (bulletCount <= 1)
        {
            angleStep = 0f;
            startAngle = 0f;
        }

        for (int i = 0; i < bulletCount; i++)
        {
            DandelionBullet dandelionBullet = GameManager.Pool.GetFromPool(this.dandelionBullet);

            float currentAngle = startAngle + (angleStep * i); //���� ������ ����(������� ���� ���)

            Quaternion rotation = transform.rotation * Quaternion.Euler(0, currentAngle, 0);


            dandelionBullet.transform.SetLocalPositionAndRotation(bulletPos.position, rotation);
            //dandelionBullet.transform.forward = transform.forward;

            Debug.Log(i);
        }

    }
}