using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMagnetAbility : MonoBehaviour, IAbility {

    public delegate void DemagnetBalls();
    public static event DemagnetBalls onDemagnetBalls;

    private bool isMagnetActive = false;

    public float magnetTime = 5;

    [SerializeField]
    private List<BallMagnetBehaviour> magnetedBalls;

    [SerializeField]
    private List<BallMagnetBehaviour> ballsInMagetZone;

    public void activateAbility() {
        isMagnetActive = true;
    }

    public void deactivateAbility() {
        isMagnetActive = false;

        foreach(BallMagnetBehaviour bb in magnetedBalls) {
            bb.pushBall();
        }

        magnetedBalls.Clear();
    }

    private void demagnetBalls() {
        Debug.Log("Demagnet!!");
        isMagnetActive = false;
        magnetedBalls.Clear();
        onDemagnetBalls?.Invoke();

    }

    public int objectsInMagnetField = 0;

    private void OnTriggerEnter(Collider other) {

        if(other.gameObject.GetComponent<BallMagnetBehaviour>()) {
            var curBall = other.gameObject.GetComponent<BallMagnetBehaviour>();
            ballsInMagetZone.Add(curBall);
        }
    }


    private void OnTriggerExit(Collider other) {
        if(other.gameObject.GetComponent<BallMagnetBehaviour>()) {
            var curBall = other.gameObject.GetComponent<BallMagnetBehaviour>();
            if(ballsInMagetZone.Contains(curBall)) { 
                ballsInMagetZone.Remove(curBall);
            }
        }
    }


    private void OnCollisionEnter(Collision col) {
        if(col.gameObject.GetComponent<BallMagnetBehaviour>()) {
            var curBall = col.gameObject.GetComponent<BallMagnetBehaviour>();

            var contactPoint = col.contacts[0].point;
            var contactNormal = col.contacts[0].normal;

            if(isMagnetActive && ballsInMagetZone.Contains(curBall)) {
                curBall.magnetBall(contactPoint, contactNormal, transform);
                magnetedBalls.Add(curBall);
            }
        }
    }
}