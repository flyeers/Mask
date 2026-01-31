using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{


    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private InputActionReference_SO useAbilityInputAction;
        [SerializeField] private InputActionReference_SO moveInputAction;
        [SerializeField] private InputActionReference_SO jumpInputAction;
        [SerializeField] private InputActionReference_SO previousInputAction;
        [SerializeField] private InputActionReference_SO nextInputAction;
        private PlayerInput m_playerInput;

        public event Action UseAbility;
        public event Action Jump;
        public event Action Previous;
        public event Action Next;
        public event Action Move;

        private void Awake()
        {
            m_playerInput = GetComponent<PlayerInput>();
            m_playerInput.onActionTriggered += PlayerInputOnActionTriggered;
        }

        private void PlayerInputOnActionTriggered(InputAction.CallbackContext context)
        {
            if (context.action == useAbilityInputAction.InputAction)
            {
                if (context.performed)
                {
                    UseAbility?.Invoke();
                }
            }

            if (context.action == moveInputAction.InputAction)
            {
                if (context.started)
                {
                    Move?.Invoke();
                }
            }

            if (context.action == jumpInputAction.InputAction)
            {
                if (context.performed)
                {
                    Jump?.Invoke();
                }
            }

            if (context.action == previousInputAction.InputAction)
            {
                if (context.performed)
                {
                    Previous?.Invoke();
                }
            }

            if (context.action == nextInputAction.InputAction)
            {
                if (context.performed)
                {
                    Next?.Invoke();
                }
            }
        }

        public Vector2 ReadMove()
        {
            return moveInputAction.InputAction.ReadValue<Vector2>();
        }
    }
}