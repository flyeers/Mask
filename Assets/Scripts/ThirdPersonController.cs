using UnityEngine;
using Input;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInputController _playerInputController;

    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float gravity = -9.81f; // Added gravity constant

    private Vector3 currentMovement;
    private float verticalVelocity; // Added to track falling speed

    private bool loockDirection = true; //right
    private bool loockForward = true; // up
    private bool canMove = true; 

    private void Update()
    {
        // Apply gravity every frame so the player stays on the ground
        ApplyGravity();

        if (canMove)
        {
            HandleMovement();
        }
    }

    private void ApplyGravity()
    {
        // Reset downward velocity when grounded to prevent build-up
        if (characterController.isGrounded && verticalVelocity < 0)
        {
            verticalVelocity = -2f; // Small force to keep player snapped to floors
        }
        else
        {
            // Acceleration: speed increases over time while falling
            verticalVelocity += gravity * Time.deltaTime;
        }

        // Apply only the vertical part of movement
        characterController.Move(new Vector3(0, verticalVelocity, 0) * Time.deltaTime);
    }

    private void HandleMovement() 
    {
        // Caching input for performance and logic checks
        Vector2 input = _playerInputController.ReadMove();
        Vector3 inputDirection = new Vector3(input.x, 0f, input.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);

        if (input.x > 0) loockDirection = true; //right
        else if (input.x < 0) loockDirection = false; //left

        if (input.y > 0) loockForward = true; //up
        else if (input.y < 0) loockForward = false; //down

        currentMovement.x = worldDirection.x * speed;
        currentMovement.z = worldDirection.z * speed;

        // Move horizontally (Y is set to 0 because ApplyGravity handles the vertical)
        characterController.Move(new Vector3(currentMovement.x, 0, currentMovement.z) * Time.deltaTime);
    }

    public bool GetLoockDirection() 
    {
        return loockDirection;
    }

    public bool GetloockForward()
    {
        return loockForward;
    }

    public void SetCanMove(bool newCanMove) 
    { 
        canMove = newCanMove;
    }
}