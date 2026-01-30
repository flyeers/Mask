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

    [Header("Salto")]
    [SerializeField] private float jumpHeight = 1.2f;     // altura del salto
    [SerializeField] private float groundStick = -2f;     // para "pegarse" al suelo

    private bool jumpRequested;


    private void Update()
    {
        // 1) Actualiza verticalVelocity (NO mueve)
        ApplyGravityAndJump();

        // 2) Calcula el movimiento horizontal
        Vector2 input = _playerInputController.ReadMove();
        Vector3 inputDirection = new Vector3(input.x, 0f, input.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);

        currentMovement.x = canMove ? worldDirection.x * speed : 0f;
        currentMovement.z = canMove ? worldDirection.z * speed : 0f;
        currentMovement.y = verticalVelocity;

        // 3) ✅ Un solo Move por frame
        CollisionFlags flags = characterController.Move(currentMovement * Time.deltaTime);

        // 4) Grounded fiable
        bool groundedNow = (flags & CollisionFlags.Below) != 0;


    }




    public void Awake()
    {
        if (_playerInputController == null) _playerInputController = GetComponent<PlayerInputController>();
    }

    private void OnEnable()
    {
        if (_playerInputController != null)
            _playerInputController.Jump += Jump;
    }

    private void OnDisable()
    {
        if (_playerInputController != null)
            _playerInputController.Jump -= Jump;
    }

    private void ApplyGravityAndJump()
    {
        bool grounded = characterController.isGrounded; // del frame anterior

        if (grounded && verticalVelocity < 0f)
            verticalVelocity = groundStick;

        if (jumpRequested && grounded)
            verticalVelocity = Mathf.Sqrt(2f * jumpHeight * -gravity);

        jumpRequested = false;

        verticalVelocity += gravity * Time.deltaTime;
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

    private void Jump()
    {
        Debug.Log("Grounded: ");
        if (!canMove) return; // no salto mientras arrastro
        jumpRequested = true;
    }
}