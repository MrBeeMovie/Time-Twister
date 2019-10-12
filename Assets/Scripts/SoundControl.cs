using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundControl : MonoBehaviour
{
    [SerializeField] private float fadeTime = .5f, minVolume = .1f;

    private bool isWaiting = false;
    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        if (!isWaiting & TimeController.GetTimeScale() == TimeController.TIME_FROZEN)
            StartCoroutine(WaitForUnfreeze());
    }

    private IEnumerator WaitForUnfreeze()
    {
        isWaiting = true;

        float lastVolume = audioSource.volume;
        bool faded = false;

        do
        {
            if (!faded)
            {
                // Fade volume until zero then pause
                if (audioSource.volume > minVolume)
                {
                    audioSource.volume -= (lastVolume / fadeTime) * Time.unscaledDeltaTime;
                    audioSource.volume = Mathf.Max(minVolume, audioSource.volume);
                }
                else
                    faded = true;
            }
            yield return null;
        } while (TimeController.GetTimeScale() == TimeController.TIME_FROZEN);

        do
        {
            // Raise volume until we reach last volume
            audioSource.volume += (lastVolume / fadeTime) * Time.unscaledDeltaTime;
            audioSource.volume = Mathf.Min(lastVolume, audioSource.volume);
            yield return null;
        } while (audioSource.volume != lastVolume);

        isWaiting = false;
    }
}
