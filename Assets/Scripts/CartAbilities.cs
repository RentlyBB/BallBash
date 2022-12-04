using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartAbilities : MonoBehaviour {

    public float radius;

    [Range(0, 360)]
    public float angle;

    public LayerMask targetMask;

    public Vector3 lastDir;

    public bool canSeeBall = false;

    public Collider[] rayData;

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
                    target.GetComponent<BallController>().changeBallDirection(directionToTarget);
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
}
