using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[CreateAssetMenu]
public class InputDataSO : ScriptableObject {
    
    [System.Serializable]
    public struct AvailableDeviceData {
        public int deviceId;
        public string deviceName;
        public string device;

        public AvailableDeviceData(InputDevice device) {
            this.deviceId = device.deviceId;
            this.deviceName = device.displayName;
            this.device = device.name;
           
        }
    }
    
    public InputActionAsset inputAsset;

    public List<AvailableDeviceData> availableDevices = new List<AvailableDeviceData>();


    public void AddDevice(InputDevice device) {
        availableDevices.Add(new AvailableDeviceData(device));
    }

    public void RemoveAllDevices() {
        availableDevices.Clear();
    }

    private void OnEnable() {

        foreach(InputControlScheme cs in inputAsset.controlSchemes) {
            //Debug.Log("ahoj> " + cs.name);
        }

        RemoveAllDevices();

        foreach(var device in InputSystem.devices) {
            if(device is Keyboard || device is Gamepad) {
                AddDevice(device);
            }
        }
    }
}
