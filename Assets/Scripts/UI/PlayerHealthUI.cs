using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerHealthUI : MonoBehaviour {

    [SerializeField]
    private PlayerDataSO playerData;

    [SerializeField]
    private TextMeshProUGUI textComponent;

    const string P_prefix = "P";


    private void Awake() {
        textComponent = GetComponentInChildren<TextMeshProUGUI>();
    }



    private void OnEnable() {
        CanvasSwitcher.onStartGame += updateUIOnStartGame;
        GoalHandler.goalScored += updatePlayerHealth;
    }

    private void OnDisable() {
        CanvasSwitcher.onStartGame -= updateUIOnStartGame;
        GoalHandler.goalScored -= updatePlayerHealth;
    }

    private void updateUIOnStartGame() {

        updatePlayerHealth(playerData.playerId);

        if(!playerData.inPlay) {
            gameObject.SetActive(false);
        }
    }

    private void updatePlayerHealth(int playerID) {

        if(playerID == playerData.playerId) { 
            textComponent.text = P_prefix + playerData.playerId + ": " + playerData.health;
        }

    }

}
