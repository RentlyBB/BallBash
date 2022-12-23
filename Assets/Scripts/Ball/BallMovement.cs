using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovement : MonoBehaviour {

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

    private bool canApplyForce = false;
    private bool canChangeVelocityDir = false;
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
        resetBall();
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
        
        if(canApplyForce && canApplyForce) {
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
        speedLevel = 0;
        ballSpeed = ballSpeedLevels[speedLevel];
        bounceDirection = transform.forward;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    private void resetBall() {
        canApplyForce = false;
        canChangeVelocityDir = false;
    }

    public void disableBall() { 
        
    }


    private void OnCollisionEnter(Collision collision) {

        bounceSurface = collision;

        if(bounceSurface.gameObject.CompareTag("Ground")) {
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
            canApplyForce = true;
        } else {
            colContactNormal = collision.contacts[0].normal;
            canChangeVelocityDir = true;
        }

        if(collision.transform.GetComponent<CartController>()) {
            speedUpBall();
        }
    }

    private void OnCollisionStay(Collision collision) {
        if(collision.transform.GetComponent<CartController>() && collision.contacts[0].normal != colContactNormal) {
            bounceDirection = collision.contacts[0].normal;
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Goal")) {
            gameObject.SetActive(false);
        }
    }
}

