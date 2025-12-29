using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHpBar : MonoBehaviour
{
    public Slider hpSlider;
    public Transform enemy;
    public Camera mainCamera;
    private EnemyController enemyController;
    public Vector3 offset = new Vector3(0, 0, 2f);

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

            if (mainCamera == null)
            mainCamera = Camera.main;
        // 체력바를 처음엔 비활성화
            // transform.gameObject.SetActive(false);
        }
        

    }

    void Update()
    {
        if (enemy == null || enemyController == null) return;

        
        Vector3 screenPos = Camera.main.WorldToScreenPoint(enemy.position + offset);
        transform.position = screenPos;
        
          

        hpSlider.value = enemyController.currentHp;    
    }

    void LateUpdate()
    {
        
    }
}


