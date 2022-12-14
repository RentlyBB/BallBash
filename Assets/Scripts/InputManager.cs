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
    private InputAction boost;
    private InputAction ability;
    private PlayerInput playerInput;

    PlayerInputData inputData = new PlayerInputData();

    private CartController cartController;
    private CartAbilities cartAbilities;


    private void Awake() {

        cartController = GetComponent<CartController>();
        cartAbilities = GetComponent<CartAbilities>();
        playerInput = GetComponent<PlayerInput>();

        foreach(InputDevice s in playerInput.devices) { 
            Debug.Log(s.displayName);
        }


        inputAsset = GetComponent<PlayerInput>().actions;

        gameplay = inputAsset.FindActionMap("Gameplay");
    }

    private void OnEnable() {
        movement = gameplay.FindAction("Movement");
        movement.performed += handlePlayerInputs;
        movement.canceled += handlePlayerInputs;

        boost = gameplay.FindAction("Boost");
        boost.performed += handlePlayerInputs;
        boost.canceled += handlePlayerInputs;

        ability = gameplay.FindAction("Ability");
        ability.performed += handlePlayerInputs;
        ability.canceled += handlePlayerInputs;

        gameplay.Enable();
    }


    private void OnDisable() {
        movement.performed -= handlePlayerInputs;
        movement.canceled -= handlePlayerInputs;

        boost.performed -= handlePlayerInputs;
        boost.canceled -= handlePlayerInputs;

        ability.performed -= handlePlayerInputs;
        ability.canceled -= handlePlayerInputs;

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
        } else if(ctx.action.name == "Boost") {
            switch(ctx.phase) {
                case InputActionPhase.Performed:
                    boostPerformed(ctx);
                    break;
                case InputActionPhase.Canceled:
                    boostCanceled(ctx);
                    break;
            }
        } else if(ctx.action.name == "Ability") {
            switch(ctx.phase) {
                case InputActionPhase.Performed:
                    abilityPerformed(ctx);
                    break;
                case InputActionPhase.Canceled:
                    abilityCanceled(ctx);
                    break;
            }
        }

        cartController.SetPlayerInput(ref inputData);
    }

    private void movementPerformed(InputAction.CallbackContext ctx) {
        inputData.movementDirection = ctx.ReadValue<Vector2>();
        inputData.isMoving = true;
    }

    private void movementCanceled(InputAction.CallbackContext ctx) {
        inputData.isMoving = false;
    }

    private void boostPerformed(InputAction.CallbackContext ctx) {
        cartController.accelerateMovement(true);
    }

    private void boostCanceled(InputAction.CallbackContext ctx) {
        cartController.accelerateMovement(false);
    }

    private void abilityPerformed(InputAction.CallbackContext ctx) {
        cartAbilities.pushBallAway();
    }

    private void abilityCanceled(InputAction.CallbackContext ctx) {
        // Nothing yet
    }
}
