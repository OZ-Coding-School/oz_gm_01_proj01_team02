using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public PlayerData playerData;

    [Header("UI")]
    public Text coinText;

    public Slider expSlider;
    public Text levelText;
    public Text expText;
    public MapPanel mapPanel;


    

    void Start()
    {
        // 추가한 코드
        if (CoinManager.Instance != null && playerData != null)
        {
            CoinManager.Instance.LoadCoin(playerData);
        }

        if (playerData.selectedMap != null)
        {
            playerData.OnSelectedMapChanged += OnMapSelected;
        

            if (playerData.selectedMap != null)
            {
                OnMapSelected(playerData.selectedMap);
            }
        }
        UpdateUI();
    }

    private void OnDestroy()
    {
        if (playerData != null)
        {
            playerData.OnSelectedMapChanged -= OnMapSelected;
        }
    }

    public void UpdateUI() // public으로 보호 수준 변경
    {
        if (coinText != null)
        coinText.text = playerData.totalCoin.ToString();
        
        if (levelText != null)
        levelText.text = playerData.adventureLevel.ToString();

        if (expSlider != null)
        {
            if (playerData.adventureLevel >= playerData.maxAdventureLevel)
            {
                expSlider.maxValue = 1;
                expSlider.value = 1;

                if (expText != null)
                expText.text = "MAX";
            }
            else
            {
                int needExp = playerData.nextAdventureExp[playerData.adventureLevel - 1];

                expSlider.maxValue = needExp;
                expSlider.value = playerData.adventureExp;

                if (expText != null)
                expText.text = $"{playerData.adventureExp} / {needExp}";
            }
        }
        
    }

    

    private void OnMapSelected(MapData map)
    {
        if (mapPanel != null && mapPanel.mapCardView != null)
        mapPanel.mapCardView.Refresh(map);

        UpdateUI();
    }

    public void StartGame()
    {
        if (playerData.selectedMap != null)
        {
            SceneManager.LoadScene(playerData.selectedMap.sceneName);
        }
    }

}
