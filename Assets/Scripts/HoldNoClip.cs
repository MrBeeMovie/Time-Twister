using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HoldNoClip : MonoBehaviour
{
    [SerializeField] private const float MIN_WEIGHT = .1f;

    private ParentConstraint parentConstraint;

    private const float FULL_WEIGHT = 1f, ZERO_WEIGHT = 0f;

    private void Awake()
    {
        parentConstraint = GetComponent<ParentConstraint>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // set the translation at rest 
        parentConstraint.translationAtRest = Camera.main.transform.InverseTransformPoint(transform.position);
    }

    private void OnCollisionStay(Collision collision)
    {
        // set weight to zero when colliding with a different object
        parentConstraint.weight = ZERO_WEIGHT;
    }

    private void OnCollisionExit(Collision collision)
    {
        // set weight to full when no longer colliding
        parentConstraint.weight = FULL_WEIGHT;
    }
}
