using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public struct PlayerInputData {

    // values
    public float movementDirection;

    // state
    public bool isMoving;

    public bool horizontalMovement;

    public bool invertMovement;
}

[RequireComponent(typeof(CartController))]

public class InputController : MonoBehaviour {

    private InputActionAsset inputAsset;
    private InputActionMap gameplay;
    private InputAction ia_movement;
    private InputAction ia_boost;
    private InputAction ia_ability;
    private PlayerInput playerInput;

    PlayerInputData inputData = new PlayerInputData();

    private CartController cartController;
    private IAbility ability;


    private void Awake() {

        cartController = GetComponent<CartController>();
        
        ability = GetComponent<IAbility>();

        playerInput = GetComponent<PlayerInput>();

        inputAsset = GetComponent<PlayerInput>().actions;

        gameplay = inputAsset.FindActionMap("Gameplay");
    }

    private void Start() {
        switch(playerInput.playerIndex){
            case 0:
                inputData.horizontalMovement = true;
                inputData.invertMovement = false;
                break;
            case 1:
                inputData.horizontalMovement = true;
                inputData.invertMovement = true;
                break;
            case 2:
                inputData.horizontalMovement = false;
                inputData.invertMovement = false;
                break;
            case 3:
                inputData.horizontalMovement = false;
                inputData.invertMovement = true;
                break;
        }
    }

    private void OnEnable() {
        ia_movement = gameplay.FindAction("Movement");
        ia_movement.performed += handlePlayerInputs;
        ia_movement.canceled += handlePlayerInputs;

        ia_boost = gameplay.FindAction("Boost");
        ia_boost.performed += handlePlayerInputs;
        ia_boost.canceled += handlePlayerInputs;

        ia_ability = gameplay.FindAction("Ability");
        ia_ability.performed += handlePlayerInputs;
        ia_ability.canceled += handlePlayerInputs;

        gameplay.Enable();
    }

    private void OnDisable() {
        ia_movement.performed -= handlePlayerInputs;
        ia_movement.canceled -= handlePlayerInputs;

        ia_boost.performed -= handlePlayerInputs;
        ia_boost.canceled -= handlePlayerInputs;

        ia_ability.performed -= handlePlayerInputs;
        ia_ability.canceled -= handlePlayerInputs;

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

        cartController.setPlayerInput(ref inputData);
    }

    private void movementPerformed(InputAction.CallbackContext ctx) {
        if(inputData.horizontalMovement) {
            inputData.movementDirection = ctx.ReadValue<Vector2>().x;
        } else { 
            inputData.movementDirection = ctx.ReadValue<Vector2>().y;
        }

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
        if (ability != null) ability.activateAbility();
        else Debug.LogWarning("No ability not assigned.");

    }

    private void abilityCanceled(InputAction.CallbackContext ctx) {
        if(ability != null) ability.deactivateAbility();
        else Debug.LogWarning("No ability not assigned.");
    }
}
