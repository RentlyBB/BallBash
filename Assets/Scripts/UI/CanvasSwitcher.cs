using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#if UNITY_EDITOR
using UnityEditor;
#endif


[RequireComponent(typeof(Button))]
public class CanvasSwitcher : MonoBehaviour {

    public delegate void StartGame();
    public static StartGame startGame;


    [Header("The type of canvas you want to switch to.")]
    public CanvasManager.CanvasType desiredCanvas;
    
    [Space]
    public CanvasManager.ButtonType buttonType;
    [HideInInspector]
    public int numberOfPlayers;

    private CanvasManager canvasManager;
    private PlayerManager playerManager;
    private Button button;


    private void Start() {
        canvasManager = CanvasManager.Instance;
        playerManager = PlayerManager.Instance;
        button = GetComponent<Button>();
        button.onClick.AddListener(onButtonClicked);
    }

    private void onButtonClicked() {

        switch(buttonType) {
            case CanvasManager.ButtonType.MenuNavigation:
                canvasManager.switchCanvas(desiredCanvas);
                break;
            case CanvasManager.ButtonType.PlayerNumberSelection:
                canvasManager.switchCanvas(desiredCanvas);
                playerManager.numberOfPlayers = numberOfPlayers;
                canvasManager.updatePlayersSelectionBoxes();
                break;
            case CanvasManager.ButtonType.StartGame:
                canvasManager.switchCanvas(desiredCanvas);
                GameObject.FindGameObjectWithTag("Logo").SetActive(false);
                playerManager.addPlayers();
                startGame?.Invoke();
                break;
        }
        
    }

}

#if UNITY_EDITOR
[CustomEditor(typeof(CanvasSwitcher))]
public class CanvasSwitcher_Editor : Editor {
    public override void OnInspectorGUI() {

        base.OnInspectorGUI();

        CanvasSwitcher canvasSwitcher = target as CanvasSwitcher;


        if(canvasSwitcher.buttonType == CanvasManager.ButtonType.PlayerNumberSelection) {
            canvasSwitcher.numberOfPlayers = EditorGUILayout.IntField("Number Of Player", canvasSwitcher.numberOfPlayers);
        }

    }
}

#endif

