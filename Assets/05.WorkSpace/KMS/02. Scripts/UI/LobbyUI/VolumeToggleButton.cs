using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class VolumeToggleButton : MonoBehaviour
{
    public bool isBgmButton = true;    
    public GameObject muteXImage;

    private void Start()
    {
        if (OptionManager.Instance == null) return;

        RefreshUI();

    }

    public void OnClick()
    {
        if (isBgmButton)
            OptionManager.Instance.ToggleBgmMute();
        else
            OptionManager.Instance.ToggleSfxMute();

        RefreshUI();
    }

    private void RefreshUI()
    {
        if (isBgmButton)
            muteXImage.SetActive(OptionManager.Instance.isBgmMuted);
        else  
            muteXImage.SetActive(OptionManager.Instance.isSfxMuted);
    }

    

    
}
