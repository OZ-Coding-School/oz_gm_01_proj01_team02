using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegmentedHpBar : MonoBehaviour
{

    [Header("HP")]
    public int maxHp;
    public int currentHp;
    private float displayHp;
    private float displayBackHp;

    public int segmentCount = 7;
    private Image[] fillUnits;
    private Image[] backUnits;


    [Header("UI")]
    public Transform hpFillRoot;
    public Transform hpBackRoot;
    public Image hpUnitPrefab;
    public Image hpBackUnitPrefab;

    [Header("Animation")]
    public float hpAnimSpeed = 100.0f;
    public float backAnimSpeed = 50.0f;
    private Coroutine hpAnimCoroutine;



    private bool isInitialized = false;

    void Start()
    {
        fillUnits = new Image[segmentCount];
        backUnits = new Image[segmentCount];


        for (int i = 0; i < segmentCount; i++)
        {
            fillUnits[i] = Instantiate(hpUnitPrefab, hpFillRoot);
            backUnits[i] = Instantiate(hpBackUnitPrefab, hpBackRoot);
        }

        currentHp = TestGameManager.Instance.currentHp;
        maxHp = TestGameManager.Instance.maxHp;

        displayHp = currentHp;
        displayBackHp = currentHp;

        isInitialized = true;

        UpdateUIWithValue(displayHp, displayBackHp);
    
    }

    
    void Update()
    {
        int gmHp = TestGameManager.Instance.currentHp;

        if (gmHp != currentHp)
        {
            SetHp(gmHp);
        }

    }

    public void SetHp(int hp)
    {
        if (!isInitialized) return;
        currentHp = Mathf.Clamp(hp, 0, maxHp);

        if (hpAnimCoroutine != null)
        {
            StopCoroutine(hpAnimCoroutine);
        }
        
        
        hpAnimCoroutine = StartCoroutine(AnimateHp());
    }

    IEnumerator AnimateHp()
    {
        while (!Mathf.Approximately(displayHp, currentHp) || !Mathf.Approximately(displayBackHp, displayHp))
        {
            displayHp = Mathf.MoveTowards(displayHp, currentHp, hpAnimSpeed * Time.deltaTime);

            displayBackHp = Mathf.MoveTowards(displayBackHp, displayHp, backAnimSpeed * Time.deltaTime);
        

            UpdateUIWithValue(displayHp, displayBackHp);
            yield return null;
        }

        displayHp = currentHp;
        displayBackHp = displayHp;
        UpdateUIWithValue(displayHp, displayBackHp);
    }


    void UpdateUIWithValue(float frontamount, float backAmount)
    {
        float frontRatio = frontamount / maxHp;
        float backRatio = backAmount / maxHp;

        for (int i = 0; i < segmentCount; i++)
        {
            float unitStart = (float)i / segmentCount;
            float unitEnd = (float)(i + 1) / segmentCount;

            float fillScale = Mathf.Clamp01((frontRatio - unitStart) / (unitEnd - unitStart));
            fillUnits[i].transform.localScale = new Vector3(fillScale, 1, 1);

            float backScale = Mathf.Clamp01((backRatio - unitStart) / (unitEnd - unitStart));
            backUnits[i].transform.localScale = new Vector3(backScale, 1, 1);       
        }
    }
}
