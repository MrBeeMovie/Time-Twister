using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class HoldNoClip : MonoBehaviour
{
    [SerializeField] private float weightStep = .1f, minWeight = .1f, moveDelay = 1;

    private ParentConstraint parentConstraint;
    private Vector3 translationOffset, rotationOffset;
    private bool scaleWeight = false;
    private float timeElapsed = 0f;

    private const float FULL_WEIGHT = 1f;

    private void Awake()
    {
        parentConstraint = GetComponent<ParentConstraint>();
    }

    private void Update()
    {
        // scale weight back up
        if(scaleWeight)
            parentConstraint.weight = Mathf.Min(parentConstraint.weight + weightStep, 1f);
    }

    private void OnCollisionEnter(Collision collision)
    {
        // set elapsed time equal to zero
        timeElapsed = 0;

        // set scaleWeight to false
        scaleWeight = false;
    }

    private void OnCollisionStay(Collision collision)
    {
        // ignore collisions between the holdable and player
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Holdable"));

        // record elapsedTime
        timeElapsed += Time.deltaTime;
        
        // if the elapsed time is greater than or equal to the move delay
        if (timeElapsed >= moveDelay)
        {
            // set rest position of parent constraint
            parentConstraint.translationAtRest = parentConstraint.GetSource(0).sourceTransform.TransformPoint(Vector3.forward * .5f);

            // scale down parent constraint until zero
            if (parentConstraint.weight != minWeight)
                parentConstraint.weight = Mathf.Max(parentConstraint.weight - weightStep, minWeight);
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        // reenable collisions between the holdable and player
        Physics.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Holdable"), true);

        // set scaleWeight back to false
        scaleWeight = true;
    }
}
