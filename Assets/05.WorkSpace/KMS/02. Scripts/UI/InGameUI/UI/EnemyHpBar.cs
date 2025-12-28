using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Slider hpSlider;
    public Transform enemy;
    private EnemyController enemyController;

    void Start()
    {
        if (enemy != null)
        {
            enemyController = enemy.GetComponent<EnemyController>();

            if (enemyController != null && hpSlider != null)
            {
                hpSlider.maxValue = enemyController.maxHp;
                hpSlider.value = enemyController.maxHp;
            }
        }

    }

    void Update()
    {
        if (enemy == null || enemyController == null) return;

        transform.position = enemy.position + Vector3.up * 2.0f;

        hpSlider.value = enemyController.currentHp;    
    }
}
