using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PickupBomb : MonoBehaviour {

    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
     if(Vector3.Distance(transform.position, player.position) <= 0.4f) {
            player.GetComponent<Stats>().Resources += 1;
            GameObject.FindGameObjectWithTag("Finish").transform.GetChild(0).GetChild(0).GetComponent<Text>().text = ""+ player.GetComponent<Stats>().Resources;
            GameObject.Destroy(gameObject);
        }   
    }
}
