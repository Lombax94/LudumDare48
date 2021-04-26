using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootSpawner : MonoBehaviour {



    public GameObject Bomb;

    public void IDied(Transform enemy) {
        if(Random.Range(0, 6) >= 3) {
            Instantiate(Bomb, enemy.position, Quaternion.identity, transform);
        
        } 

    } 

}
