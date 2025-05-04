using UnityEngine;

public class VictoryScreen : MonoBehaviour
{
    [SerializeField] VoidEventChannel levelclearedEventChannel;// ������Ƶ��

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
        GetComponent<Canvas>().enabled = true; // ���û���
        GetComponent<Animator>().enabled = true; // ���Ŷ���

    }
}
