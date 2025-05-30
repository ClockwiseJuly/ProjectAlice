using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BackgroundMusicManager : MonoBehaviour
{
    [Header("背景音乐设置")]
    [SerializeField] private List<SceneMusicData> sceneMusicList = new List<SceneMusicData>();
    [SerializeField] private float fadeTime = 1.0f;
    [SerializeField] private float defaultVolume = 0.5f;
    
    [Header("调试信息")]
    [SerializeField] private bool showDebugMessages = true;
    
    private AudioSource audioSource;
    private string currentSceneName;
    private bool isTransitioning = false;
    
    // 单例模式
    public static BackgroundMusicManager Instance { get; private set; }
    
    private void Awake()
    {
        // 单例模式设置
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            
            // 获取或添加AudioSource组件
            audioSource = GetComponent<AudioSource>();
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
            
            // 设置AudioSource属性
            audioSource.loop = true;
            audioSource.playOnAwake = false;
            audioSource.volume = defaultVolume;
            
            // 监听场景切换事件
            SceneManager.sceneLoaded += OnSceneLoaded;
            
            if (showDebugMessages)
                Debug.Log("BackgroundMusicManager: 初始化完成");
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        // 播放当前场景的背景音乐
        currentSceneName = SceneManager.GetActiveScene().name;
        PlayMusicForScene(currentSceneName);
    }
    
    private void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
    
    /// <summary>
    /// 场景加载完成时调用
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        string newSceneName = scene.name;
        
        if (newSceneName != currentSceneName)
        {
            if (showDebugMessages)
                Debug.Log($"BackgroundMusicManager: 场景切换 {currentSceneName} -> {newSceneName}");
            
            currentSceneName = newSceneName;
            PlayMusicForScene(newSceneName);
        }
    }
    
    /// <summary>
    /// 为指定场景播放背景音乐
    /// </summary>
    private void PlayMusicForScene(string sceneName)
    {
        SceneMusicData musicData = GetMusicDataForScene(sceneName);
        
        if (musicData != null && musicData.backgroundMusic != null)
        {
            if (audioSource.clip != musicData.backgroundMusic)
            {
                if (showDebugMessages)
                    Debug.Log($"BackgroundMusicManager: 切换到音乐 '{musicData.backgroundMusic.name}' (场景: {sceneName})");
                
                StartCoroutine(CrossFadeMusic(musicData.backgroundMusic, musicData.volume));
            }
        }
        else
        {
            if (showDebugMessages)
                Debug.Log($"BackgroundMusicManager: 场景 '{sceneName}' 没有设置背景音乐");
            
            // 如果没有找到对应音乐，淡出当前音乐
            if (audioSource.isPlaying)
            {
                StartCoroutine(FadeOutMusic());
            }
        }
    }
    
    /// <summary>
    /// 获取指定场景的音乐数据
    /// </summary>
    private SceneMusicData GetMusicDataForScene(string sceneName)
    {
        foreach (SceneMusicData data in sceneMusicList)
        {
            if (data.sceneName.Equals(sceneName, System.StringComparison.OrdinalIgnoreCase))
            {
                return data;
            }
        }
        return null;
    }
    
    /// <summary>
    /// 交叉淡入淡出切换音乐
    /// </summary>
    private IEnumerator CrossFadeMusic(AudioClip newClip, float targetVolume)
    {
        if (isTransitioning) yield break;
        isTransitioning = true;
        
        float originalVolume = audioSource.volume;
        
        // 淡出当前音乐
        if (audioSource.isPlaying)
        {
            float fadeOutTime = fadeTime * 0.5f;
            for (float t = 0; t < fadeOutTime; t += Time.deltaTime)
            {
                audioSource.volume = Mathf.Lerp(originalVolume, 0, t / fadeOutTime);
                yield return null;
            }
            audioSource.Stop();
        }
        
        // 切换音乐
        audioSource.clip = newClip;
        audioSource.volume = 0;
        audioSource.Play();
        
        // 淡入新音乐
        float fadeInTime = fadeTime * 0.5f;
        for (float t = 0; t < fadeInTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(0, targetVolume, t / fadeInTime);
            yield return null;
        }
        
        audioSource.volume = targetVolume;
        isTransitioning = false;
    }
    
    /// <summary>
    /// 淡出音乐
    /// </summary>
    private IEnumerator FadeOutMusic()
    {
        if (isTransitioning) yield break;
        isTransitioning = true;
        
        float originalVolume = audioSource.volume;
        
        for (float t = 0; t < fadeTime; t += Time.deltaTime)
        {
            audioSource.volume = Mathf.Lerp(originalVolume, 0, t / fadeTime);
            yield return null;
        }
        
        audioSource.Stop();
        audioSource.volume = defaultVolume;
        isTransitioning = false;
    }
    
    /// <summary>
    /// 外部接口：设置音量
    /// </summary>
    public void SetVolume(float volume)
    {
        defaultVolume = Mathf.Clamp01(volume);
        if (!isTransitioning)
        {
            audioSource.volume = defaultVolume;
        }
    }
    
    /// <summary>
    /// 外部接口：暂停音乐
    /// </summary>
    public void PauseMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
    }
    
    /// <summary>
    /// 外部接口：恢复音乐
    /// </summary>
    public void ResumeMusic()
    {
        if (!audioSource.isPlaying && audioSource.clip != null)
        {
            audioSource.UnPause();
        }
    }
    
    /// <summary>
    /// 外部接口：停止音乐
    /// </summary>
    public void StopMusic()
    {
        StartCoroutine(FadeOutMusic());
    }
    
    /// <summary>
    /// 外部接口：手动播放指定场景的音乐
    /// </summary>
    public void PlayMusicForSceneManually(string sceneName)
    {
        PlayMusicForScene(sceneName);
    }
}

/// <summary>
/// 场景音乐数据结构
/// </summary>
[System.Serializable]
public class SceneMusicData
{
    [Header("场景设置")]
    public string sceneName;           // 场景名称
    
    [Header("音乐设置")]
    public AudioClip backgroundMusic;  // 背景音乐
    [Range(0f, 1f)]
    public float volume = 0.5f;        // 音量
    
    [Header("描述")]
    [TextArea(2, 3)]
    public string description;         // 描述信息
}