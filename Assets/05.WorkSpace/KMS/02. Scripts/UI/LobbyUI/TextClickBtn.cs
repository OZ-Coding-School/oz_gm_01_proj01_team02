using UnityEngine;
using UnityEngine.SceneManagement;

public class TextClickButton : MonoBehaviour
{
    public MapData mapData;  // 버튼이 속한 맵카드의 MapData

    public void OnClickMapCard()
    {
        if (mapData == null)
        {
            Debug.LogError("MapData가 연결되지 않았습니다!");
            return;
        }
        GameManager.Pool.ClearPool();
        // 씬 이동
        SceneManager.LoadScene(mapData.sceneName);
    }
}