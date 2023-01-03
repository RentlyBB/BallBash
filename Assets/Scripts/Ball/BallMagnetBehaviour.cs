using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BallMovement))]
public class BallMagnetBehaviour : MonoBehaviour {

    private BallMovement bm;
    private Rigidbody rb;

    private Vector3 normal;

    public bool isMagneted { get; private set; }

    private void Awake() {
        bm = GetComponent<BallMovement>();
        rb = GetComponent<Rigidbody>();
    }

    private void Update() {
        Debug.DrawRay(transform.position, normal, Color.green);
    }

    private void OnEnable() {
        BallMagnetAbility.onDemagnetBalls += demagnetBall;
    }

    private void OnDisable() {
        BallMagnetAbility.onDemagnetBalls -= demagnetBall;
    }

    public void magnetBall(Vector3 magnetPos, Vector3 pushDir, Transform parent) {

        normal = pushDir * -1;
        bm.canChangeVelocity = false;
        bm.canApplyForce = false;
        rb.velocity = Vector3.zero;
        rb.constraints = RigidbodyConstraints.FreezeAll;

        bm.changeBallDirection(normal);
        bm.changeBallRotation();
        transform.SetParent(parent);

        isMagneted = true;
    }

    private void demagnetBall() {
        bm.setBallSpeed(0);
        transform.parent = null;
        bm.canApplyForce = true;
       // rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ | RigidbodyConstraints.FreezePositionY;
    }

    public void pushBall() {
        bm.speedUpBall();
        transform.parent = null;
        bm.canApplyForce = true;
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        isMagneted = false;
    }

    private void OnCollisionStay(Collision collision) {

        //if(collision.gameObject.GetComponent<IAbility>() != null && isMagneted) {
        //    transform.position += -transform.forward * Time.deltaTime * 1.05f;
        //}
    }
}
