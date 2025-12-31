using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }

    [Header("Sound Data")]
    [SerializeField] private SoundData soundData;

    private AudioSource bgmSource;          // BGM용 AudioSource
    private AudioSource sfxSource;          // 효과음용 AudioSource

    private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();
    private Dictionary<string, SoundData.Sound> sfxDictionary = new Dictionary<string, SoundData.Sound>();

    public float bgmVolume;
    public float sfxVolume;
    public bool isBgmMuted = false;
    public bool isSfxMuted = false;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize(soundData);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// SoundData를 주입받아 초기화
    /// </summary>
    public void Initialize(SoundData data)
    {
        if (data == null)
        {
            Debug.LogError("SoundData가 null입니다!");
            return;
        }

        soundData = data;
        InitializeAudioSources();
        InitializeDictionaries();

        Debug.Log($"SoundManager 초기화 완료: BGM {soundData.bgmSounds.Length}개, SFX {soundData.sfxSounds.Length}개");
    }

    /// <summary>
    /// AudioSource 초기화
    /// </summary>
    private void InitializeAudioSources()
    {
        if (soundData == null)
        {
            Debug.LogError("SoundData가 할당되지 않았습니다!");
            return;
        }

        bgmVolume = soundData.defaultBGMVolume;
        sfxVolume = soundData.defaultSFXVolume;

        // BGM 전용 AudioSource (없을 때만 추가)
        if (bgmSource == null)
        {
            bgmSource = gameObject.AddComponent<AudioSource>();
            bgmSource.loop = true;
            bgmSource.playOnAwake = false;
        }
        bgmSource.volume = bgmVolume; // 볼륨 설정은 매번 해도 OK
        bgmSource.pitch = 1f; // 피치 초기화

        // 효과음 전용 AudioSource (없을 때만 추가)

        if (sfxSource == null)
        {
            sfxSource = gameObject.AddComponent<AudioSource>();
            sfxSource.loop = false;
            sfxSource.playOnAwake = false;
        }
        sfxSource.volume = sfxVolume; // 볼륨 설정은 매번 해도 OK
        sfxSource.pitch = 1f; // 피치 초기화

    }

    /// <summary>
    /// 사운드 Dictionary 초기화
    /// </summary>
    private void InitializeDictionaries()
    {
        if (soundData == null) return;

        // BGM Dictionary
        foreach (SoundData.Sound sound in soundData.bgmSounds)
        {
            if (sound.clip != null && !bgmDictionary.ContainsKey(sound.name))
            {
                bgmDictionary.Add(sound.name, sound.clip);
            }
        }

        // SFX Dictionary
        foreach (SoundData.Sound sound in soundData.sfxSounds)
        {
            if (sound.clip != null && !sfxDictionary.ContainsKey(sound.name))
            {
                sfxDictionary.Add(sound.name, sound);
            }
        }
    }

    /// <summary>
    /// 효과음 재생
    /// </summary>
    public void Play(string soundName)
    {
        if (sfxDictionary.TryGetValue(soundName, out SoundData.Sound sound))
        {
            sfxSource.PlayOneShot(sound.clip, sound.volume * sfxVolume);
        }
        else
        {
            Debug.LogWarning($"효과음 '{soundName}'을 찾을 수 없습니다!");
        }
    }

    public void Stop()
    {
        sfxSource.Stop();
    }

    public void SetBgmVolume(float volume)
    {
        bgmVolume = volume;

        if (volume > 0f)
            isBgmMuted = false;
        else
            isBgmMuted = true;
    }

    public void SetSfxVolume(float volume)
    {
        sfxVolume = volume;

        if (volume > 0f)
            isSfxMuted = false;
        else
            isSfxMuted = true;
    }
}

