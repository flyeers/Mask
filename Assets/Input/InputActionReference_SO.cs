using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    [CreateAssetMenu(fileName = "InputActionReference_SO", menuName = "Scriptable Objects/InputActionReference_SO")]
    public class InputActionReference_SO : ScriptableObject
    {
        [SerializeField]
        private InputActionAsset inputActionAsset;

        [SerializeField]
        private string actionPath;

        public InputAction InputAction => inputActionAsset.FindAction(actionPath);
    }
}
