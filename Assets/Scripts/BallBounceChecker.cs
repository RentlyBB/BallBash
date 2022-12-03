using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallBounceChecker : MonoBehaviour{

    public Transform potencialBounceSurface;

    private void OnTriggerEnter(Collider other) {
        potencialBounceSurface = other.transform;
    }

}
