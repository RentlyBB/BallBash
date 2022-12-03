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

    private Rigidbody rb;
    private BallBounceChecker bbc;

    private bool canApplyForce = false;
    private bool canChangeVelocityDir = false;
    private Vector3 colContactNormal = Vector3.forward;

    private Transform bounceSurface;

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

        Vector3 toChange = transform.InverseTransformPoint(transform.position);


        Debug.DrawRay(transform.position, bounceDirection, Color.red);

        //Debug.DrawRay(transform.TransformPoint(toChange + offset), bounceDirection, Color.red);

        //Debug.DrawRay(transform.TransformPoint(toChange - offset), bounceDirection, Color.red);


        ////Debug.DrawRay(transform.localPosition - offset, bounceDirection, Color.red);

        //Debug.DrawRay(transform.position, rb.velocity, Color.blue);
        //Debug.DrawRay(transform.position, colContactNormal, Color.green);

    }

    private void FixedUpdate() {

        if(canChangeVelocityDir) {
            CalculateBounceDirection();
        }

        if(canApplyForce && ballIsActive) {
            rb.AddForceAtPosition(bounceDirection.normalized * ballSpeed, transform.position, ForceMode.VelocityChange);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(bounceDirection);
        }
    }


    private void CalculateBounceDirection() {

        bounceDirection = Vector3.Reflect(bounceDirection, colContactNormal);

        if(bbc.potencialBounceSurface != bounceSurface.transform) {
            bounceDirection = colContactNormal;
        } 

        if(Vector3.Angle(bounceDirection, colContactNormal) == 0) {
            Quaternion offsetBounce = Quaternion.Euler(new Vector3(0, 0, 0.05f));
            bounceDirection = bounceDirection + offsetBounce.eulerAngles;
        }

        bounceDirection.y = 0f;
        rb.velocity = bounceDirection.normalized * ballSpeed;
        canChangeVelocityDir = false;
    }


    public void activateBall() {
        ballIsActive = true;
        speedLevel = 0;
        ballSpeed = ballSpeedLevels[speedLevel];
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ;
    }

    private void resetBall() {
        canApplyForce = false;
        canChangeVelocityDir = false;
        ballIsActive = false;
        BallsManager.Instance.addBallToQueue(transform);
        rb.constraints = RigidbodyConstraints.FreezeAll;
    }

    private void OnCollisionEnter(Collision collision) {

        bounceSurface = collision.transform;

        if(bounceSurface.gameObject.CompareTag("Ground")) {
            canApplyForce = true;
            rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
        } else {
            colContactNormal = collision.contacts[0].normal;
            canChangeVelocityDir = true;
        }

        if(collision.gameObject.CompareTag("Player")) {

            if(speedLevel < (ballSpeedLevels.Length - 1)){ 
                speedLevel++;
                ballSpeed = ballSpeedLevels[speedLevel];
            }
        }
    }

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.CompareTag("Goal")) {
            resetBall();
        }
    }
}

