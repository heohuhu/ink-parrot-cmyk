using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Audio Database")]
    [SerializeField]
    private List<AudioData> audioList = new();

    private Dictionary<string, AudioData> audioDict = new();

    [Header("Audio Sources")]
    [SerializeField]
    private AudioSource bgmSource;

    [SerializeField]
    private AudioSource subbgmSource;

    [SerializeField]
    private AudioSource sfxSource;

    [SerializeField]
    private AudioSource uiSource;

    private float masterVolume = 1f;
    private float bgmVolume = 1f;
    private float sfxVolume = 1f;
    private float uiVolume = 1f;
    private List<string> bgmPlaylist = new();

    private int currentBGMIndex = 0;

    private bool isBGMPaused = false;

    private bool usePlaylist = false;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        Initialize();
        PlayBGMPlaylist(
        new List<string>()
        {
            "기본브금1",
            "기본브금2",
            "기본브금3"
        });

        PlaySubBGM("백그라운드새소리");
    }

    private void Update()
    {
        CheckBGMPlaylist();
    }


    private void Initialize()
    {
        audioDict.Clear();

        foreach (AudioData data in audioList)
        {
            if (data == null || data.clip == null)
                continue;

            if (audioDict.ContainsKey(data.key))
            {
                Debug.LogWarning($"Duplicate Audio Key : {data.key}");
                continue;
            }

            audioDict.Add(data.key, data);
        }

        ApplyVolumeSettings();
    }

    /// <summary>
    /// SettingManager 값 변경 시 호출
    /// </summary>
    public void ApplyVolumeSettings()
    {
        masterVolume = NormalizeVolume(
            SettingManager.Instance.setting.sound.Master);

        sfxVolume = NormalizeVolume(
            SettingManager.Instance.setting.sound.SFX);

        bgmVolume = NormalizeVolume(
            SettingManager.Instance.setting.sound.BGM);

        uiVolume = NormalizeVolume(
            SettingManager.Instance.setting.sound.UI);

        UpdateSourceVolumes();
    }

    private void UpdateSourceVolumes()
    {
        bgmSource.volume = masterVolume * bgmVolume;
        sfxSource.volume = masterVolume * sfxVolume;
        uiSource.volume = masterVolume * uiVolume;
        subbgmSource.volume = masterVolume * bgmVolume;
    }

    private float NormalizeVolume(int value)
    {
        return Mathf.Clamp01(value / 100f);
    }

    public void Play(string key)
    {
        if (!audioDict.TryGetValue(key, out AudioData data))
        {
            Debug.LogWarning($"Audio Key Not Found : {key}");
            return;
        }

        switch (data.type)
        {
            case AudioType.BGM:
                PlayBGM(data);
                break;

            case AudioType.SFX:
                PlaySFX(data);
                break;

            case AudioType.UI:
                PlayUI(data);
                break;
        }
    }

    #region BGM

    public void PlayBGM(string key)
    {
        if (!audioDict.TryGetValue(key, out AudioData data))
        {
            Debug.LogWarning($"Audio Key Not Found : {key}");
            return;
        }

        PlayBGM(data);
    }

    private void PlayBGM(AudioData data)
    {
        if (bgmSource.clip == data.clip && bgmSource.isPlaying)
            return;

        bgmSource.Stop();

        bgmSource.clip = data.clip;
        bgmSource.loop = data.loop;

        bgmSource.volume =
            masterVolume *
            bgmVolume *
            data.defaultVolume;

        bgmSource.Play();
    }

    public void StopALLBGM()
    {
        bgmSource.Stop();
        subbgmSource.Stop();

        isBGMPaused = false;

        usePlaylist = false;
    }
    public void PauseALLBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Pause();

            isBGMPaused = true;
        }

        if (subbgmSource.isPlaying)
        {
            subbgmSource.Pause();
        }
    }
    public void ResumeALLBGM()
    {
        if (bgmSource.clip != null)
        {
            bgmSource.UnPause();

            isBGMPaused = false;
        }

        if (subbgmSource.clip != null)
        {
            subbgmSource.UnPause();
        }
    }
    public void StopBGM()
    {
        bgmSource.Stop();

        isBGMPaused = false;

        usePlaylist = false;
    }

    public void StopSUBBGM()
    {
        subbgmSource.Stop();
    }


    public void PauseBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Pause();

            isBGMPaused = true;
        }
    }
    
    public void PauseSUBBGM()
    {
        if (bgmSource.isPlaying)
        {
            bgmSource.Pause();

            isBGMPaused = true;
        }

        if (subbgmSource.isPlaying)
        {
            subbgmSource.Pause();
        }
    }

    public void ResumeBGM()
    {
        if (bgmSource.clip != null)
        {
            bgmSource.UnPause();

            isBGMPaused = false;
        }
    }

    public void ResumeSUBBGM()
    {
        if (subbgmSource.clip != null)
        {
            subbgmSource.UnPause();
        }
    }


    public void PlayBGMPlaylist(List<string> playlist)
    {
        if (playlist == null || playlist.Count == 0)
            return;

        bgmPlaylist = playlist;

        currentBGMIndex = 0;

        usePlaylist = true;

        PlayBGM(bgmPlaylist[currentBGMIndex]);
    }

    private void CheckBGMPlaylist()
    {
        if (!usePlaylist)
            return;

        if (isBGMPaused)
            return;

        if (bgmSource.clip == null)
            return;

        if (bgmSource.isPlaying)
            return;

        PlayNextBGM();
    }

    private void PlayNextBGM()
    {
        currentBGMIndex++;

        if (currentBGMIndex >= bgmPlaylist.Count)
        {
            currentBGMIndex = 0;
        }

        PlayBGM(bgmPlaylist[currentBGMIndex]);
    }

    public void PlaySubBGM(string key)
    {
        if (!audioDict.TryGetValue(key, out AudioData data))
        {
            Debug.LogWarning($"Audio Key Not Found : {key}");
            return;
        }

        if (data.clip == null)
            return;

        if (subbgmSource.clip == data.clip &&
            subbgmSource.isPlaying)
        {
            return;
        }

        subbgmSource.Stop();

        subbgmSource.clip = data.clip;

        subbgmSource.loop = data.loop;

        subbgmSource.volume =
            masterVolume *
            bgmVolume *
            data.defaultVolume;

        subbgmSource.Play();
    }

    public bool IsSubBGMPlaying()
    {
        return subbgmSource.isPlaying;
    }



    #endregion

    #region SFX

    public void PlaySFX(string key)
    {
        if (!audioDict.TryGetValue(key, out AudioData data))
        {
            Debug.LogWarning($"Audio Key Not Found : {key}");
            return;
        }

        PlaySFX(data);
    }

    private void PlaySFX(AudioData data)
    {
        sfxSource.PlayOneShot(
            data.clip,
            masterVolume *
            sfxVolume *
            data.defaultVolume);
    }

    #endregion

    #region UI

    public void PlayUI(string key)
    {
        if (!audioDict.TryGetValue(key, out AudioData data))
        {
            Debug.LogWarning($"Audio Key Not Found : {key}");
            return;
        }

        PlayUI(data);
    }

    private void PlayUI(AudioData data)
    {
        uiSource.PlayOneShot(
            data.clip,
            masterVolume *
            uiVolume *
            data.defaultVolume);
    }

    #endregion
}

public enum AudioType
{
    SFX,
    BGM,
    UI
}

[System.Serializable]
public class AudioData
{
    public string key;

    public AudioType type;

    public AudioClip clip;

    [Range(0f, 1f)]
    public float defaultVolume = 1f;

    public bool loop = false;
}