using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public struct AvailableDeviceData {
    public int deviceId;
    public string deviceName;

    public AvailableDeviceData(int deviceId, string deviceName) {
        this.deviceId = deviceId;
        this.deviceName = deviceName;
    }
}

public class InputDevicesManager : MonoBehaviour {

    public List<AvailableDeviceData> availableDevices;

    public AvailableDeviceData div;

    private void Start() {
        foreach(var device in InputSystem.devices) {
            if(device is Keyboard || device is Gamepad) {
                Debug.Log("Name: " + device.displayName + ", ID: " + device.deviceId);
                availableDevices.Add(new AvailableDeviceData(device.deviceId, device.displayName));
            }
        }
    }
}
