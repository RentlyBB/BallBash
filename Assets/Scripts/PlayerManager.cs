using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instance { get; private set; }

    [SerializeField]
    private List<GameObject> list_spawnPoints;

    [SerializeField]
    private GameObject playerTrackPrefab;

    [SerializeField]
    private GameObject playerCart;

    [SerializeField]
    private int numberOfPlayers;

    private void Awake() {
        //Singleton init
        if(Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }

        list_spawnPoints.AddRange(GameObject.FindGameObjectsWithTag("PlayerSpawn"));

    }

    private void Start() {
        addPlayers();
    }

    public void addPlayers() {
        for(int i = 0; i < numberOfPlayers; i++) {
            GameObject track = Instantiate(playerTrackPrefab, list_spawnPoints[i].transform.position, list_spawnPoints[i].transform.rotation);
            GameObject player = Instantiate(playerCart, list_spawnPoints[i].transform.position, list_spawnPoints[i].transform.rotation);
            player.GetComponent<CartController>().track = track.transform;
        }
    }
}
