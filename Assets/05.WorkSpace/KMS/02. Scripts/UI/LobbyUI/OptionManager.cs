using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionManager : MonoBehaviour
{
    public static OptionManager Instance;

    [Header("Audio Source")]
    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("UI References")]
    public Slider bgmSlider;
    public Slider sfxSlider;


    [Header("Volume Settings")]
    [Range(0f, 1f)] public float bgmVolume;
    [Range(0f, 1f)] public float sfxVolume;
    public bool isBgmMuted = false;
    public bool isSfxMuted = false;
    private float prevBgmVolume = 1.0f;
    private float prevSfxVolume = 1.0f;


    private void Awake()
    {

        
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded;

            bgmSource.volume = bgmVolume;
            sfxSource.volume = sfxVolume;
            
        }

        else
        {
            Destroy(gameObject);
        }
       
    }

    private void OnDestroy()
    {
        if (Instance == this)
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(UpdateSlidersNextFrame());
    }
    private IEnumerator UpdateSlidersNextFrame()
    {

        yield return null;
        


        Slider[] sliders = FindObjectsOfType<Slider>();
        foreach (var s in sliders)
        {
            if (s.name == "MusicVolumeSlider")
                bgmSlider = s;
            else if (s.name == "SoundEffectSlider")
                sfxSlider = s;
            else
            {
                
            }
        }

        if (bgmSlider != null)
        {
            bgmSlider.onValueChanged.RemoveAllListeners();
            bgmSlider.onValueChanged.AddListener(OnBGMSliderChanged);
            bgmSlider.value = isBgmMuted ? 0f : bgmVolume; 
            
            

        
        }

        if (sfxSlider != null)
        {
            sfxSlider.onValueChanged.RemoveAllListeners();
            sfxSlider.onValueChanged.AddListener(OnSFXSliderChanged);
            sfxSlider.value = isSfxMuted ? 0f : sfxVolume;
            
            
        }

        

    }


    private void OnBGMSliderChanged(float value)
    {
        SetBgmVolume(value);
    }

    private void OnSFXSliderChanged(float value)
    {
        SetSfxVolume(value);
    }

    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;
        bgmSource.volume = bgmVolume;

        if (volume > 0f)
            isBgmMuted = false;
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;
        sfxSource.volume = sfxVolume;

        if (volume > 0f)
            isSfxMuted = false;
    }

    public void UpdateSliders()
    {
        if (bgmSlider != null)
        bgmSlider.value = isBgmMuted ? 0f : bgmVolume;

        if (sfxSlider != null)
        sfxSlider.value = isSfxMuted ? 0f : sfxVolume;
    }
    public void PlaySFX(AudioClip clip)
    {
        sfxSource.PlayOneShot(clip, sfxVolume);
    }

    public void ToggleBgmMute()
    {
        isBgmMuted = !isBgmMuted;

        if (isBgmMuted)
        {
            prevBgmVolume = bgmVolume;
            SetBgmVolume(0.0f);
            if (bgmSlider != null)
            bgmSlider.value = 0.0f;
        }

        else
        {
            SetBgmVolume(prevBgmVolume);
            if (bgmSlider != null)
            bgmSlider.value = prevBgmVolume;
        }
    }

    public void ToggleSfxMute()
    {
        isSfxMuted = !isSfxMuted;

        if (isSfxMuted)
        {
            prevSfxVolume = sfxVolume;
            SetSfxVolume(0.0f);
            if (sfxSlider != null)
            sfxSlider.value = 0.0f;
        }

        else
        {
            SetSfxVolume(prevSfxVolume);
            if (sfxSlider != null)
            sfxSlider.value = prevSfxVolume;
        }
    }

    private void ApplyAudio()
    {
        bgmSource.volume = isBgmMuted ? 0f : bgmVolume;
        sfxSource.volume = isSfxMuted ? 0f : sfxVolume;
    }
}
