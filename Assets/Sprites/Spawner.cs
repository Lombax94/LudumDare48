using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public World thegrid;
    public GameObject zombie;
    public GameObject mage;
    public Vector3Int Gravity = Vector3Int.zero;
    public Vector3Int MoveDirection = Vector3Int.zero;
    public Vector3Int groundTile = Vector3Int.zero;

    public bool NeedVisiting = true;

    public float SpawnTime = 10;
    float counter = 0;

    Movement unit;
    Transform player;

    public int unitpower = 0;

    private void Start() {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        thegrid = GameObject.Find("Grid").GetComponent<World>();

        groundTile.x = Mathf.RoundToInt(transform.position.x) + Gravity.x;
        groundTile.y = Mathf.RoundToInt(transform.position.y) + Gravity.y;
    }

    private void Update() {

        if(thegrid.CollidableTileMap.HasTile(groundTile) == false) {
            GameObject.Destroy(transform.parent.gameObject);
        }

        if (NeedVisiting == true) {
            if (Vector3.Distance(transform.position, player.transform.position) < 0.25f) {
                NeedVisiting = false;
                counter = SpawnTime - 2;
            }

        } else {


            counter += Time.deltaTime;

            if (counter >= SpawnTime) {
                counter = 0;

                if(Random.Range(0,11) < unitpower) {
                    unit = Instantiate(mage, transform.position, Quaternion.identity, transform).GetComponent<Movement>();

                } else {
                unit = Instantiate(zombie, transform.position, Quaternion.identity, transform).GetComponent<Movement>();

                }
               
                unit.GravityDirection = Gravity;
                unit.MoveDirection = MoveDirection;
                unit.transform.eulerAngles = transform.eulerAngles;

            }

        }


    }





}
