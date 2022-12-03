using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;
using UnityEngine.InputSystem;
using System;

public class TrackController : MonoBehaviour, ISplineProvider {

    // cart/player's rail
    [SerializeField]
    private Spline cartRail;

    // players tranform
    [SerializeField]
    private Transform P_cart;

    // players movement speed

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

    private bool P_isMoving = false;

    private float targetSpeed;
    private float velSmoothTime;

    private float lastDir;

    private PlayerInputs inputActions;

    // Starting position on rail (middle of the rail)
    [SerializeField]
    float railCartPosition = 0.5f;

    // rail tranform
    Transform T_rail;

    public IEnumerable<Spline> Splines => new[] { cartRail };



    private void Awake() {
        T_rail = transform;

        inputActions = new PlayerInputs();
        inputActions.Gameplay.Enable();
        inputActions.Gameplay.Movement.canceled += Movement_canceled;
        inputActions.Gameplay.Movement.performed += Movement_performed;

    }

    private void Start() {
        P_cart.position = T_rail.TransformPoint(cartRail.EvaluatePosition(railCartPosition));
    }

    float velocity;
    private void Update() {

        moveCartOnSpline(P_isMoving);

        rotateCartOnSpline();
    }


    private void Movement_canceled(InputAction.CallbackContext ctx) {
        P_isMoving = false;
    }

    private void Movement_performed(InputAction.CallbackContext ctx) {
        lastDir = ctx.ReadValue<Vector2>().x;
        P_isMoving = true;
    }

    private void moveCartOnSpline(bool P_isMoving) {

        // Set up movement values based on playersMovement state (moving/not_moving)
        if(P_isMoving) {
            targetSpeed = P_cartMaxSpeed * lastDir;
            velSmoothTime = P_cartAccelerationTime;
        }else { 
            targetSpeed = 0f;
            velSmoothTime = P_cartDecelerationTime;
        }

        // Applying acceleration and decceleration
        P_currentSpeed = Mathf.SmoothDamp(P_currentSpeed, targetSpeed, ref velocity, velSmoothTime);

        P_currentSpeed = roundToThreeDigit(P_currentSpeed);

        // Round down P_currentSpeed to 0
        if(Mathf.Abs(P_currentSpeed) < P_cartMinSpeed) {
            P_currentSpeed = 0f;
        }

        // Calculating new position for player movement (0..1)
        railCartPosition = Mathf.Clamp(railCartPosition + (P_currentSpeed / cartRail.GetLength() * Time.deltaTime), 0f, 1f);

        railCartPosition = roundToThreeDigit(railCartPosition);


        // Update current player position with new one
        P_cart.position = T_rail.TransformPoint(cartRail.EvaluatePosition(railCartPosition));

        // Reset P_currentSpeed to 0f if player reach the edge of the spline.
        // Must check after applying movement to P_cart.position because otherwise
        // player will stuck at the end of the spline
        if(railCartPosition == 1 || railCartPosition == 0) {
            P_currentSpeed = 0f;
        }
    }

    private void rotateCartOnSpline() {

        // Calculate up vector of the spline
        var upSplineDirection = SplineUtility.EvaluateUpVector(cartRail, railCartPosition);

        Vector3 direction = SplineUtility.EvaluateTangent(cartRail, railCartPosition);
        Vector3 worldDirection = T_rail.TransformDirection(direction);
        Vector3 targetRotation = Quaternion.Euler(0, -90f, 0) * worldDirection;

        if(targetRotation != Vector3.zero) {
            P_cart.rotation = Quaternion.LookRotation(targetRotation, upSplineDirection);
        }
    }

    private float roundToThreeDigit(float val) { 
        
        return (float)Math.Round(val, 3);
    }
}
