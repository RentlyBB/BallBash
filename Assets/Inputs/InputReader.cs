using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Events;

[CreateAssetMenu(fileName = "InputReader", menuName = "Game/Input Reader")]
public class InputReader : ScriptableObject, PlayerInputs.IGameplayActions {


	public event UnityAction MoveEvent = delegate { };

	private PlayerInputs _gameInput;

	private void OnEnable() {
		if(_gameInput == null) {
			_gameInput = new PlayerInputs();
			_gameInput.Gameplay.SetCallbacks(this);
		}
	}

	private void OnDisable() {
		_gameInput.Gameplay.Disable();
	}



	void PlayerInputs.IGameplayActions.OnAbility(InputAction.CallbackContext context) => throw new System.NotImplementedException();
    void PlayerInputs.IGameplayActions.OnBoost(InputAction.CallbackContext context) => throw new System.NotImplementedException();
    
    
    
    void PlayerInputs.IGameplayActions.OnMovement(InputAction.CallbackContext context) {
		if(context.phase == InputActionPhase.Performed)
			MoveEvent.Invoke();
	}
}
