﻿using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BallMovement))]
public class BallMagnetBehaviour : MonoBehaviour {

    private BallMovement ballMovement;
    private Rigidbody rb;

    private void Awake() {
        ballMovement = GetComponent<BallMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private void OnEnable() {
        BallMagnetAbility.onDemagnetBalls += demagnetBall;
    }

    private void OnDisable() {
        BallMagnetAbility.onDemagnetBalls -= demagnetBall;
    }

    public void magnetBall(Vector3 magnetPos, Vector3 pushDir, Transform parent) {
        ballMovement.changeBallDirection(pushDir);
        ballMovement.transform.position = new Vector3(magnetPos.x, ballMovement.transform.position.y, magnetPos.z);
        
        ballMovement.canApplyForce = false;
        ballMovement.changeBallRotation();
        rb.constraints = RigidbodyConstraints.FreezeAll;
        rb.velocity = Vector3.zero;
        transform.SetParent(parent);
    }

    private void demagnetBall() {
        ballMovement.setBallSpeed(0);
        transform.parent = null;
        ballMovement.canApplyForce = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
    }

    public void pushBall() {
        ballMovement.speedUpBall();
        transform.parent = null;
        ballMovement.canApplyForce = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

    }
}
