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
    public Text levelText;
    public MapPanel mapPanel;

    void Start()
    {
 
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

    private void UpdateUI()
    {
        if (coinText != null)
        coinText.text = playerData.totalCoin.ToString();
        
        if (levelText != null)
        levelText.text = "Level " + playerData.level;
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
