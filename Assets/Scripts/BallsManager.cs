using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsManager : MonoBehaviour{

    public static BallsManager Instance { get; private set; }

    [SerializeField]
    private List<Transform> list_spawnPoints;

    public List<Transform> list_ballsQueue;

    [SerializeField]
    private GameObject ballPrefab;

    [SerializeField]
    private int ballCount = 5;

    [SerializeField]
    [Header("Spawn Time Range (sec)")]
    private Vector2 timeRange = new Vector2(5,10);

    private Vector3 ballStorage = new Vector3(10, 5, 10);


    public float curTime = 0f;
    private float timeToSpawn = 0f;


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

        // Create all balls for the arena
        for(int i = 0; i < ballCount; i++) {
            list_ballsQueue.Add(GameObject.Instantiate(ballPrefab, ballStorage, Quaternion.identity).transform);
        }
    }

    private void Start() {
        setRandomSpawnTime();
        curTime = 0f;
    }

    private void FixedUpdate() {
        if(list_ballsQueue.Count > 0) {
            curTime += Time.deltaTime;
            putBallToPlay();
        } else {
            curTime = 0;
        }
      
    }


    private void setRandomSpawnTime() {
        timeToSpawn = Random.Range(timeRange.x, timeRange.y);
    }


    private void putBallToPlay() {
        if(curTime > timeToSpawn) {
            Transform spawnPoint = list_spawnPoints[Random.Range(0, list_spawnPoints.Count)];
            Transform ballToSpawn = list_ballsQueue[0];
            BallController ballController = ballToSpawn.GetComponent<BallController>();
            
            list_ballsQueue.RemoveAt(0);

            ballToSpawn.position = spawnPoint.position;
            ballToSpawn.rotation = spawnPoint.rotation;
            ballController.bounceDirection = spawnPoint.forward;
            ballController.activateBall();

            setRandomSpawnTime();
            curTime = 0;
        }
    }


    public void addBallToQueue(Transform ball) {
        ball.position = ballStorage;
        list_ballsQueue.Add(ball);
    }
}
