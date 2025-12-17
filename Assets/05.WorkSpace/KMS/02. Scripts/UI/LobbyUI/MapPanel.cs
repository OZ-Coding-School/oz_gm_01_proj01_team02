using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapPanel : MonoBehaviour
{
    public GameObject mapSelectPanel;
    public GameObject tab;
    public GameObject[] otherPanels;


    public void OpenMapSelectPanel()
    {

        foreach (var panel in otherPanels)
        panel.SetActive(false);

        mapSelectPanel.SetActive(true);
        tab.SetActive(false);
    }

   
}
