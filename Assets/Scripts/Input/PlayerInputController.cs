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
        private PlayerInput m_playerInput;

        public event Action UseAbility;
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
        }

        public Vector2 ReadMove()
        {
            return moveInputAction.InputAction.ReadValue<Vector2>();
        }
    }
}