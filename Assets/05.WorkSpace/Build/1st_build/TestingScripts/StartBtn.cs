using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartBtn : MonoBehaviour
{
    MapSelectPanel mapSelectPanel;

    private void Start()
    {
        mapSelectPanel = FindObjectOfType<MapSelectPanel>();
    }

    public void OnClickStartBtn()
    {
        Debug.Log("���۹�ư �۵�");
        if (mapSelectPanel == null) return;
        SceneManager.LoadScene($"Stage_0{mapSelectPanel.mapIndex+1}_Scene");
    }
}
