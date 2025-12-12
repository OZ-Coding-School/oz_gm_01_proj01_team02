using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPanelManager : MonoBehaviour
{
    public GameObject mapSelectPanel;
    public GameObject[] otherPanels;


    public void OpenMapSelectPanel()
    {

        foreach (var panel in otherPanels)
        panel.SetActive(false);

        mapSelectPanel.SetActive(true);
    }

   
}
