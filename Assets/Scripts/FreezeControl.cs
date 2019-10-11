using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeControl : MonoBehaviour
{
    // WARNING DO NOT ATTACH TO GAMEOBJECTS YOU WANT TO BE TIMEAVERSE

    private Vector3 lastVelocity = Vector3.zero;
    private new Rigidbody rigidbody;
    private bool isRunningCoroutine = false, isTimeDependent;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
        isTimeDependent = CompareTag("TimeDependent");
    }

    private void FixedUpdate()
    {
        // If GameObject is timedependent wait and timescale is TIME_FROZEN run WaitForUnfreeze coroutine
        if (isTimeDependent & TimeController.GetTimeScale() == TimeController.TIME_FROZEN & !isRunningCoroutine)
            StartCoroutine(WaitForUnfreeze());
        // If GameObject is timeindependent wait and timescale is TIME_DEFAULT run WaitForFreeze coroutine
        else if (!isTimeDependent & TimeController.GetTimeScale() == TimeController.TIME_DEFAULT & !isRunningCoroutine)
            StartCoroutine(WaitForFreeze());
    }

    IEnumerator WaitForUnfreeze()
    {
        isRunningCoroutine = true;

        // Save last know velocity before freeze and set to kinematic
        lastVelocity = rigidbody.velocity;
        rigidbody.isKinematic = true;

        // Wait until timeScale is no longer zero
        do
        {
            // Make sure that nothing changes kinematic state
            if (!rigidbody.isKinematic)
            {
                // If it kinematic state was changed re-calculate last velocity
                lastVelocity = rigidbody.velocity;
                rigidbody.isKinematic = true;
            }

            yield return null;
        } while (TimeController.GetTimeScale() == TimeController.TIME_FROZEN);

        // Reset velocity and set kinematic to false
        rigidbody.isKinematic = false;
        rigidbody.velocity = lastVelocity;
        lastVelocity = Vector3.zero;

        isRunningCoroutine = false;
    }

    IEnumerator WaitForFreeze()
    {
        isRunningCoroutine = true;

        // Save last know velocity before unfreeze and set to kinematic
        lastVelocity = rigidbody.velocity;
        rigidbody.isKinematic = true;

        // Wait until timeScale is equal to zero
        do
        {
            // Make sure that nothing changes kinematic state
            if (!rigidbody.isKinematic)
            {
                // If it kinematic state was changed re-calculate last velocity
                lastVelocity = rigidbody.velocity;
                rigidbody.isKinematic = true;
            }

            yield return null;
        } while (TimeController.GetTimeScale() != TimeController.TIME_FROZEN);

        // Reset velocity and set kinematic to false
        rigidbody.isKinematic = false;
        rigidbody.velocity = lastVelocity;
        lastVelocity = Vector3.zero;

        isRunningCoroutine = false;
    }
}
