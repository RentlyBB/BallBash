using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using System;


public class CartController : MonoBehaviour {

    public Transform track;

    [Header("Standart Movement")]
    [SerializeField]
    private float P_currentSpeed = 0f;

    [SerializeField]
    private float P_cartMinSpeed = 0.1f;

    [SerializeField]
    private float P_cartMaxSpeed = 7f;

    [SerializeField]
    private float P_cartAccelerationTime = 0.07f;

    [SerializeField]
    private float P_cartDecelerationTime = 0.12f;

    [Header("Boost Movement")]
    [SerializeField]
    private float P_additionalBoostSpeed = 7f;

    [SerializeField]
    private float P_additionalBoostDeceleration = 0.03f;

    [Header("Starting position on track")]
    [SerializeField]
    float railCartPosition = 0.5f;

    [SerializeField]
    private bool invertedControll = false;

    private SplineContainer trackSplineContainer;

    private Spline trackSpline;

    // Parameters for calculation
    private float targetSpeed;
    private float boostSpeed = 0;
    private float boostCartDeceleration;
    private float velSmoothTime;
    private float velocity = 0f;

    // INPUT DATA
    private bool PI_isMoving = false;
    private float PI_movemetnDirectionX;

    private void Start() {

        trackSplineContainer = track.GetComponent<SplineContainer>();
        trackSpline = trackSplineContainer.Spline;
        
        // Set starting position for player
        this.transform.position = track.TransformPoint(trackSpline.EvaluatePosition(railCartPosition));
    }

    private void Update() {

        moveCartOnSpline();
        rotateCartOnSpline();
    }

    public void moveCartOnSpline() {

        // Set up movement values based on playersMovement state (moving/not_moving)
        if(PI_isMoving) {
            targetSpeed = (P_cartMaxSpeed + boostSpeed) * PI_movemetnDirectionX;
            velSmoothTime = P_cartAccelerationTime;
        } else {
            targetSpeed = 0f;
            velSmoothTime = P_cartDecelerationTime + boostCartDeceleration;
        }

        // Applying acceleration and decceleration
        P_currentSpeed = Mathf.SmoothDamp(P_currentSpeed, targetSpeed, ref velocity, velSmoothTime);

        P_currentSpeed = roundToThreeDigit(P_currentSpeed);

        // Round down P_currentSpeed to 0
        if(Mathf.Abs(P_currentSpeed) < P_cartMinSpeed) {
            P_currentSpeed = 0f;
        }

        // Calculating new position for player movement (0..1)
        railCartPosition = Mathf.Clamp(railCartPosition + (P_currentSpeed / trackSpline.GetLength() * Time.deltaTime), 0.001f, 0.999f);

        railCartPosition = roundToThreeDigit(railCartPosition);

        // Update current player position with new one
        this.transform.position = track.TransformPoint(trackSpline.EvaluatePosition(railCartPosition));

        // In case of rewriting movement to physics based, this is the world position/direction
        // where force should face >> track.TransformDirection(trackSpline.EvaluatePosition(railCartPosition))
    }

    private void rotateCartOnSpline() {

        // Calculate up vector of the spline
        var upSplineDirection = SplineUtility.EvaluateUpVector(trackSpline, railCartPosition);

        Vector3 direction = SplineUtility.EvaluateTangent(trackSpline, railCartPosition);
        Vector3 worldDirection = track.TransformDirection(direction);

        Vector3 targetRotation = Quaternion.Euler(0, -90f, 0) * worldDirection;

        if(targetRotation != Vector3.zero) {
            this.transform.rotation = Quaternion.LookRotation(targetRotation, upSplineDirection);
        }
    }

    public void SetPlayerInput(ref PlayerInputData inputData) {

        PI_movemetnDirectionX = inputData.movementDirection.x;
        if(invertedControll) {
            PI_movemetnDirectionX *= -1;
        }
        PI_isMoving = inputData.isMoving;
    }

    public void accelerateMovement(bool accelerate) {
        if(accelerate) {
            boostSpeed = P_additionalBoostSpeed;
            boostCartDeceleration = P_additionalBoostDeceleration;
        } else { 
            boostSpeed = 0f;
            boostCartDeceleration = 0f;
        }
    }

    public void performeAbility(bool performing) {
        Debug.Log("Booom!");
    }


    private float roundToThreeDigit(float val) {
        return (float)Math.Round(val, 4);
    }
}
