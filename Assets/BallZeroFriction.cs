using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallZeroFriction : MonoBehaviour {

    private Rigidbody rb;
    private Vector3 newVelocity;
    public float speed;
    //public float maxSpeed;
    //public float minSpeed;
    public float magnitude;

    private void Awake() {
        rb = GetComponent<Rigidbody>();
        newVelocity = transform.forward;
    }

    void Start() {
        rb.AddForce(transform.forward * 50, ForceMode.Impulse);
    }

    private void Update() {
        
    }

    void FixedUpdate() {
        //rb.AddForce(newVelocity * speed, ForceMode.VelocityChange);
        //rb.AddForceAtPosition(newVelocity * speed, transform.position, ForceMode.VelocityChange);
    
        
        //if(canChangeVelocity) {
        //    rb.velocity = newVelocity.normalized * speed;
        //    canChangeVelocity = false;
        //}

    
        magnitude = rb.velocity.magnitude;
        rb.velocity = rb.velocity.normalized * speed;
        

        Debug.DrawRay(transform.position, rb.velocity.normalized * speed, Color.red);

        //if(rb.velocity.magnitude > maxSpeed) {
        //}

        //if(rb.velocity.magnitude < minSpeed) {
        //    rb.velocity = rb.velocity.normalized * minSpeed;
        //}

        //rb.velocity = Vector3.ClampMagnitude(rb.velocity, maxSpeed);
    }

    void OnCollisionEnter(Collision collision) {
        if(!collision.gameObject.CompareTag("Ground")) {
            ContactPoint contactPoint = collision.contacts[0];
            newVelocity = Vector3.Reflect(-collision.relativeVelocity, contactPoint.normal);
            //transform.rotation = Quaternion.LookRotation(newVelocity);
        } else {
            rb.constraints = RigidbodyConstraints.FreezePositionY;
        }
    }

}
