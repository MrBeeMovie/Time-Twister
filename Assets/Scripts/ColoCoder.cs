using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WARNING DO NOT ATTACH TO OBJECTS WHICH ARE TIME AVERSE WILL DECREASE GAME PERFORMANCE

public class ColoCoder : MonoBehaviour
{
    [SerializeField] private Color freezeColor = Color.blue;

    private bool isWaiting = false;
    private Color previousColor;
    private Material material;

    private void Awake()
    {
        material = GetComponent<Renderer>().material;
        previousColor = material.color;
    }

    private void Update()
    {
        CheckTimeScale();
    }

    private void CheckTimeScale()
    {
        if (!isWaiting)
        {
            float timeScale = TimeController.GetTimeScale();

            if (timeScale == TimeController.TIME_FROZEN & CompareTag("TimeDependent"))
                StartCoroutine(KeepFrozenColor());
            else if (timeScale == TimeController.TIME_DEFAULT & CompareTag("TimeIndependent"))
                StartCoroutine(KeepFrozenColor());
        }
    }

    private IEnumerator KeepFrozenColor()
    {
        isWaiting = true;

        material.SetColor("_Color", freezeColor);

        if(CompareTag("TimeDependent"))
        {
            do
            {
                yield return null;
            } while (TimeController.GetTimeScale() == 0);
        }

        else
        {
            do
            {
                yield return null;
            } while (TimeController.GetTimeScale() != 0);
        }

        material.SetColor("_Color", previousColor);
        isWaiting = false;
    }
}
