using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColoCoder : MonoBehaviour
{
    [SerializeField] private Color freezeColor = Color.blue;
        
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
        float timeScale = TimeController.GetTimeScale();

        if (timeScale == 0 & CompareTag("TimeDependent"))
            StartCoroutine(KeepFrozenColor());
        else if(timeScale != 0 & CompareTag("TimeIndependent"))
            StartCoroutine(KeepFrozenColor());
    }

    private IEnumerator KeepFrozenColor()
    {
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
    }
}
