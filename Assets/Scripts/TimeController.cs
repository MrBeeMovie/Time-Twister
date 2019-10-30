using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    public const float TIME_DEFAULT = 1f;
    public const float TIME_FROZEN = 0f;
    private static float currentTimeScale = TIME_DEFAULT;

    public static void SetTimeScale(float timeScale)
    {
        currentTimeScale = timeScale;
    }

    public static float GetTimeScale()
    {
        return currentTimeScale;
    }
}
