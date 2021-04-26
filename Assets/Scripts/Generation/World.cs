using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class World : MonoBehaviour {

    public Tilemap BackgroundTileMap;
    public Tilemap NonCollidableTileMap;
    public Tilemap CollidableTileMap;




    public Tilemap FromTileMap;
    public Tilemap ToTileMap;


    public Tilemap tilemap; // The Tilemap component on the gameobject where the platform should be drawn
    public TileBase tile; // The tile sprite (TileBase) to be used to draw the platform

    // Use this for initialization
    void Start() {
        GenerateWorld();
    }

    void GenerateWorld() {
        // Create a 10x10 platform of tiles
        /*  for (int x = -5; x < 5; x++) {
              for (int y = 0; y > -10; y--) {
                  tilemap.SetTile(new Vector3Int(x, y, 0), tile);
              }
          }

          RefreshCollider();*/

    //    ToTileMap.SetTile(Vector3Int.zero, null);
    //    ToTileMap.SetTile(Vector3Int.zero + Vector3Int.right, tile);


    }

    void RefreshCollider() {
        // Update the tilemap collider by disabling and enabling the TilemapCollider2D component. This is the only solution I found to refresh it during runtime.
        tilemap.gameObject.GetComponent<TilemapCollider2D>().enabled = false;
        tilemap.gameObject.GetComponent<TilemapCollider2D>().enabled = true;
    }
}
