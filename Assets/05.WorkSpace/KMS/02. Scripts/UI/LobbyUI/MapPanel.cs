using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MapPanel : MonoBehaviour
{
    public MapSelectPanel mapSelectPanel;
    public GameObject mapSelPanel;
    public GameObject tab;
    public GameObject[] otherPanels;
    public Image mapCardImage;
    public MapData mapCard;
    public Text mapCardText;
    public MapData currentMapData;

   

    private void OnMapSelected(MapData data)
    {
            Debug.Log($"MapPanel.OnMapSelected 호출: mapName={data.mapName}, sprite={data.mapSprite?.name}");
        if (mapCardImage == null)
            Debug.LogError("MapPanel의 mapCardImage가 Inspector에서 연결되지 않았습니다!");
        else
            mapCardImage.sprite = data.mapSprite;
            Debug.Log("MapPanel의 맵카드이미지 연결됨");
    }

    public void OnClickConfirmButton()
    {
         if (mapSelectPanel == null)
    {
        Debug.Log("MapSelectPanel이 연결되지 않았습니다");
        return;
    }

    // 현재 선택된 mapIndex로 MapData 가져오기
        MapData selectedData = mapSelectPanel.mapCard[mapSelectPanel.mapIndex];

    // MapPanel의 이미지 갱신 직접 호출
        OnMapSelected(selectedData);
    }
    
    public void UpdateMapCardUI()
    {
        if (currentMapData == null) return;

        // MapCard(MapPanel 내부) 데이터 동기화
        mapCard.mapName = currentMapData.mapName;
        mapCard.sceneName = currentMapData.sceneName;
        mapCard.mapSprite = currentMapData.mapSprite;

        // UI 갱신
        if (mapCardImage != null)
            mapCardImage.sprite = mapCard.mapSprite;
        
        if (mapCardText != null)
            mapCardText.text = mapCard.mapName;


        Debug.Log($"[MapPanel] MapCard 동기화: {mapCard.mapName}, 씬: {mapCard.sceneName}");
    
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
