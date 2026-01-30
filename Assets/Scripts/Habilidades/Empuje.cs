using UnityEngine;

public class Empuje : MonoBehaviour
{

    [SerializeField] private float fuerzaEmpuje = 5f;
    [SerializeField] private KeyCode teclaAgarrar = KeyCode.E;

    private Rigidbody rbMovable;
    private Transform trMovable;
    private bool puedeAgarrar;
    private bool agarrando;
    private GameObject movableActual;
    PlayerInputController playerInputController;

     private void Awake()
    {
        playerInputController = GetComponent<PlayerInputController>();

        if (playerInputController == null)
        {
            Debug.LogError("Empuje: No se encontró PlayerInputController.");
        }
    }

    private void OnEnable()
    {
        if (playerInputController != null)
            playerInputController.UseAbility += Push;
    }

    private void OnDisable()
    {
        if (playerInputController != null)
            playerInputController.UseAbility -= Push;
    }

    private void Push()
    {
      if(puedeAgarrar && Input.GetKeyDown(teclaAgarrar)){
            agarrando = !agarrando;

            if(rbMovable != null){
                rbMovable.freezeRotation = agarrando;
            }
        }
    }
    private void OnCollisionEnter(Collision collision){
        if(!collision.gameObject.CompareTag("movable")) return;

        rbMovable = collision.rigidbody;
        trMovable = collision.transform;
        puedeAgarrar = (rbMovable != null);
    
    }

    private void OnCollisionExit(Collision collision){
        if(!collision.gameObject.CompareTag("movable")) return;
        
        puedeAgarrar = false;
        agarrando = false;
        rbMovable = null;
        trMovable = null;

    }

    // Update is called once per frame
    void Update()
    {
  
    }

    private void FixedUpdate(){
        if(!agarrando || rbMovable == null || trMovable == null) return;

        float v = Input.GetAxisRaw("Vertical");
        if (Mathf.Abs(v) < 0.01f) return;

        Vector3 axis = trMovable.forward;
        axis.y = 0f;
        axis.Normalize();

        rbMovable.AddForce(axis * (v * fuerzaEmpuje), ForceMode.Force);
    }

}
