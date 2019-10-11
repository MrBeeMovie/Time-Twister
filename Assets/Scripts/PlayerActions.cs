using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        int layermask = LayerMask.GetMask(holdableLayer);
        RaycastHit raycast;

        // If we hit a GameObject on specified layer
        if (Physics.Raycast(transform.position, transform.TransformDirection(Vector3.forward), out raycast, pickupDistance, layermask)) {

            Transform hit = raycast.transform;
            Rigidbody hitRigidbody = hit.GetComponent<Rigidbody>();

            // Set the hit object to kinimatic and set player as parent
            hitRigidbody.isKinematic = true;
            hit.SetParent(transform);
            

            do
            {
                // Check if hit's rigidbody is kinematic if not make kinematic
                if (!hitRigidbody.isKinematic)
                    hitRigidbody.isKinematic = true;

                yield return null;
            } while (!Input.GetButtonDown(leftClickName));

            // When left click is pressed throw object
            hit.SetParent(null);
            hitRigidbody.isKinematic = false;
            Vector3 throwForce = transform.TransformDirection(Vector3.forward * throwDistance);
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
