using UnityEngine;

public class SoundEffectPlayer : MonoBehaviour
{
    public static AudioSource audioSource { get; private set; }

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.playOnAwake = false;
    }
}
