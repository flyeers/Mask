using UnityEngine;
using Input;
using Unity.Cinemachine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInputController _playerInputController;
    [SerializeField] private MaskInventory maskInventory;
    [SerializeField] private CinemachineCamera cinemachineCameraRight;
    [SerializeField] private CinemachineCamera cinemachineCameraLeft;


    [SerializeField] private float speed = 3.0f;
    [SerializeField] private float gravity = -9.81f; // Added gravity constant

    private Vector3 currentMovement;
    private float verticalVelocity; // Added to track falling speed

    private bool loockDirection = true; //right
    private bool loockForward = true; // up

    [Header("Salto")]
    [SerializeField] private float jumpHeight = 1.2f;     // altura del salto
    [SerializeField] private float groundStick = -2f;     // para "pegarse" al suelo

    private bool jumpRequested;
    
    private Animator _animator;
    private SpriteRenderer _spriteRenderer;

    public bool CanMove { get; set; } = true;

    private bool _gravityEnabled = true;

    public bool GravityEnabled
    {
        get { return _gravityEnabled; }
        set
        {
            _gravityEnabled = value;
            if (!_gravityEnabled)
            {
                verticalVelocity = 0f;
            }
        }
    }

    private void Update()
    {
        if (GravityEnabled)
        {
            // 1) Actualiza verticalVelocity (NO mueve)
            ApplyGravityAndJump();
        }

        if (CanMove) 
        { 
            HandleMovement();
        }
    }




    public void Awake()
    {
        if (_playerInputController == null) _playerInputController = GetComponent<PlayerInputController>();
        _animator = GetComponentInChildren<Animator>();
        _spriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    private void OnEnable()
    {
        if (_playerInputController != null)
            _playerInputController.Jump += Jump;

        if (_playerInputController != null)
            _playerInputController.Previous += Previous;

        if (_playerInputController != null)
            _playerInputController.Next += Next;
    }

    private void OnDisable()
    {
        if (_playerInputController != null)
            _playerInputController.Jump -= Jump;


        if (_playerInputController != null)
            _playerInputController.Previous -= Previous;

        if (_playerInputController != null)
            _playerInputController.Next -= Next;
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
        // 2) Calcula el movimiento horizontal
        Vector2 input = _playerInputController.ReadMove();
        Vector3 inputDirection = new Vector3(input.x, 0f, input.y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);

        currentMovement.x = CanMove ? worldDirection.x * speed : 0f;
        currentMovement.z = CanMove ? worldDirection.z * speed : 0f;
        currentMovement.y = verticalVelocity;

        //SET IF LOCK RIGHT / LEFT 
        if (input.x > 0)
        {
            loockDirection = true; //right
            cinemachineCameraRight.enabled = true;
            cinemachineCameraLeft.enabled = false;
        }
        else if (input.x < 0) 
        {
            loockDirection = false; //left
            cinemachineCameraRight.enabled = false;
            cinemachineCameraLeft.enabled = true;
        }

        //SET IF LOCK RIGHT / LEFT 
        if (input.y > 0) loockForward = true; //up
        else if (input.y < 0) loockForward = false; //down

        // 3) ✅ Un solo Move por frame
        CollisionFlags flags = characterController.Move(currentMovement * Time.deltaTime);

        // 4) Grounded fiable
        bool groundedNow = (flags & CollisionFlags.Below) != 0;

        if (currentMovement.x > 0f)
        {
            _spriteRenderer.flipX = false;
        }
        else if (currentMovement.x < 0f)
        {
            _spriteRenderer.flipX = true;
        }
        
        var horizontalSpeed = new Vector2(currentMovement.x, currentMovement.z).magnitude;
        _animator.SetBool("Move", horizontalSpeed > 0.01f);
        
        _animator.SetBool("Grounded", groundedNow);
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
        CanMove = newCanMove;
    }

    public void EnableAllMovement(bool enable)
    {
        CanMove = enable;
        GravityEnabled = enable;
    }

    private void Jump()
    {
        if (!CanMove) return; // no salto mientras arrastro
        jumpRequested = true;
        _animator.SetTrigger("Jump");
    }

    private void Previous()
    {
        maskInventory.PrevItem();
    }

    private void Next()
    {
        maskInventory.NextItem();

    }
}