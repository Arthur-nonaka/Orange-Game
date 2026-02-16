using System;
using UnityEngine;

public enum SoundType
{
    FOOTSTEP,
    GRAB,
    SELL,
    BUY,
}

[RequireComponent(typeof(AudioSource))]
public class SoundManager : MonoBehaviour
{
    [SerializeField]
    private SoundList[] soundList;
    private static SoundManager _instance;
    private AudioSource audioSource;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public static void PlaySound(SoundType type)
    {
        AudioClip[] clips = _instance.soundList[(int)type].Sounds;
        AudioClip randomClip = clips[UnityEngine.Random.Range(0, clips.Length)];

        _instance.audioSource.PlayOneShot(randomClip);
    }

#if UNITY_EDITOR
    private void OnEnable()
    {
        string[] names = Enum.GetNames(typeof(SoundType));
        Array.Resize(ref soundList, names.Length);

        for (int i = 0; i < names.Length; i++)
        {
            soundList[i].name = names[i];
        }
    }
#endif
}

[Serializable]
public struct SoundList
{
    public AudioClip[] Sounds
    {
        get => soundList;
    }

    [SerializeField]
    public string name;

    [SerializeField]
    private AudioClip[] soundList;
}
