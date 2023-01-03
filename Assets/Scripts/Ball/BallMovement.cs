using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

    public const RigidbodyConstraints active = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;

    [SerializeField]
    private float ballSpeed;

    [SerializeField]
    private float maxVelocity;

    [SerializeField]
    private float[] ballSpeedLevels;

    [SerializeField]
    private int speedLevel = 0;

    [SerializeField]
    private Animator ballAnimator;

    [SerializeField]
    private bool canSpeedUp = true;

    [HideInInspector]
    public Rigidbody rb;

    [HideInInspector]
    public bool canApplyForce = false;

    private Vector3 ballMovementDirection;

    private Vector3 colContactNormal = Vector3.forward;

    private Collision bounceSurface;

    public Vector3 offset;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        ballAnimator.speed = ballSpeed;
    }

    private void OnEnable() {
        activateBall();
    }

    private void OnDisable() {
        freezeBall();
    }

    private void Update() {
        // FixMe
        ballAnimator.speed = ballSpeed;
        Debug.DrawRay(transform.position, ballMovementDirection, Color.red);
    }

    private void FixedUpdate() {
        calculateBounceDirection();
        processMovement();
    }

    private void calculateBounceDirection() {

        if(bounceSurface != null) {
            ballMovementDirection = Vector3.Reflect(-bounceSurface.relativeVelocity, colContactNormal);
        }

        if(Vector3.Angle(ballMovementDirection, colContactNormal) == 0) {
            Quaternion offsetBounce = Quaternion.Euler(new Vector3(0, 0, 0.05f));
            ballMovementDirection += offsetBounce.eulerAngles;
        }

        ballMovementDirection.y = 0f;
        //rb.velocity = ballMovementDirection.normalized * ballSpeed;
    }

    private void processMovement() {
        if(!canApplyForce) return;

        // Calculate a rotation a step closer to the target and applies rotation to this object
        transform.rotation = Quaternion.LookRotation(ballMovementDirection);

        rb.AddForceAtPosition(ballMovementDirection.normalized * ballSpeed, transform.position, ForceMode.VelocityChange);
        rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);
    }

    public void speedUpBall() {
        if(speedLevel < (ballSpeedLevels.Length - 1) && canSpeedUp){
            speedLevel++;
            ballSpeed = ballSpeedLevels[speedLevel];
        }
    }

    public void activateBall() {
        setBallSpeed(0);
        changeBallDirection(transform.forward);
        changeBallRotation();
        rb.AddForce(ballMovementDirection * ballSpeed, ForceMode.Impulse);
        //canApplyForce = true;
    }

    public void setBallSpeed(int levelOfSpeed) {
        speedLevel = levelOfSpeed;
        ballSpeed = ballSpeedLevels[speedLevel];
    }

    public void freezeBall() {
        canApplyForce = false;
    }

    public void unfreezeBall() { 
        
    }

    public void changeBallDirection(Vector3 newDir) {
        ballMovementDirection = newDir;
    }

    public void changeBallRotation() {
        transform.rotation = Quaternion.LookRotation(ballMovementDirection);
    }

    private void OnCollisionEnter(Collision collision) {

        if(collision.gameObject.CompareTag("Ground")) {
            canApplyForce = true;
        } else {
            if(canApplyForce) {
                bounceSurface = collision;
                colContactNormal = collision.contacts[0].normal;
            }
        }

        if(collision.transform.GetComponent<CartController>()) {
            speedUpBall();
        }
    }

    private void OnCollisionStay(Collision collision) {
        if(collision.transform.GetComponent<CartController>() && collision.contacts[0].normal != colContactNormal) {
            ballMovementDirection = collision.contacts[0].normal;
        }
    }
}

