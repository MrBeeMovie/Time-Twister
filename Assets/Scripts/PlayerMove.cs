using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private string horizontalInputName, verticalInputName, leftClickName;
    [SerializeField] private AnimationCurve jumpFalloff;
    [SerializeField] private float jumpMultiplier, gravityMultiplier, 
        movementSpeed, runMultiplier, pushForce;
    [SerializeField] private KeyCode jumpKey, runKey, freezeKey;

    private Vector3 gravity;
    private bool isJumping, isRunning, tookAction;
    private CharacterController charController;

    private void Awake()
    {
        gravity = new Vector3(0, gravityMultiplier * Physics.gravity.y, 0)  ;
        charController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        isJumping = isRunning = tookAction = false;
    }

    private void Update()
    {
        Debug.Log("Updating");
        PlayerMovement();
        PlayerActions();
    }

    private void PlayerActions()
    {
        ActionInput();
        FreezeTimeInput();
    }

    private void PlayerMovement()
    {
        float vertInput = Input.GetAxisRaw(verticalInputName) * movementSpeed;
        float horizInput = Input.GetAxisRaw(horizontalInputName) * movementSpeed;

        Vector3 forwardMovement = transform.forward * vertInput * Time.unscaledDeltaTime;
        Vector3 rightMovement = transform.right * horizInput * Time.unscaledDeltaTime;

        RunInput();

        if(isRunning)
        {
            forwardMovement *= runMultiplier;
            rightMovement *= runMultiplier;
        }

        charController.Move(forwardMovement + rightMovement);

        JumpInput();
        ApplyGravity();
    }

    private void JumpInput()
    {
        if(Input.GetKeyDown(jumpKey) & !isJumping)
        {
            isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }

    private void RunInput()
    {
        if(Input.GetKey(runKey) & !isRunning)
        {
            isRunning = true;
            StartCoroutine(RunEvent());
        }
    }

    private void FreezeTimeInput()
    {
        if(Input.GetKey(freezeKey))
        {
            tookAction = false;
            StartCoroutine(TimeFreezeEvent());
        }
    }

    private void ActionInput()
    {
        if (Input.GetButtonDown(leftClickName))
            tookAction = true;
    }

    private IEnumerator JumpEvent()
    {
        float slopeLimit = charController.slopeLimit;
        charController.slopeLimit = 90.0f;

        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFalloff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.unscaledDeltaTime);
            timeInAir += Time.unscaledDeltaTime;

            // checks if we have reached last key in animation curve
            if (jumpForce == jumpFalloff[jumpFalloff.length - 1].value)
                isJumping = false;
            yield return null;
        } while (charController.collisionFlags != CollisionFlags.Above & isJumping);

        charController.slopeLimit = slopeLimit;
    }

    private IEnumerator RunEvent()
    {
        do
        {
            yield return null;
        } while (Input.GetKey(runKey));

        isRunning = false;
    }

    private void ApplyGravity()
    {
        if (!charController.isGrounded & !isJumping)
            charController.Move(gravity * Time.unscaledDeltaTime);
    }

    private IEnumerator TimeFreezeEvent()
    {
        TimeController.SetTimeScale(0);

        do {
            yield return null;
        } while (!tookAction);

        TimeController.SetTimeScale(1);
        tookAction = false;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody hitBody = hit.rigidbody;

        if (hitBody == null || hitBody.isKinematic)
            return;

        hitBody.velocity = pushForce * hit.moveDirection;
    }
}
