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
    // private bool isPlaying = false;


    void Awake()
    {
        dmgText = GetComponent<TextMeshProUGUI>();
        rect = GetComponent<RectTransform>();

    }
    
    
    void OnEnable()
    {
        StopAllCoroutines();

        // LookCamera();
          
    }

    private void Update()
    {
        // rect.position += Vector3.up * 30.0f * Time.deltaTime;
    }


    public void Play(float dmg, Vector2 localPos, bool critical = false)
    {
        // if (isPlaying) return;
        // isPlaying = true;

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
        rect.anchoredPosition = localPos;

        
        
        lifeTime = 1.0f;
        
        StartCoroutine(PlayCoroutine());
    }

    private IEnumerator PlayCoroutine()
    {
        yield return null;
        gameObject.SetActive(true);
        StartCoroutine(MoveUp());
    }

    private System.Collections.IEnumerator MoveUp()
    {
        
        yield return null;

        float elapsed = 0.0f;
        Color startColor = dmgText.color;

        while (elapsed < lifeTime)
        {
            elapsed += Time.deltaTime;
             rect.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;

            float alpha = Mathf.Lerp(1.0f, 0f, elapsed / lifeTime);
            dmgText.color = new Color(startColor.r, startColor.g, startColor.b, alpha);

            yield return null;
        }
        transform.SetParent(PoolManager.pool_instance.transform, false);
        gameObject.SetActive(false);
        PoolManager.pool_instance.ReturnPool(this);
        // isPlaying = false;
    }
    

    
}
