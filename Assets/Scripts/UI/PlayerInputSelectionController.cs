using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputSelectionController : MonoBehaviour {

    public List<GameObject> playersSelections;

    private void Awake() {
        foreach(Transform elem in transform) {
            playersSelections.Add(elem.gameObject);
            elem.gameObject.SetActive(false);
        }

    }

    private void deactivateAllElem() {
        foreach(GameObject elem in playersSelections) {
            elem.SetActive(false);
        }
    }

    public void activatePlayerSelectionBoxes() {

        if(playersSelections.Count == 0) {
            return;
        }

        deactivateAllElem();

        int numberOfPlayers = PlayerManager.Instance.numberOfPlayers;

        for(int i = 0; i < numberOfPlayers; i++) {
            playersSelections[i].SetActive(true);
        }
    }
}
