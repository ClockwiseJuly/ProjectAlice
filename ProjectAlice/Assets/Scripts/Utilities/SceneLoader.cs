using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader
{
    public static void LoadScene(string sceneName)
    {
        int sceneIndex = SceneManager.GetActiveScene().buildIndex;

        SceneManager.LoadScene(sceneBuildIndex: sceneIndex);
    }

    public static void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}

//失败场景的
// void  OnEnable()和void  OnDisable()
// {
//     retryButton.onClick.AddListener(() => SceneLoader.LoadScene("ReloadScene"));
//     quitButton.onClick.AddListener(() => SceneLoader.QuitGame());
// }
