using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    private Slider expSlider;
    private TextMeshProUGUI levelText;
    private TextMeshProUGUI coinText;
    
    void Awake()
    {
        expSlider = transform.Find("ExpBar")?.GetComponent<Slider>();
        levelText = transform.Find("Level")?.GetComponent<TextMeshProUGUI>();
        coinText = transform.Find("Coin/CoinText")?.GetComponent<TextMeshProUGUI>();
    }

    void LateUpdate()
    {
        var gm = TestGameManager.Instance;
        if (gm == null) return;

        if (expSlider)
        {
            float curExp = gm.exp;
            float maxExp = gm.nextExp[Mathf.Min(gm.level, gm.nextExp.Length - 1)];
            expSlider.value = curExp / maxExp;
        }

        if (levelText)
            levelText.text = $"Lv.{gm.level}";

        if (coinText)
            coinText.text = $"{gm.coin}";
        
    }
}
