using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallMovementV2 : MonoBehaviour {

    private Vector3 ballDirection;

    private Rigidbody rb;

    [SerializeField]
    private float gravity;

    [SerializeField]
    private float speed;

    [SerializeField]
    private float maxSpeed;

    public Vector3 colContactNormal { get; private set; }

    private void Awake() {
        rb = GetComponent<Rigidbody>();
    }

    private void Start() {
        ballDirection = transform.forward;
    }

    private void Update() {
        Debug.DrawRay(transform.position, ballDirection, Color.red);
    }

    void FixedUpdate(){

        if(Vector3.Angle(ballDirection, colContactNormal) == 0) {
            Quaternion offsetBounce = Quaternion.Euler(new Vector3(0, 0, 0.05f));
            ballDirection = ballDirection + offsetBounce.eulerAngles;
        }

        transform.rotation = Quaternion.LookRotation(ballDirection);
        //rb.AddForceAtPosition(ballDirection.normalized * speed, transform.position, ForceMode.VelocityChange);
        //rb.AddForce(ballDirection.normalized * speed, ForceMode.VelocityChange);
        //rb.maxLinearVelocity = maxSpeed;
        rb.velocity = ballDirection.normalized * speed;
        processGravity();
    }


    private void processGravity() {
        rb.AddForce(-transform.up.normalized * gravity * (rb.mass));
    } 

    private void OnCollisionEnter(Collision col) {
        if(!col.gameObject.CompareTag("Ground")) {
            colContactNormal = col.contacts[0].normal;
            ballDirection = Vector3.Reflect(-col.relativeVelocity, colContactNormal);
        }
    }


    private void OnCollisionStay(Collision collision) {
        if(collision.transform.GetComponent<CartController>() && collision.contacts[0].normal != colContactNormal) {
            ballDirection = collision.contacts[0].normal;
        }
    }
}
