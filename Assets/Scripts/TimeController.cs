using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private static float currentTimeScale;
    public const float TIME_DEFAULT = 1f;
    public const float TIME_FROZEN = 0f;

    private void Awake()
    {
        // Set timeScale = to the default timeScale set by Unity
        currentTimeScale = Time.timeScale;
    }

    public static void SetTimeScale(float timeScale)
    {
        currentTimeScale = timeScale;
    }

    public static float GetTimeScale()
    {
        return currentTimeScale;
    }
}
