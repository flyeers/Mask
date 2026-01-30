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

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement() 
    {     
        Vector3 inputDirection =   new Vector3(_playerInputController.ReadMove().x, 0f, _playerInputController.ReadMove().y);
        Vector3 worldDirection = transform.TransformDirection(inputDirection);

        if (_playerInputController.ReadMove().x > 0) loockDirection = true; //right
        else loockDirection = false; //left


        currentMovement.x = worldDirection.x * speed;
        currentMovement.z = worldDirection.z * speed;

        characterController.Move(currentMovement * Time.deltaTime);
    }

    public bool GetLoockDirection() 
    {
        return loockDirection;
    }
}
