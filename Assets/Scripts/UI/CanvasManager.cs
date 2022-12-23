using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class CanvasManager : Singleton<CanvasManager> {

    public enum CanvasType { 
        MainMenu,
        PlayerSelection,
        PlayerInputSelection,
        ArenaSelection,
        GameUI,
        PauseGame
    }

    public enum ButtonType {
        MenuNavigation,
        PlayerNumberSelection,
        StartGame,
        EndGame
    }

    public List<CanvasController> listCanvasControllers;

    public bool hasPlayerInputSelection = false;

    [HideInInspector]
    public GameObject playerInputSelectionPrefab;

    private PlayerInputSelectionController pisc;

    private CanvasController lastActiveCanvas;

    private void Awake() {
        listCanvasControllers = GetComponentsInChildren<CanvasController>().ToList();

        if(hasPlayerInputSelection) {
            pisc = playerInputSelectionPrefab.GetComponent<PlayerInputSelectionController>();
        }

        foreach(CanvasController canvas in listCanvasControllers) {
            canvas.gameObject.SetActive(false);
        }

        switchCanvas(CanvasType.MainMenu);

    }

    public void switchCanvas(CanvasType _canvasType) {

        if(lastActiveCanvas != null) { 
            lastActiveCanvas.gameObject.SetActive(false);
        }

        CanvasController desiredCanvas = listCanvasControllers.Find(x => x.canvasType == _canvasType);

        if(desiredCanvas != null) {
            desiredCanvas.gameObject.SetActive(true);
            lastActiveCanvas = desiredCanvas;

            if(desiredCanvas.firstButtonToBeSelected != null) {
                EventSystem.current.SetSelectedGameObject(desiredCanvas.firstButtonToBeSelected.gameObject);
            } else {
                Debug.LogWarning("Not selected button found in the canvas.");
            }

        } else {
            Debug.LogWarning("Desired canvas does not exist: " + _canvasType.ToString() );
        }
    }

    public void updatePlayersSelectionBoxes() {
        pisc.activatePlayerSelectionBoxes();
    }
}



#if UNITY_EDITOR
[CustomEditor(typeof(CanvasManager))]
public class CanvasManager_Editor : Editor {
    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        CanvasManager canvasManager = target as CanvasManager;


        if(canvasManager.hasPlayerInputSelection == true) {
            canvasManager.playerInputSelectionPrefab = (GameObject)EditorGUILayout.ObjectField("playerInputSelectionPrefab:", canvasManager.playerInputSelectionPrefab, typeof(GameObject), true); ;
        }

    }
}

#endif
