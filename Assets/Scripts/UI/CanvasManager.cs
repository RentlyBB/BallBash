using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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

    private CanvasController lastActiveCanvas;

    private void Awake() {
        listCanvasControllers = GetComponentsInChildren<CanvasController>().ToList();

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
        } else {
            Debug.LogWarning("Desired canvas does not exist: " + _canvasType.ToString() );
        }
    }
}
