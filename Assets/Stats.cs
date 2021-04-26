using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour {

    public int Health = 10;
    public int Attack = 1;

    public int Resources = 0;

    public void TakeDmg(int dmg) {
        Health -= dmg;
        if (Health <= 0) {
            GameObject.FindGameObjectWithTag("Respawn").GetComponent<LootSpawner>().IDied(transform);
            GameObject.Destroy(this.gameObject);

        }
    }


}
