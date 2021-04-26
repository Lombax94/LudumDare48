using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

public class PlayerMakePath : MonoBehaviour {

    public GameObject maincam;
    public World GroundTiles;

    Dictionary<string, TileBase> Tiles = new Dictionary<string, TileBase>();
    public List<TileBase> AddOnList = new List<TileBase>();




    void Start() {
        maincam = GameObject.Find("Main Camera");
        GroundTiles = GameObject.Find("Grid").GetComponent<World>();

        Tiles.Add("00000", AddOnList[0]);
        Tiles.Add("01111", AddOnList[1]);
        Tiles.Add("10111", AddOnList[2]);
        Tiles.Add("11110", AddOnList[3]);
        Tiles.Add("11101", AddOnList[4]);
        Tiles.Add("01110", AddOnList[5]);
        Tiles.Add("10101", AddOnList[6]);
        Tiles.Add("01101", AddOnList[7]);
        Tiles.Add("00111", AddOnList[8]);
        Tiles.Add("10110", AddOnList[9]);
        Tiles.Add("11100", AddOnList[10]);
        Tiles.Add("01100", AddOnList[11]);
        Tiles.Add("00101", AddOnList[12]);
        Tiles.Add("00110", AddOnList[13]);
        Tiles.Add("10100", AddOnList[14]);
        Tiles.Add("00100", AddOnList[15]);
        Tiles.Add("11111", AddOnList[16]);

    }

    int pos0 = 0;
    int pos1 = 0;
    int pos2 = 0;
    int pos3 = 0;
    int pos4 = 0;

    Vector3Int MousePositions;

    void Update() {
        if (Input.GetKeyDown(KeyCode.LeftArrow) == true) {
            maincam.transform.position += Vector3.left;

        }
        if (Input.GetKeyDown(KeyCode.RightArrow) == true) {
            maincam.transform.position += Vector3.right;

        }
        if (Input.GetKeyDown(KeyCode.UpArrow) == true) {
            maincam.transform.position += Vector3.up;

        }
        if (Input.GetKeyDown(KeyCode.DownArrow) == true) {
            maincam.transform.position += Vector3.down;

        }



        if (Input.GetMouseButtonDown(1) == true) {

            if(GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>().Resources <= 0) {
            } else {
                GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>().Resources -= 1;
                GameObject.FindGameObjectWithTag("Finish").transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "" + GameObject.FindGameObjectWithTag("Player").GetComponent<Stats>().Resources;



                MousePositions.x = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).x);
            MousePositions.y = Mathf.RoundToInt(Camera.main.ScreenToWorldPoint(Input.mousePosition).y);
            MousePositions.z = 0;


            //  if (CollisionTilemap.HasTile(a) == true) {
            //  tilemap.SetTile(new Vector3Int(x, y, 0), tile);

            if (GroundTiles.CollidableTileMap.HasTile(MousePositions) == true) {
                GroundTiles.CollidableTileMap.SetTile(MousePositions, null);
                SelectPositions(MousePositions, GroundTiles.CollidableTileMap);

            } else if (GroundTiles.NonCollidableTileMap.HasTile(MousePositions) == true) {
                GroundTiles.NonCollidableTileMap.SetTile(MousePositions, null);
                SelectPositions(MousePositions, GroundTiles.CollidableTileMap);

            }


            }



        }


    }

    void SelectPositions(Vector3Int tilePosition, Tilemap theMap) {
        CheckToChangeSurrounding(tilePosition + Vector3Int.up, theMap);
        CheckToChangeSurrounding(tilePosition + Vector3Int.left, theMap);
        CheckToChangeSurrounding(tilePosition + Vector3Int.right, theMap);
        CheckToChangeSurrounding(tilePosition + Vector3Int.down, theMap);

    }


    void CheckToChangeSurrounding(Vector3Int tilePosition, Tilemap theMap) {

        //   Debug.Log((pos0 + "" + pos1 + "" + pos2 + "" + pos3 + "" + pos4));
        if (GroundTiles.CollidableTileMap.HasTile(tilePosition + Vector3Int.up) == true || GroundTiles.NonCollidableTileMap.HasTile(tilePosition + Vector3Int.up) == true) {
            pos0 = 1;
        } else {
            pos0 = 0;
        }

        if (GroundTiles.CollidableTileMap.HasTile(tilePosition + Vector3Int.left) == true || GroundTiles.NonCollidableTileMap.HasTile(tilePosition + Vector3Int.left) == true) {
            pos1 = 1;
        } else {
            pos1 = 0;
        }

        if (GroundTiles.CollidableTileMap.HasTile(tilePosition) == true || GroundTiles.NonCollidableTileMap.HasTile(tilePosition) == true) {
            pos2 = 1;
        } else {
            pos2 = 0;
            return;
        }

        if (GroundTiles.CollidableTileMap.HasTile(tilePosition + Vector3Int.right) == true || GroundTiles.NonCollidableTileMap.HasTile(tilePosition + Vector3Int.right) == true) {
            pos3 = 1;
        } else {
            pos3 = 0;
        }

        if (GroundTiles.CollidableTileMap.HasTile(tilePosition + Vector3Int.down) == true || GroundTiles.NonCollidableTileMap.HasTile(tilePosition + Vector3Int.down) == true) {
            pos4 = 1;
        } else {
            pos4 = 0;
        }

        GroundTiles.NonCollidableTileMap.SetTile(tilePosition, null);
        GroundTiles.CollidableTileMap.SetTile(tilePosition, Tiles[pos0 + "" + pos1 + "" + pos2 + "" + pos3 + "" + pos4]);
    
    }

}
