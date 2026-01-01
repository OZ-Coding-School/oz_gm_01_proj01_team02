using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class DmgText : MonoBehaviour
{
    private TextMeshProUGUI dmgText;
    private RectTransform rect;
    private Camera cam;
    private float lifeTime = 1.0f;
    private float moveSpeed = 2.0f;

    public RectTransform canvasTransform;

    private bool isCritical = false;
    private Coroutine moveCoroutine;
    // private bool isPlaying = false;


    void Awake()
    {
        dmgText = GetComponent<TextMeshProUGUI>();
        rect = GetComponent<RectTransform>();

    }
    
    
    void OnEnable()
    {
        

        // LookCamera();
          
    }

   

    private void Update()
    {
        // rect.position += Vector3.up * 30.0f * Time.deltaTime;
        

    }


    public void Play(float dmg, Vector2 localPos, bool critical = false)
    {
        
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
            moveCoroutine = null;
        }
        

        dmgText.text = $"-{Mathf.RoundToInt(dmg)}";
        isCritical = critical;

        if (isCritical)
        {
            dmgText.color = Color.red;
        }

        else
        {
            dmgText.color = Color.white;
        }

        rect.localScale = Vector3.one;
        rect.localPosition = localPos;

        
        
        
        gameObject.SetActive(true);
        moveCoroutine = StartCoroutine(MoveUp());
    }

    
    private IEnumerator MoveUp()
    {
        
        yield return null;

        float elapsed = 0.0f;
        Color startColor = dmgText.color;

        while (elapsed < lifeTime)
        {
            elapsed += Time.deltaTime;
            rect.localPosition += Vector3.up * moveSpeed * Time.deltaTime;

            float alpha = Mathf.Lerp(1.0f, 0f, elapsed / lifeTime);
            dmgText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            yield return null;
        }
        ReturnToPool();
        moveCoroutine = null;

    }
    
    public void ReturnToPool()
    {
        
        transform.SetParent(PoolManager.pool_instance.transform, false);
        PoolManager.pool_instance.ReturnPool(this);
        gameObject.SetActive(false);  
    }

    private void OnDisable()
    {
        
    }

    
}