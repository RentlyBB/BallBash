using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu]
public class PlayerDataSO : ScriptableObject {


    public int playerId;

    public int health;
       
    public bool isDead = false;
    public bool inPlay = false;


    public void decreasedHealth(int amount) {
        health -= amount;
        if(health <= 0) {
            isDead = true;
        }

    }
}
