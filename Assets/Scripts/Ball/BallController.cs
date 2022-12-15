using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    [SerializeField]
    private bool ballIsActive = false;

    [SerializeField]
    private float ballSpeed;

    [SerializeField]
    private float maxVelocity;

    public Vector3 bounceDirection;

    [SerializeField]
    private float[] ballSpeedLevels;

    [SerializeField]
    private int speedLevel = 0;

    [SerializeField]
    private Animator ballAnimator;

    [SerializeField]
    private bool canSpeedUp = true;

    private Rigidbody rb;
    private BallBounceChecker bbc;

    private bool canApplyForce = false;
    private bool canChangeVelocityDir = false;
    private Vector3 colContactNormal = Vector3.forward;

    private Collision bounceSurface;

    public Vector3 offset;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        bbc = GetComponentInChildren<BallBounceChecker>();
    }

    private void Start() {
        bounceDirection = transform.forward;
        ballAnimator.speed = ballSpeed;
    }

    private void Update() {
        // FixMe
        ballAnimator.speed = ballSpeed;

        Debug.DrawRay(transform.position, bounceDirection, Color.red);
    }

    private void FixedUpdate() {

        if(canChangeVelocityDir) {
            calculateBounceDirection();
        }

        if(canApplyForce && ballIsActive) {
            rb.AddForceAtPosition(bounceDirection.normalized * ballSpeed, transform.position, ForceMode.VelocityChange);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(bounceDirection);
        }
    }


    private void calculateBounceDirection() {

        bounceDirection = Vector3.Reflect(-bounceSurface.relativeVelocity, colContactNormal);

        if(Vector3.Angle(bounceDirection, colContactNormal) == 0) {
            Quaternion offsetBounce = Quaternion.Euler(new Vector3(0, 0, 0.05f));
            bounceDirection = bounceDirection + offsetBounce.eulerAngles;
        }

        bounceDirection.y = 0f;
        rb.velocity = bounceDirection.normalized * ballSpeed;
        canChangeVelocityDir = false;
    }

    public void changeBallDirection(Vector3 newDir) {
        bounceDirection = newDir;
        speedUpBall();
    }

    private void speedUpBall() {
        if(speedLevel < (ballSpeedLevels.Length - 1) && canSpeedUp){
            speedLevel++;
            ballSpeed = ballSpeedLevels[speedLevel];
        }
    }

    public void activateBall() {
        ballIsActive = true;
        canApplyForce = true;
        speedLevel = 0;
        ballSpeed = ballSpeedLevels[speedLevel];
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
        rb.AddForceAtPosition(bounceDirection.normalized * ballSpeed, transform.position, ForceMode.VelocityChange);
    }

    private void resetBall() {
        canApplyForce = false;
        canChangeVelocityDir = false;
        ballIsActive = false;
        BallsManager.Instance.addBallToQueue(transform);
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnCollisionEnter(Collision collision) {

        bounceSurface = collision;

        if(bounceSurface.gameObject.CompareTag("Ground")) {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        } else {
            colContactNormal = collision.contacts[0].normal;
            canChangeVelocityDir = true;
        }

        if(collision.gameObject.CompareTag("Player")) {
            speedUpBall();
        }
    }

    private void OnCollisionStay(Collision collision) {
        if(collision.transform.CompareTag("Player") && collision.contacts[0].normal != colContactNormal) {
            bounceDirection = collision.contacts[0].normal;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Goal")) {
            resetBall();
        }
    }
}

