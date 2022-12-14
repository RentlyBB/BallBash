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

    private PlayerInputManager pim;

    private void Awake() {

        list_spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("PlayerSpawn"));
        pim = GetComponent<PlayerInputManager>();
    }

    private void Start() {
        addPlayers();
    }

    public void addPlayers() {
        for(int i = 0; i < numberOfPlayers; i++) {
            GameObject track = Instantiate(playerTrackPrefab, list_spawnPoints[i].transform.position, list_spawnPoints[i].transform.rotation);
            GameObject player = Instantiate(playerCart, list_spawnPoints[i].transform.position, list_spawnPoints[i].transform.rotation);
            player.name = player.name + "_P" + i;
            player.GetComponent<CartController>().track = track.transform;
        }
    }

    void OnPlayerJoined(PlayerInput playerInput) {
        //Debug.Log("PlayerInput Joined: " + playerInput.gameObject.name);
    }
}
