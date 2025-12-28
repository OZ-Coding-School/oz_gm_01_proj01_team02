using UnityEngine;
using UnityEngine.SceneManagement;

public class TextClickButton : MonoBehaviour
{
    public MapCardView mapCard;  // 버튼이 속한 맵카드의 MapData

    public void OnClickMapCard()
    {
        if (mapCard == null)
        {
            Debug.LogError("MapData가 연결되지 않았습니다!");
            return;
        }
        // 씬 이동
        SceneManager.LoadScene(mapCard.data.sceneName);
    }
}