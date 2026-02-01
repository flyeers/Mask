using Damage;
using Input;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.UI.Image;

public class Dash : MonoBehaviour
{
    [SerializeField] private float dashMovement = 2f;
    [SerializeField] private float dashDuration = 0.25f;
    [SerializeField] private float coolDown = 0.5f;
    [SerializeField] private LayerMask axeLayer;
    
    [SerializeField] private FMODUnity.StudioEventEmitter _dashSfx;

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
            
            _dashSfx.Play();


            Vector3 origin = gameObject.transform.position;
            Vector3 direction = pos - origin;
            float distance = direction.magnitude;

            Color rayColor = Physics.Raycast(origin, direction.normalized, distance)
                ? Color.red
                : Color.green;

            Debug.DrawRay(origin, direction.normalized * distance, rayColor, 1f);

            if (Physics.Raycast(origin, direction.normalized, out RaycastHit hit, distance))
            {
                //if (hit.collider.gameObject.layer == axeLayer)
                if ((axeLayer.value & (1 << hit.collider.gameObject.layer)) != 0)                              
                {
                    gameObject.transform.position = pos;
                    if (dashSprites)
                        dashSprites.ReproduceSpriteSequence(thirdPersonController.GetLoockDirection());
                    StartCoroutine(WaitDashDie());
                }
                else Debug.Log("Can't dash");
            }
            else //CAN DASH
            {
                gameObject.transform.position = pos;

                if (dashSprites)
                    dashSprites.ReproduceSpriteSequence(thirdPersonController.GetLoockDirection());

                StartCoroutine(CoolDownDash());
            }
        }
    }

    IEnumerator WaitDash() 
    {
        thirdPersonController.SetCanMove(false);
        yield return new WaitForSeconds(dashDuration);
        thirdPersonController.SetCanMove(true);
    }

    IEnumerator WaitDashDie()
    {
        yield return new WaitForSeconds(dashDuration);
        if (gameObject.TryGetComponent<Damageable>(out Damageable damage)) damage.Die();
    }

    IEnumerator CoolDownDash() 
    {
        canDash = false;
        yield return new WaitForSeconds(coolDown);
        canDash = true;
    }
}
