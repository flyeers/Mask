using Input;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private float dashMovement = 2f;

    private PlayerInputController playerInputController;
    private ThirdPersonController thirdPersonController;


    private void Awake()
    {
        playerInputController = GetComponent<PlayerInputController>();
        thirdPersonController = GetComponent<ThirdPersonController>();

        if (playerInputController == null)
        {
            Debug.LogError("Dash: PlayerInputController.");
        }
        if (thirdPersonController == null)
        {
            Debug.LogError("Dash: ThirdPersonController.");
        }
    }

    private void OnEnable()
    {
        if (playerInputController != null)
            playerInputController.UseAbility += DashAction;
    }

    private void OnDisable()
    {
        if (playerInputController != null)
            playerInputController.UseAbility -= DashAction;
    }

    private void DashAction()
    {
        Debug.Log("DASHEO");

        Vector3 pos = gameObject.transform.position;

        if (thirdPersonController.GetLoockDirection()) //right
        {
            pos.x += dashMovement;

        }
        else 
        {
            pos.x -= dashMovement;
        }
        gameObject.transform.position = pos;

    }


}
