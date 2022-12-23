using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;


public class InputDevicesManager : MonoBehaviour {


    public InputDataSO inputData;

    public InputActionAsset inputAsset;

    private void Start() {

        //foreach(InputControlScheme cs in inputAsset.controlSchemes) {
        //    Debug.Log("ahoj> " + cs.name);
        //}

        //foreach(var device in InputSystem.devices) {
        //    if(device is Keyboard || device is Gamepad) {

        //        inputData.AddDevice(device);
        //    }
        //}
    }

    //private void OnDisable() {
    //    inputData.RemoveAllDevices();   
    //}
}
