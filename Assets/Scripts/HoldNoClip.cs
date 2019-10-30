using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HoldNoClip : MonoBehaviour
{
    [SerializeField] private float reducedWeight = .8f;
    [SerializeField] private new Transform camera = null;

    private ParentConstraint parentConstraint;
    private Vector3 translationOffset, rotationOffset;

    private const float FULL_WEIGHT = 1f;

    private void Awake()
    {
        parentConstraint = GetComponent<ParentConstraint>();
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        parentConstraint.constraintActive = false;
    }

    private void OnCollisionStay(Collision collision)
    {
    }

    private void OnCollisionExit(Collision collision)
    {
        parentConstraint.weight = FULL_WEIGHT;
    }
}
