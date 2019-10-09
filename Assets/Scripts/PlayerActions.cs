using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerActions : MonoBehaviour
{
    [SerializeField] private string leftClickName = "Fire1";   
    [SerializeField] private KeyCode freezeKey = KeyCode.E;

    private bool tookAction;

    private void Start()
    {
        tookAction = false;
    }

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
            tookAction = true;
    }

    private void FreezeTimeInput()
    {
        if (Input.GetKey(freezeKey))
        {
            tookAction = false;
            StartCoroutine(TimeFreezeEvent());
        }
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
