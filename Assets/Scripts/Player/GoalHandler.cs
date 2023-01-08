using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoalHandler : MonoBehaviour {


    public delegate void GoalScored(int playerID);
    public static GoalScored goalScored;

    public PlayerDataSO playerData;

    private void OnEnable() {
        CanvasSwitcher.onStartGame += checkIfDead;
    }

    private void OnDisable() {
        CanvasSwitcher.onStartGame -= checkIfDead;
    }

    private void checkIfDead() {
        if(playerData.isDead || !playerData.inPlay) {
            transform.GetChild(0).gameObject.SetActive(true);
        } else {
            transform.GetChild(0).gameObject.SetActive(false);
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Ball")) {
            playerData.decreasedHealth(1);
            checkIfDead();

            goalScored?.Invoke(playerData.playerId);
            other.gameObject.SetActive(false);
        }
    }

}
