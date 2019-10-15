using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// WARNING DO NOT ATTACH TO GAMEOBJECTS YOU WANT TO BE TIMEAVERSE

public class FreezeControl : MonoBehaviour
{
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

        // Save last know velocity before freeze
        lastVelocity = rigidbody.velocity;

        // Freeze rigid body's position and rotation
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        // Wait until timeScale is no longer zero
        do
        {
            yield return null;
        } while (TimeController.GetTimeScale() == TimeController.TIME_FROZEN);

        // Reset velocity and unfreeze
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.velocity = lastVelocity;

        isRunningCoroutine = false;
    }

    IEnumerator WaitForFreeze()
    {
        isRunningCoroutine = true;

        // Save last know velocity before unfreeze
        lastVelocity = rigidbody.velocity;

        // Freeze rigid body's position and rotation
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        // Wait until timeScale is equal to zero
        do
        {
            yield return null;
        } while (TimeController.GetTimeScale() != TimeController.TIME_FROZEN);

        // Reset velocity and unfreeze
        rigidbody.constraints = RigidbodyConstraints.None;
        rigidbody.velocity = lastVelocity;

        isRunningCoroutine = false;
    }

    public void SetLastVelocity(Vector3 newVelocity)
    {
        lastVelocity = newVelocity;
    }
}
