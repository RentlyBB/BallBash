using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour {

    public static PlayerManager Instance { get; private set; }

    [SerializeField]
    private List<Transform> list_spawnPoints;

    [SerializeField]
    private GameObject[] players;

    [SerializeField]
    private Transform playerTrackPrefab;

    private void Awake() {
        //Singleton init
        if(Instance != null && Instance != this) {
            Destroy(this);
        } else {
            Instance = this;
        }

        // Get all ball's spawn points
        foreach(Transform child in transform) {
            list_spawnPoints.Add(child);
        }

        players = GameObject.FindGameObjectsWithTag("Player");

        foreach(GameObject player in players) { 
            
        }

    }

    public void addPlayer() {
        Debug.Log("Player Added");
    }
}
