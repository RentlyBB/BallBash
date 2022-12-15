using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerManager : Singleton<PlayerManager> {

    protected PlayerManager() { }

    [SerializeField]
    private List<GameObject> list_spawnPoints;

    [SerializeField]
    private GameObject playerTrackPrefab;

    [SerializeField]
    private GameObject playerCart;

    [SerializeField]
    private int numberOfPlayers;


    private void Awake() {

        list_spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("PlayerSpawn"));
    }

    private void Start() {
        //addPlayers();
    }

    public void setPlayerNumber(int number) {
        numberOfPlayers = number;
    }

    public void addPlayers() {
        if(list_spawnPoints != null) {
            for(int i = 0; i < numberOfPlayers; i++) {
                GameObject track = Instantiate(playerTrackPrefab, list_spawnPoints[i].transform.position, list_spawnPoints[i].transform.rotation);
                GameObject player = PlayerInput.Instantiate(playerCart, i, "Keyboard", -1, Keyboard.current).gameObject;
               // GameObject player = Instantiate(playerCart, list_spawnPoints[i].transform.position, list_spawnPoints[i].transform.rotation);
                player.name = player.name + "_P" + i;
                player.GetComponent<CartController>().track = track.transform;
            }
        }

        foreach(var player in PlayerInput.all) {
            Debug.Log("Player ID: " + player.playerIndex + ", Player Name: " + player.name + ", Player Controll Scheme: " + player.currentControlScheme);
        }
    }

    void OnPlayerJoined(PlayerInput playerInput) {
        //Debug.Log("PlayerInput Joined: " + playerInput.gameObject.name);
    }
}
