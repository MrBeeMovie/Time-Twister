using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    [SerializeField] private string horizontalInputName, verticalInputName;
    [SerializeField] private float movementSpeed;
    [SerializeField] private AnimationCurve jumpFalloff;
    [SerializeField] private float jumpMultiplier, gravityMultiplier;
    [SerializeField] private KeyCode jumpKey;

    private Vector3 gravity;
    private bool isJumping = false;
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

        Vector3 forwardMovement = transform.forward * vertInput * Time.deltaTime;
        Vector3 rightMovement = transform.right * horizInput * Time.deltaTime;

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

    private IEnumerator JumpEvent()
    {
        float slopeLimit = charController.slopeLimit;
        charController.slopeLimit = 90.0f;

        float timeInAir = 0.0f;

        do
        {
            float jumpForce = jumpFalloff.Evaluate(timeInAir);
            charController.Move(Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;

            // checks if we have reached last key in animation curve
            if (jumpForce == jumpFalloff[jumpFalloff.length - 1].value)
                isJumping = false;
            yield return null;
        } while (charController.collisionFlags != CollisionFlags.Above & isJumping);

        charController.slopeLimit = slopeLimit;
    }

    private void ApplyGravity()
    {
        if (!charController.isGrounded & !isJumping)
            charController.Move(gravity * Time.deltaTime);
    }
}
