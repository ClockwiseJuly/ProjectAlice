using UnityEngine;
using UnityEngine.SceneManagement;

public class PersistentManager : MonoBehaviour
{
    [Header("持久化设置")]
    [SerializeField] private bool makePersistent = true;
    [SerializeField] private bool showDebugInfo = true;
    
    private static PersistentManager instance;
    
    void Awake()
    {
        // 单例模式，确保只有一个Manager实例
        if (instance == null)
        {
            instance = this;
            
            if (makePersistent)
            {
                // 使Manager及其所有子节点在场景切换时不被销毁
                DontDestroyOnLoad(gameObject);
                
                if (showDebugInfo)
                {
                    Debug.Log($"Manager对象 '{gameObject.name}' 已设置为持久化");
                    Debug.Log($"子节点数量: {transform.childCount}");
                }
            }
        }
        else if (instance != this)
        {
            // 如果已经存在实例，销毁重复的对象
            if (showDebugInfo)
            {
                Debug.Log($"销毁重复的Manager对象: {gameObject.name}");
            }
            Destroy(gameObject);
        }
    }
    
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (showDebugInfo)
        {
            Debug.Log($"场景切换到: {scene.name}，Manager对象保持持久化");
        }
    }
    
    // 公共方法：手动取消持久化
    public void RemovePersistence()
    {
        if (instance == this)
        {
            instance = null;
            SceneManager.MoveGameObjectToScene(gameObject, SceneManager.GetActiveScene());
            
            if (showDebugInfo)
            {
                Debug.Log($"Manager对象 '{gameObject.name}' 已取消持久化");
            }
        }
    }
}