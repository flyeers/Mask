using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{


    [RequireComponent(typeof(PlayerInput))]
    public class PlayerInputController : MonoBehaviour
    {
        [SerializeField] private InputActionReference useAbilityInputAction;
        [SerializeField] private InputActionReference moveInputAction;
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
            if (context.action == useAbilityInputAction.action)
            {
                if (context.performed)
                {
                    UseAbility?.Invoke();
                }
            }

            if (context.action == moveInputAction.action)
            {
                if (context.started)
                {
                    Move?.Invoke();
                }
            }
        }

        public Vector2 ReadMove()
        {
            return moveInputAction.action.ReadValue<Vector2>();
        }
    }
}
