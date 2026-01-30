using Input;
using Unity.VisualScripting;
using UnityEngine;

public class ThirdPersonController : MonoBehaviour
{
    [SerializeField] private CharacterController characterController;
    [SerializeField] private PlayerInputController _playerInputController;

    [SerializeField] private float speed = 3.0f;
    private Vector3 currentMovement;

    private bool loockDirection = true; //right
    private bool loockForward = true; // up
    private bool canMove = true; 

    private void Update()
    {
        if (canMove)
        {
            HandleMovement();
        }
    }

    private void HandleMovement() 
    {
       
        Vector3 inputDirection =   new Vector3(_playerInputController.ReadMove().x, 0f, _playerInputController.ReadMove().y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);

        if (_playerInputController.ReadMove().x > 0) loockDirection = true; //right
        else if (_playerInputController.ReadMove().x < 0) loockDirection = false; //left


        if (_playerInputController.ReadMove().y > 0) loockForward = true; //right
        else if (_playerInputController.ReadMove().y < 0) loockForward = false; //left


        currentMovement.x = worldDirection.x * speed;
        currentMovement.z = worldDirection.z * speed;

        characterController.Move(currentMovement * Time.deltaTime);
        
    }

    public bool GetLoockDirection() 
    {
        return loockDirection;
    }

    public void SetCanMove(bool newCanMove) 
    { 
        canMove = newCanMove;
    }
}
