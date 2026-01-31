using Input;
using System.Collections;
using UnityEngine;

public class Dash : MonoBehaviour
{
    [SerializeField] private float dashMovement = 2f;
    [SerializeField] private float dashDuration = 0.25f;
    [SerializeField] private float coolDown = 0.5f;

    private PlayerInputController playerInputController;
    private ThirdPersonController thirdPersonController;
    private bool canDash = true;

    [SerializeField] SpriteReproducer dashSprites;


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
        if (canDash) 
        { 
            StartCoroutine(WaitDash());

            Vector3 pos = gameObject.transform.position;
            if (thirdPersonController.GetLoockDirection()) //right
            {
                pos.x += dashMovement;

            }
            else 
            {
                pos.x -= dashMovement;
            }


            if (!Physics.Linecast(gameObject.transform.position, pos))
            {
                gameObject.transform.position = pos;
                if(dashSprites) dashSprites.ReproduceSpriteSequence(thirdPersonController.GetLoockDirection());
                StartCoroutine(CoolDownDash());
            }
            else Debug.Log("Can't dash");
        }
    }

    IEnumerator WaitDash() 
    {
        thirdPersonController.SetCanMove(false);
        yield return new WaitForSeconds(dashDuration);
        thirdPersonController.SetCanMove(true);
    }

    IEnumerator CoolDownDash() 
    {
        canDash = false;
        yield return new WaitForSeconds(coolDown);
        canDash = true;
    }
}
