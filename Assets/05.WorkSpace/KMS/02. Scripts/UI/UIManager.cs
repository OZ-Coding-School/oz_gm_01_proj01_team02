using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{   
    public static UIManager Instance;

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
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Canvas canvas = null;
        Canvas[] canvases = FindObjectsOfType<Canvas>();
        foreach (var c in canvases)
        {
            if (c.gameObject.name == "Canvas")
            {
                canvas = c;
                break;
            }
        }

        if (canvas == null) return;

        hpBar = canvas.GetComponent<SegmentedHpBar>();
        slotMachine = canvas.transform.Find("SlotMachineManager/SlotMachineUI")?.GetComponent<SlotMachineManager>();
        pauseUI = canvas.transform.Find("PauseUI")?.gameObject;
        pauseButton = canvas.transform.Find("StopButton")?.GetComponent<Button>();
        
        Button[] buttons = pauseUI.GetComponentsInChildren<Button>(true);
        foreach (var btn in buttons)
        {
            if (btn.name == "ResumeButton") resumeButton = btn;
            if (btn.name == "HomeButton") homeButton = btn;
        }

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
        SceneManager.LoadScene("TitleScene(Build)");
    }

    
  
}
