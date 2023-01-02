using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushBallsAbility : MonoBehaviour, IAbility {

    //PushBalls ability variables
    public float radius;

    [Range(0, 360)]
    public float angle;

    public LayerMask targetMask;

    [HideInInspector]
    public bool canSeeBall { get; private set; } = false;

    [HideInInspector]
    public Collider[] rayData { get; private set; }


    private void Start() {
        //Debug in editor
        StartCoroutine(FOVRoutine());
    }

    private IEnumerator FOVRoutine() {
        WaitForSeconds wait = new WaitForSeconds(0.1f);
        while(true) {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    public void pushBallAway() {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);

        if(rangeChecks.Length != 0) {

            foreach(Collider colider in rangeChecks) {
                Transform target = colider.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if(Vector3.Angle(transform.forward, directionToTarget) < angle / 2) {
                    target.GetComponent<BallMovement>().changeBallDirection(directionToTarget);
                    target.GetComponent<BallMovement>().speedUpBall();
                }
            }
        }
    }

    public void FieldOfViewCheck() {
        rayData = Physics.OverlapSphere(transform.position, radius, targetMask);

        if(rayData.Length != 0) {

            foreach(Collider colider in rayData) {
                Transform target = colider.transform;
                Vector3 directionToTarget = (target.position - transform.position).normalized;

                if(Vector3.Angle(transform.forward, directionToTarget) < angle / 2) {
                    canSeeBall = true;
                } else {
                    canSeeBall = false;
                }
            }
        } else if(canSeeBall) {
            canSeeBall = false;
        }
    }

    public void activateAbility() {
        pushBallAway();
    }

    public void deactivateAbility() {
    }
}

