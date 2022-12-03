using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public struct PlayerInputData {
    // values
    public Vector2 movementDirection;

    // state
    public bool isMoving;
}

public class InputManager : MonoBehaviour {
    


    private InputActionAsset inputAsset;
    private InputActionMap gameplay;
    private InputAction movement;
    private InputAction abilities;

    PlayerInputData inputData = new PlayerInputData();

    private CartController cartController;


    private void Awake() {

        cartController = GetComponent<CartController>();

        inputAsset = this.GetComponent<PlayerInput>().actions;
        gameplay = inputAsset.FindActionMap("Gameplay");
    }

    private void OnEnable() {
        movement = gameplay.FindAction("Movement");
        movement.performed += handlePlayerInputs;
        movement.canceled += handlePlayerInputs;

        abilities = gameplay.FindAction("Abilities");
        abilities.performed += handlePlayerInputs;
        abilities.canceled += handlePlayerInputs;

        gameplay.Enable();
    }


    private void OnDisable() {
        movement.performed -= handlePlayerInputs;
        movement.canceled -= handlePlayerInputs;

        abilities.performed -= handlePlayerInputs;
        abilities.canceled -= handlePlayerInputs;

        gameplay.Disable();
    }

    private void handlePlayerInputs(InputAction.CallbackContext ctx) {
        if(ctx.action.name == "Movement") {
            switch(ctx.phase) {
                case InputActionPhase.Performed:
                    movementPerformed(ctx);
                    break;
                case InputActionPhase.Canceled:
                    movementCanceled(ctx);
                    break;
            }
        } else if(ctx.action.name == "Abilities") {
            switch(ctx.phase) {
                case InputActionPhase.Performed:
                    abilitiesPerformed(ctx);
                    break;
                case InputActionPhase.Canceled:
                    abilitiesCanceled(ctx);
                    break;
            }
        }


        cartController.SetPlayerInput(ref inputData);
    }

    private void abilitiesPerformed(InputAction.CallbackContext ctx) {
        cartController.accelerateMovement(true);
    }

    private void abilitiesCanceled(InputAction.CallbackContext ctx) {
        cartController.accelerateMovement(false);
    }

    private void movementPerformed(InputAction.CallbackContext ctx) {
        inputData.movementDirection = ctx.ReadValue<Vector2>();
        inputData.isMoving = true;
    }

    private void movementCanceled(InputAction.CallbackContext ctx) {
        inputData.isMoving = false;
    }
}
