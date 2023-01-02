using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMagnetAbility : MonoBehaviour, IAbility {


    public delegate void PushAllBalls();
    public static PushAllBalls pushAllBalls;

    private bool isMagnetActive = false;

    [SerializeField]
    private List<BallMagnetBehaviour> magnetedBalls;

    public void activateAbility() {
        isMagnetActive = true;
    }

    public void deactivateAbility() {
        isMagnetActive = false;

        foreach(BallMagnetBehaviour bb in magnetedBalls) {
            transform.parent = null;
        }

        magnetedBalls.Clear();

        pushAllBalls?.Invoke();
    }


    private void OnCollisionEnter(Collision col) {
        if(col.gameObject.GetComponent<BallMagnetBehaviour>()) {
            var curBall = col.gameObject.GetComponent<BallMagnetBehaviour>();

            if(isMagnetActive) {
                Debug.Log("MAGNEEET");
                curBall.magnetBall(col.contacts[0].point, col.contacts[0].normal, this.transform);
                
            }
        }
    }
}