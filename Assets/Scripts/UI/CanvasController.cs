using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasController : MonoBehaviour {

    //Type of canvas, example: MainMenu/PlayerOption/
    public CanvasManager.CanvasType canvasType;

    //Button to be selected in canvas
    //Required for gamepads and arrow menu control
    public Button firstButtonToBeSelected;

}
