using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;

public class MapPanel : MonoBehaviour
{
    [Header("UI References")]
    public MapSelectPanel mapSelectPanel;
    public GameObject mapSelPanel;
    public GameObject tab;
    public GameObject[] otherPanels;

    [Header("MapCard UI")]
    public MapCardView mapCardView;

    [Header("PlayerData")]
    public PlayerData playerData;
    public MapData selectedData;

    

   
    private void Awake()
    {
        if (playerData != null)
        {
            playerData.OnSelectedMapChanged += OnMapSelected;

            if (playerData.selectedMap != null)
            {
                OnMapSelected(playerData.selectedMap);
            }   
        }
    }
    
    public void OnDestroy()
    {
        if (playerData != null)
        playerData.OnSelectedMapChanged -= OnMapSelected;
    }

    private void OnMapSelected(MapData data)
    {
        if (mapCardView != null)
        {
            mapCardView.Set(data);
        }
    }

    public void OnClickConfirmButton()
    {
         if (mapSelectPanel == null) 
    {
        Debug.Log("MapSelectPanel이 연결되지 않았습니다");
        return;
    }

    // 현재 선택된 mapIndex로 MapData 가져오기
        selectedData = mapSelectPanel.mapCard[mapSelectPanel.mapIndex].data;
        
        if (playerData != null)
        {
            playerData.SetSelectedMap(selectedData);
        }

        if (mapCardView != null)
        {
            mapCardView.Set(selectedData);
            Debug.Log($"[MapPanel] Selected Index: {mapSelectPanel.mapIndex}, MapCardView data: {mapCardView.GetData().mapName}");
        }
    }
    

    public void OpenMapSelectPanel()
    {

        foreach (var panel in otherPanels)
        panel.SetActive(false);

        mapSelPanel.SetActive(true);
        tab.SetActive(false);
    }

    public void GoGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }

   
}
