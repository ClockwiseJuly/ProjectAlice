using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] VoidEventChannel levelclearedEventChannel;// 监听此频道

    void OnEnable()
    {
        levelclearedEventChannel.AddListener(action: ShowUI);
    }

    void OnDisable()
    {
        levelclearedEventChannel.RemoveListener(action: ShowUI);
    }

    void ShowUI()
    {
        GetComponent<Canvas>().enabled = true; // 启用画布
        GetComponent<Animator>().enabled = true; // 播放动画

    }
}
