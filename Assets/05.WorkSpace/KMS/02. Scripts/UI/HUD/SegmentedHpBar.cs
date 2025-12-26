using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SegmentedHpBar : MonoBehaviour
{

    [Header("HP")]
    public float targetHp;
    public float targetMaxHp;
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
    public float backAnimSpeed = 80.0f;
    private Coroutine hpAnimCoroutine;

    public PlayerHealth playerHp;


    private bool isInitialized = false; 

    private void Awake()
    {
        if (playerHp == null)
        playerHp = FindObjectOfType<PlayerHealth>();
    }

    private void Start()
    {
        fillUnits = new Image[segmentCount];
        backUnits = new Image[segmentCount];


        for (int i = 0; i < segmentCount; i++)
        {
            fillUnits[i] = Instantiate(hpUnitPrefab, hpFillRoot);
            backUnits[i] = Instantiate(hpBackUnitPrefab, hpBackRoot);
        }

        isInitialized = true;

    

        if (playerHp != null)
        OnHpChanged(playerHp.CurrentHp, playerHp.MaxHp, true);
    
    }


    private void OnEnable()
    {
        if (playerHp == null) return;
        playerHp.OnHpChanged += OnHpChangedEvent;

    }

    private void OnDisable()
    {
        if (playerHp == null) return;
        playerHp.OnHpChanged -= OnHpChangedEvent;
    }

    private void OnHpChanged(float current, float max, bool instant = false)
    {
        if (!isInitialized) return;

        targetHp = current;
        targetMaxHp = max;

        if (hpAnimCoroutine != null)
        StopCoroutine(hpAnimCoroutine);

        if (instant)
        {
            displayHp = targetHp;
            displayBackHp = targetHp;
            UpdateUIWithValue(displayHp, displayBackHp);
        }
        else
        {
            hpAnimCoroutine = StartCoroutine(AnimateHp());
        }
    }

    private void OnHpChangedEvent(float current, float max)
    {
        OnHpChanged(current, max, false);
    }

    
    IEnumerator AnimateHp()
    {
        while (!Mathf.Approximately(displayHp, targetHp) || !Mathf.Approximately(displayBackHp, displayHp))
        {
            displayHp = Mathf.MoveTowards(displayHp, targetHp, hpAnimSpeed * Time.deltaTime);

            displayBackHp = Mathf.MoveTowards(displayBackHp, displayHp, backAnimSpeed * Time.deltaTime);
        

            UpdateUIWithValue(displayHp, displayBackHp);
            yield return null;
        }

        displayHp = targetHp;
        displayBackHp = displayHp;
        UpdateUIWithValue(displayHp, displayBackHp);
    }


    void UpdateUIWithValue(float frontamount, float backAmount)
    {
        float frontRatio = frontamount / targetMaxHp;
        float backRatio = backAmount / targetMaxHp;


        for (int i = 0; i < segmentCount; i++)
        {
            int index = segmentCount - 1 - i;
            

            float unitStart = (float)index / segmentCount;
            float unitEnd = (float)(index + 1) / segmentCount;

            float fillScale = Mathf.Clamp01((frontRatio - unitStart) / (unitEnd - unitStart));
            fillUnits[index].transform.localScale = new Vector3(fillScale, 1.0f, 1.0f);

            float backScale = Mathf.Clamp01((backRatio - unitStart) / (unitEnd - unitStart));
            backUnits[index].transform.localScale = new Vector3(backScale, 1.0f, 1.0f);
     
        }
    }
}
