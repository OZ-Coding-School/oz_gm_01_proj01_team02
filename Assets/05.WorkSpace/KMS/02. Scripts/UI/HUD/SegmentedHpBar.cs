using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class SegmentedHpBar : MonoBehaviour
{

    [Header("HP")]
    public int maxHp = 500;
    public int currentHp = 500;

    public int segmentCount = 7;

    [Header("UI")]
    public Transform hpFillRoot;
    public Image hpUnitPrefab;

    private Image[] units;

    private bool isInitialized = false;

    void Start()
    {
        units = new Image[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            units[i] = Instantiate(hpUnitPrefab, hpFillRoot);
        }

        isInitialized = true;
        UpdateUI();
    
    }

    
    

    public void SetHp(int hp)
    {
        if (!isInitialized) return;
        currentHp = Mathf.Clamp(hp, 0, maxHp);
        UpdateUI();
    }

    void UpdateUI()
    {
        float hpRatio = (float) currentHp / maxHp;
        int activeSegments = Mathf.CeilToInt(hpRatio*segmentCount);
        for (int i = 0; i < units.Length; i++)
        {
            units[i].enabled = i < activeSegments;
        }
    }
}
