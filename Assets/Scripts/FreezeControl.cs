using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreezeControl : MonoBehaviour
{
    private Vector3 lastVelocity = Vector3.zero;
    private new Rigidbody rigidbody;
    private bool isRunningCoroutine = false;

    private void Awake()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // If GameObject is timedependent wait and timescale is set to zero run WaitForUnfreeze coroutine
        if (CompareTag("TimeDependent") & TimeController.GetTimeScale() == 0 & !isRunningCoroutine)
            StartCoroutine(WaitForUnfreeze());
        // If GameObject is timeindependent wait and timescale is not set to zero run WaitForFreeze coroutine
        else if (CompareTag("TimeIndependent") & TimeController.GetTimeScale() != 0 & !isRunningCoroutine)
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
            yield return null;
        } while (TimeController.GetTimeScale() == 0);

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
            yield return null;
        } while (TimeController.GetTimeScale() != 0);

        // Reset velocity and set kinematic to false
        rigidbody.isKinematic = false;
        rigidbody.velocity = lastVelocity;
        lastVelocity = Vector3.zero;

        isRunningCoroutine = false;
    }
}
