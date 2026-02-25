using DG.Tweening;
using UnityEngine;
using System;

public class OpenFence : MonoBehaviour
{
    [SerializeField]
    private float duration = 1f;

    public void Open(Action onComplete = null)
    {
        transform.DORotate(new Vector3(0, 0, 0), duration)
            .SetEase(Ease.Linear)
            .OnComplete(() => onComplete?.Invoke());
        SoundManager.PlaySound(SoundType.DOOR, 0.5f);
    }

    public void Close()
    {
        transform.DORotate(new Vector3(0, 90, 0), duration).SetEase(Ease.Linear);
        SoundManager.PlaySound(SoundType.DOOR, 0.5f);
    }
}
