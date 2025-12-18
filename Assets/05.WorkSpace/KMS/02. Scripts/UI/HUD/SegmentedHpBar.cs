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
    private float displayHp;

    public int segmentCount = 7;
    private Image[] units;


    [Header("UI")]
    public Transform hpFillRoot;
    public Image hpUnitPrefab;

    [Header("Animation")]
    public float hpAnimSpeed = 100.0f;
    private Coroutine hpAnimCoroutine;



    private bool isInitialized = false;

    void Start()
    {
        units = new Image[segmentCount];
        for (int i = 0; i < segmentCount; i++)
        {
            units[i] = Instantiate(hpUnitPrefab, hpFillRoot);
        }

        displayHp = TestGameManager.Instance.currentHp;
        isInitialized = true;
        UpdateUI();
    
    }

    
    

    public void SetHp(int hp)
    {
        if (!isInitialized) return;
        currentHp = Mathf.Clamp(hp, 0, maxHp);

        if (hpAnimCoroutine != null)
        {
            StopCoroutine(hpAnimCoroutine);
        }
        
        UpdateUI();
        hpAnimCoroutine = StartCoroutine(AnimateHp());
    }

    IEnumerator AnimateHp()
    {
        while (!Mathf.Approximately(displayHp, currentHp))
        {
            displayHp = Mathf.MoveTowards(displayHp, currentHp, hpAnimSpeed * Time.deltaTime);
        

        UpdateUIWithValue(displayHp);
        yield return null;
        }

        displayHp = TestGameManager.Instance.currentHp;
        UpdateUIWithValue(displayHp);
    }

    void UpdateUI()
    {
        UpdateUIWithValue(currentHp);
    }


    void UpdateUIWithValue(float amount)
    {
        float hpRatio = (float) currentHp / maxHp;
        int activeSegments = Mathf.CeilToInt(hpRatio*segmentCount);
        for (int i = 0; i < units.Length; i++)
        {
            units[i].enabled = i < activeSegments;
        }
    }
}
