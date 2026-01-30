using System;
using Input;
using UnityEngine;

public class Test : MonoBehaviour
{
    PlayerInputController inputController;
    private void Awake()
    {
        inputController =  GetComponent<PlayerInputController>();
        inputController.UseAbility += InputControllerOnUseAbility;
    }

    private void InputControllerOnUseAbility()
    {
        Debug.Log("Use Ability");
    }

    private void Update()
    {
        Vector2 move = inputController.ReadMove();
        Debug.Log(move);
    }
}
