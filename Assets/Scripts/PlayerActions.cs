using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private string leftClickName = "Fire1", holdableLayer = "";   
    [SerializeField] private KeyCode freezeKey = KeyCode.E, unfreezeKey = KeyCode.Q;
    [SerializeField] private float pickupDistance = 5, throwDistance = 10;

    private bool tookAction = false, isHolding = false;

    private void Update()
    {
        PlayerAction();
    }

    private void PlayerAction()
    {
        ActionInput();
        FreezeTimeInput();
    }

    private void ActionInput()
    {
        if (Input.GetButtonDown(leftClickName))
        {
            if (!isHolding)
            {
                StartCoroutine(HoldItem());
            }
        }
    }

    IEnumerator HoldItem()
    {
        isHolding = true;

        // Create layer mask for holdable objects
        int layermask = LayerMask.GetMask(holdableLayer);

        RaycastHit raycast;

        // If we hit a GameObject on specified layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out raycast, pickupDistance, layermask)) {

            // Save object information for later use
            Transform hit = raycast.transform;
            Rigidbody hitRigidbody = hit.GetComponent<Rigidbody>();

            // Set parent constraint for object to hold
            ParentConstraint hitConstraint = hit.GetComponent<ParentConstraint>();
            hitConstraint.SetTranslationOffset(0, transform.InverseTransformPoint(hit.position));
            hitConstraint.SetRotationOffset(0, (Quaternion.Inverse(transform.rotation) * hit.rotation).eulerAngles);
            hitConstraint.constraintActive = true;

            do
            {
                yield return null;
            } while (!Input.GetButtonDown(leftClickName));
            
            // Disable the parent constraint
            hitConstraint.constraintActive = false;

            // Calculate throwForce
            Vector3 throwForce = transform.TransformDirection(Vector3.forward) * throwDistance;

            // If the holdable object has a FreezeControl script set velocity accordingly
            FreezeControl freezeControl = hit.GetComponent<FreezeControl>();

            if (freezeControl != null)
                freezeControl.SetLastVelocity(throwForce);

            hitRigidbody.velocity = throwForce;
        }

        isHolding = false;
    }

    private void FreezeTimeInput()
    {
        if (Input.GetKey(freezeKey))
        {
            tookAction = false;
            StartCoroutine(TimeFreezeEvent());
        }

        else if (Input.GetKey(unfreezeKey))
            tookAction = true;
    }

    private IEnumerator TimeFreezeEvent()
    {
        TimeController.SetTimeScale(0);

        do
        {
            yield return null;
        } while (!tookAction);

        TimeController.SetTimeScale(1);
        tookAction = false;
    }
}
