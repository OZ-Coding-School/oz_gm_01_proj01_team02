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


    void Awake()
    {
        cam = Camera.main;     
        dmgText = GetComponent<TextMeshProUGUI>(); 
        rect = GetComponent<RectTransform>();
        Canvas dmgCanvas = GameObject.Find("DmgTextCanvas")?.GetComponent<Canvas>();
    if (dmgCanvas != null)
        canvasTransform = dmgCanvas.transform as RectTransform;
    else
        Debug.LogError("씬에 'DmgTextCanvas'가 없습니다");

    }
    
    
    void OnEnable()
    {
        StopAllCoroutines();

        // LookCamera();
        StartCoroutine(LifeRoutine());        
    }

    private void Update()
    {
        // rect.position += Vector3.up * 30.0f * Time.deltaTime;
    }



    public void Play(Vector3 worldPos, float damage, bool isCritical = false)
    {
        if (dmgText == null) return; 
        Debug.Log($"[DmgText] Play 호출됨 damage:{damage}");

        dmgText.text = $"-{Mathf.RoundToInt(damage)}";

        if (isCritical)
        {
            dmgText.color = Color.red;
        }

        else
        {
            dmgText.color = Color.white;
        }

        if (canvasTransform != null)
        transform.SetParent(canvasTransform, false);

        rect.pivot = new Vector2(0.5f, 1f);

        float fixedYOffset = 2.0f;
        Vector3 fixedWorldPos = new Vector3(worldPos.x, worldPos.y + fixedYOffset, worldPos.z);

        if (cam != null)
        rect.position = cam.WorldToScreenPoint(fixedWorldPos);
        



        gameObject.SetActive(true);

    }

    private IEnumerator LifeRoutine()
    {
        yield return new WaitForSeconds(lifeTime);

        Debug.Log($"[DmgText] ReturnPool 호출됨 : {gameObject.name}");
        PoolManager.pool_instance.ReturnPool(this);

    }

    

    
}
