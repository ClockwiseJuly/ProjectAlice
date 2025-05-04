using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] VoidEventChannel levelclearedEventChannel;

    void Enable()
    {
        levelclearedEventChannel.AddListener(action: ShowUI);
    }

    void Disable()
    {
        levelclearedEventChannel.RemoveListener(action: ShowUI);
    }

    void ShowUI()
    {
        GetComponent<Canvas>().enabled = true; // 启用画布
        GetComponent<Animator>().enabled = true; // 播放动画

    }
}
