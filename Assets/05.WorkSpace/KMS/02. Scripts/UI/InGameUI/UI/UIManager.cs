using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{   

    [Header("UI References")]
    public SegmentedHpBar hpBar;
    public SlotMachineManager slotMachine;
    public GameObject pauseUI;

    [Header("Buttons")]
    public Button pauseButton;
    public Button resumeButton;
    public Button homeButton;

    private void Awake()
    {
        

        ConnectButtonEvents();
        
    }

   

    private void ConnectButtonEvents()
    {
        if (pauseButton != null) pauseButton.onClick.AddListener(TogglePause);
        if (resumeButton != null) resumeButton.onClick.AddListener(ResumeGame);
        if (homeButton != null) homeButton.onClick.AddListener(GoHome);
    }

    private void TogglePause()
    {
        if (pauseUI != null)
        pauseUI.SetActive(!pauseUI.activeSelf);

        Time.timeScale = pauseUI.activeSelf ? 0 : 1;
    }

    private void ResumeGame()
    {
        if (pauseUI != null) pauseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }

    private void GoHome()
    {
        Time.timeScale = 1.0f;
        SceneManager.LoadScene("TitleScene(kms)");
    }

    
  
}
