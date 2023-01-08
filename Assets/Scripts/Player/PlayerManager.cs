using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager> {

    protected PlayerManager() { }

    [SerializeField]
    private List<PlayerDataSO> playersData;

    [SerializeField]
    private List<GameObject> list_spawnPoints;

    [SerializeField]
    private GameObject playerTrackPrefab;

    public int numberOfPlayers;

    [SerializeField]
    private bool spawnPlayersOnStart = false;

    public int baseHealth = 10; 



    private void Awake() {
        //reset players data
        resetPlayersData();
    }

    private void Start() {

       // list_spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("PlayerSpawn"));


        if(spawnPlayersOnStart) {
            addPlayers();
        }
    }


    private void resetPlayersData() {
        foreach(PlayerDataSO pd in playersData) {
            pd.inPlay = false;
            pd.isDead = false;
            pd.health = 0;
        }
    
    }

    public void addPlayers() {
        if(list_spawnPoints != null) {
            for(int i = 0; i < numberOfPlayers; i++) {
                GameObject track = Instantiate(playerTrackPrefab, list_spawnPoints[i].transform.position, list_spawnPoints[i].transform.rotation);
                track.name = track.name + "_P" + i;

                PlayerInput cartInput = track.GetComponentInChildren<PlayerInput>();
                cartInput.SwitchCurrentControlScheme(getPlayerInputScheme(i), Keyboard.current);

                playersData[i].inPlay = true;
                playersData[i].isDead = false;
                playersData[i].health = baseHealth;

            }
        }

        foreach(var player in PlayerInput.all) {
            //Debug.Log("Player ID: " + player.playerIndex + ", Player Name: " + player.name + ", Player Controll Scheme: " + player.currentControlScheme);
        }
    }


    private string getPlayerInputScheme(int playerId) {
        switch(playerId) {
            case 0:
                return "Keyboard";
            case 1:
                return "Keyboard_P2";
            case 2:
                return "Keyboard_P3";
            case 3:
                return "Keyboard_P4";
            default:
                return "Keyboard";
        }
    }

    private InputDevice getDeviceForPlayer(int deviceID) {

        InputDevice usedDevice = null;

        foreach(InputDevice device in InputSystem.devices) {
            if(deviceID == device.deviceId) {
                usedDevice = device;
            }
        }

        if(usedDevice == null) {
            usedDevice = InputSystem.devices[0];
        }

        return usedDevice;
    }

    void OnPlayerJoined(PlayerInput playerInput) {
        //Debug.Log("PlayerInput Joined: " + playerInput.gameObject.name);
    }
}
