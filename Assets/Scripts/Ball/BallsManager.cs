using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallsManager : MonoBehaviour {

    protected BallsManager() { }

    [SerializeField]
    private List<Transform> list_spawnPoints;

    public List<Transform> list_ballsQueue;

    [SerializeField]
    private GameObject ballPrefab;

    [SerializeField]
    private int ballCount = 5;

    [SerializeField]
    [Header("Spawn Time Range (sec)")]
    private Vector2 timeRange = new Vector2(5, 10);

    private Vector3 ballStorage = new Vector3(10, 5, 10);

    public float curTime = 0f;
    private float timeToSpawn = 0f;

    private bool isGameStarted;


    private void Awake() {

        // Get all ball's spawn points
        foreach(Transform child in transform) {
            if(child.gameObject.activeSelf) {
                list_spawnPoints.Add(child);
            }
        }

        // Create all balls for the arena
        for(int i = 0; i < ballCount; i++) {
            var tmpBall = Instantiate(ballPrefab, ballStorage, Quaternion.identity);
            tmpBall.SetActive(false);
            list_ballsQueue.Add(tmpBall.transform);
        }
    }

    private void Start() {
        setRandomSpawnTime();
        curTime = 0f;


        isGameStarted = false;
    }

    private void FixedUpdate() {
        if(list_ballsQueue.Count > 0) {
            curTime += Time.deltaTime;
            putBallToPlay();
        } else {
            curTime = 0;
        }
    }

    private void OnEnable() {
        CanvasSwitcher.onStartGame += gameStarted;
    }

    private void OnDisable() {
        CanvasSwitcher.onStartGame -= gameStarted;
    }

    private void gameStarted() {
        isGameStarted = true;
    }

    private void setRandomSpawnTime() {
        timeToSpawn = Random.Range(timeRange.x, timeRange.y);
    }

    private void putBallToPlay() {
        if(curTime > timeToSpawn && isGameStarted) {
            Transform spawnPoint = list_spawnPoints[Random.Range(0, list_spawnPoints.Count)];

            Transform ballToSpawn = list_ballsQueue.Find(ball => ball.gameObject.activeSelf == false);

            if(ballToSpawn == null) {
                return;
            }

            

            var bm = ballToSpawn.GetComponent<BallMovement>();

            bm.activateBall(spawnPoint.position, spawnPoint.localRotation, spawnPoint.transform.forward);
          
            
            //ballToSpawn.position = spawnPoint.position;
            //ballToSpawn.rotation = spawnPoint.rotation;
            //ballToSpawn.gameObject.SetActive(true);


            setRandomSpawnTime();
            curTime = 0;
        }
    }
}
