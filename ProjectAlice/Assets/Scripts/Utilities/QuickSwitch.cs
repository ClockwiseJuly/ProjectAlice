using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;

public class QuickSwitch : MonoBehaviour
{
    [Header("场景名称设置")]
    [SerializeField] private string level1SceneName = "Level1";
    [SerializeField] private string level2SceneName = "Level2";
    [SerializeField] private string level3SceneName = "Level3";

    [Header("调试信息")]
    [SerializeField] private bool showDebugInfo = true;

    private void Update()
    {
        // 检测数字键1、2、3
        if (Keyboard.current.digit1Key.wasPressedThisFrame || Keyboard.current.numpad1Key.wasPressedThisFrame)
        {
            SwitchToScene(level1SceneName, "Level 1");
        }
        else if (Keyboard.current.digit2Key.wasPressedThisFrame || Keyboard.current.numpad2Key.wasPressedThisFrame)
        {
            SwitchToScene(level2SceneName, "Level 2");
        }
        else if (Keyboard.current.digit3Key.wasPressedThisFrame || Keyboard.current.numpad3Key.wasPressedThisFrame)
        {
            SwitchToScene(level3SceneName, "Level 3");
        }
    }

    private void SwitchToScene(string sceneName, string displayName)
    {
        // 检查场景是否存在于Build Settings中
        if (IsSceneInBuildSettings(sceneName))
        {
            if (showDebugInfo)
            {
                Debug.Log($"正在切换到 {displayName} ({sceneName})");
            }

            SceneManager.LoadScene(sceneName);
        }
        else
        {
            Debug.LogWarning($"场景 '{sceneName}' 未在Build Settings中找到！请确保场景已添加到Build Settings中。");
        }
    }

    private bool IsSceneInBuildSettings(string sceneName)
    {
        for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            string scenePath = SceneUtility.GetScenePathByBuildIndex(i);
            string sceneNameFromPath = System.IO.Path.GetFileNameWithoutExtension(scenePath);

            if (sceneNameFromPath.Equals(sceneName, System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    private void OnEnable()
    {
        if (showDebugInfo)
        {
            Debug.Log("QuickSwitch已启用 - 按键说明:");
            Debug.Log("1 -> Level 1");
            Debug.Log("2 -> Level 2");
            Debug.Log("3 -> Level 3");
        }
    }
}