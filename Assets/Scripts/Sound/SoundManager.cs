using System;
using UnityEngine;

public enum SoundType
{
    FOOTSTEP,
    GRAB,
    SELL,
    BUY,
    CLICK,
    DOOR,
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

    public static void PlaySound(SoundType type, float volume = 1f)
    {
        Audio[] clips = _instance.soundList[(int)type].Sounds;
        AudioClip randomClip = GetRandomClip(clips);

        _instance.audioSource.PlayOneShot(randomClip, volume);
    }

    private static AudioClip GetRandomClip(Audio[] clips)
    {
        float totalWeight = 0f;
        foreach (Audio audio in clips)
        {
            totalWeight += audio.chance;
        }

        float randomValue = UnityEngine.Random.Range(0f, totalWeight);

        float cumulativeWeight = 0f;
        foreach (Audio audio in clips)
        {
            cumulativeWeight += audio.chance;
            if (randomValue <= cumulativeWeight)
            {
                return audio.sound;
            }
        }

        return clips[0].sound;
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
    public Audio[] Sounds
    {
        get => soundList;
    }

    [SerializeField]
    public string name;

    [SerializeField]
    private Audio[] soundList;
}

[Serializable]
public struct Audio
{
    public AudioClip sound;
    public float chance;
}
