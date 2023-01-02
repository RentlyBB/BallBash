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

    public void activateAbility() {
        isMagnetActive = true;
    }

    public void deactivateAbility() {
        isMagnetActive = false;

        foreach(BallMagnetBehaviour bb in magnetedBalls) {
            bb.pushBall();
        }

        magnetedBalls.Clear();
        StopCoroutine(demagnetBallsOverTime());

    }

    IEnumerator demagnetBallsOverTime() {
        yield return new WaitForSeconds(magnetTime);
        if(magnetedBalls.Count > 0) { 
            demagnetBalls();
        }

    }


    private void demagnetBalls() {
        Debug.Log("Demagnet!!");
        isMagnetActive = false;
        magnetedBalls.Clear();
        onDemagnetBalls?.Invoke();

    }


    private void OnCollisionEnter(Collision col) {
        if(col.gameObject.GetComponent<BallMagnetBehaviour>()) {
            var curBall = col.gameObject.GetComponent<BallMagnetBehaviour>();

            if(isMagnetActive) {
                curBall.magnetBall(col.contacts[0].point, col.contacts[0].normal, this.transform);
                magnetedBalls.Add(curBall);

                StartCoroutine(demagnetBallsOverTime());

            }
        }
    }
}