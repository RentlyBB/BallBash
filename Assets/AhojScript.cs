using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AhojScript : MonoBehaviour {
    public InputReader inputReader;



    private void OnEnable() {
        inputReader.MoveEvent += AhojVoid;
    }
    private void OnDisable() {
        inputReader.MoveEvent -= AhojVoid;
    }


    public void AhojVoid() {
        Debug.Log("Jestli to bude fungovat tak se naseru");
    }
}
