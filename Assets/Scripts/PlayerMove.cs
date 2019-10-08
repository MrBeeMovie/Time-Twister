using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private string horizontalInputName, verticalInputName;
    [SerializeField] private AnimationCurve jumpFalloff;
    [SerializeField] private float jumpMultiplier, gravityMultiplier, 
        movementSpeed, runMultiplier, pushForce;
    [SerializeField] private KeyCode jumpKey, runKey;

    private Vector3 gravity;
    private bool isJumping = false, isRunning = false;
    private CharacterController charController;

    private void Awake()
    {
        gravity = new Vector3(0, gravityMultiplier * Physics.gravity.y, 0)  ;
        charController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        PlayerMovement();
    }

    private void PlayerMovement()
    {
        float vertInput = Input.GetAxis(verticalInputName) * movementSpeed;
        float horizInput = Input.GetAxis(horizontalInputName) * movementSpeed;

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

    private IEnumerator JumpEvent()
    {
        float slopeLimit = charController.slopeLimit;
        charController.slopeLimit = 90.0f;

        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFalloff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.unscaledDeltaTime);
            timeInAir += Time.deltaTime;

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

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody hitBody = hit.rigidbody;

        if (hitBody == null || hitBody.isKinematic)
            return;

        hitBody.velocity = pushForce * hit.moveDirection;
    }
}
