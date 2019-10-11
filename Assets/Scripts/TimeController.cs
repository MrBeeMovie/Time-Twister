using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeController : MonoBehaviour
{
    private static float currentTimeScale;

    private void Awake()
    {
        currentTimeScale = Time.timeScale;
    }

    public static void SetTimeScale(float timeScale)
    {
        // Set the time scale to value passed
        currentTimeScale = timeScale;
    }

    public static float GetTimeScale()
    {
        return currentTimeScale;
    }
}
