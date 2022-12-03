using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour {

    [SerializeField]
    private float ballSpeed;

    [SerializeField]
    private float maxVelocity;

    private Vector3 bounceDirection;

    [SerializeField]
    private float[] ballSpeedLevels;

    [SerializeField]
    private int speedLevel = 0;

    [SerializeField]
    private Animator ballAnimator;

    private Rigidbody rb;

    private bool canApplyForce = false;
    private bool canChangeVelocityDir = false;
    private Vector3 colContactNormal = Vector3.forward;

    private Transform bounceSurface;

    public Vector3 offset;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
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

        Debug.DrawRay(transform.TransformPoint(toChange + offset), bounceDirection, Color.red);

        Debug.DrawRay(transform.TransformPoint(toChange - offset), bounceDirection, Color.red);

        //Debug.DrawRay(transform.localPosition - offset, bounceDirection, Color.red);

        //Debug.DrawRay(transform.position, rb.velocity, Color.blue);
        //Debug.DrawRay(transform.position, colContactNormal, Color.green);

    }

    private void FixedUpdate() {

        if(canChangeVelocityDir) {
            CalculateBounceDirection();
        }

        if(canApplyForce) {
            rb.AddForceAtPosition(bounceDirection.normalized * ballSpeed, transform.position, ForceMode.VelocityChange);
            rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxVelocity);

      
            // Calculate a rotation a step closer to the target and applies rotation to this object
            transform.rotation = Quaternion.LookRotation(bounceDirection);
        }
    }


    private void CalculateBounceDirection() {
        //Debug.Log(Vector3.Angle(bounceDirection, colContactNormal));
        Vector3 toChange = transform.InverseTransformPoint(transform.position);
        RaycastHit hit;
        // Does the ray intersect any objects excluding the player layer
        if(Physics.Raycast(transform.position, bounceDirection, out hit, 1f) && hit.transform == bounceSurface.transform) {
            bounceDirection = Vector3.Reflect(bounceDirection, colContactNormal);
        } else if(Physics.Raycast(transform.TransformPoint(toChange + offset), bounceDirection, out hit, 1f) && hit.transform == bounceSurface.transform) {
            bounceDirection = Vector3.Reflect(bounceDirection, colContactNormal);
        } else if(Physics.Raycast(transform.TransformPoint(toChange - offset), bounceDirection, out hit, 1f) && hit.transform == bounceSurface.transform) {
            bounceDirection = Vector3.Reflect(bounceDirection, colContactNormal);
        } else {
            bounceDirection = colContactNormal;
            //bounceDirection = Vector3.Reflect(colContactNormal, bounceDirection);
        }

       

        bounceDirection.y = 0f;
        rb.velocity = bounceDirection.normalized * ballSpeed;
        canChangeVelocityDir = false;
    }

    private void OnCollisionEnter(Collision collision) {

        bounceSurface = collision.transform;

        if(bounceSurface.gameObject.CompareTag("Ground")) {
            canApplyForce = true;
            rb.constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezeRotationY;
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
            Destroy(gameObject);
        }
    }
}

