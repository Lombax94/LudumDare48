using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Movement : MonoBehaviour {

    //Tilemap VecorInt(0,0,0) == World Coordinate (0,0,0)
    public Transform Sprite;

    public Tilemap CollisionTilemap;
    public TileBase t;

    public bool Falling = false;
    public Vector3Int GravityDirection = Vector3Int.down;//Quaternion.Euler(0,0, 90), Z 90 Is Rotate A Vector Against The Clock From 0,-1,0 To 1,0,0
    public Vector3Int MoveDirection = Vector3Int.zero;
    public Vector3 MoveDirectionSaver = Vector3.zero;

    public GameObject ShowingObject;
    public GameObject ShowingObject2;


    public Vector3Int Ground;
    public bool RotateRight = false;
    public bool RotateLeft = false;

    float offset = 0.25f;
    Vector3 newpoint = Vector3.zero;
    Vector3Int a = Vector3Int.zero;
    Vector3 VSaver1 = Vector3.zero;
    Vector3 VSaver2 = Vector3.zero;
    public Vector3 TargetPoint = Vector3.zero;

    public Vector3 StartVector = Vector3.zero;
    public Vector3 CurrentVector = Vector3.zero;
    public Vector3 RotatePoint = Vector3.zero;
    float RotateTime = 0;
    public Vector3 CommingPosition = Vector3.zero;
    float gravity = 0;
    float startrotation = 0;
    /// <summary>
    /// True == Right
    /// </summary>
    public bool MovingRightOrLeft = true;

    void Start() {
        CollisionTilemap = GameObject.Find("CollidableObjects").GetComponent<Tilemap>();
        Sprite = transform.GetChild(0);
        //        CollisionTilemap = (GameObject.Find("CollidableObjects").GetComponent<Tilemap>().HasTile(Vector3Int.zero));

        /*MoveDirectionSaver.x = GravityDirection.x;
        MoveDirectionSaver.y = GravityDirection.y;

        MoveDirectionSaver = (Quaternion.Euler(0, 0, 90) * MoveDirectionSaver);
        MoveDirection.x = Mathf.RoundToInt(MoveDirectionSaver.x);
        MoveDirection.y = Mathf.RoundToInt(MoveDirectionSaver.y);

        CollisionTilemap.SetTile(Vector3Int.right * 12, t);*/

    }


    bool TileInFront() {
        a.x = Mathf.RoundToInt(CommingPosition.x + (MoveDirection.x * offset));//Checking If Wall Ahead.
        a.y = Mathf.RoundToInt(CommingPosition.y + (MoveDirection.y * offset));

        if (CollisionTilemap.HasTile(a) == true) {
            return true;
       
        }
        return false;

    }

    bool TileBelow() {

        if (MovingRightOrLeft == true) {
            a.x = Mathf.RoundToInt(CommingPosition.x + (Quaternion.Euler(0, 0, -60) * MoveDirection).x * 0.5f);//Checking If Ground Below Exist.
            a.y = Mathf.RoundToInt(CommingPosition.y + (Quaternion.Euler(0, 0, -60) * MoveDirection).y * 0.5f);

        } else {
            a.x = Mathf.RoundToInt(CommingPosition.x + (Quaternion.Euler(0, 0, 60) * MoveDirection).x * 0.5f);//Checking If Ground Below Exist.
            a.y = Mathf.RoundToInt(CommingPosition.y + (Quaternion.Euler(0, 0, 60) * MoveDirection).y * 0.5f);

        }


        if (CollisionTilemap.HasTile(a) == true) {//Colliding
            return true;

        }
        return false;

    }




    void Update() {
       
        VSaver1.x = Mathf.RoundToInt(transform.position.x);
        VSaver1.y = Mathf.RoundToInt(transform.position.y);

        if(Falling == true) {
            gravity += Time.deltaTime * 0.2f;
            if(gravity >= 5) {
                gravity = 5;
            
            }

            CommingPosition.x = transform.position.x + (GravityDirection.x * gravity);
            CommingPosition.y = transform.position.y + (GravityDirection.y * gravity);
            
            if(TileInFront() == false) {
                transform.position = CommingPosition;

            } else {
                ManualCollisionCheck();

            }

        } else {
            CommingPosition.x = transform.position.x + MoveDirection.x * Time.deltaTime;
            CommingPosition.y = transform.position.y + MoveDirection.y * Time.deltaTime;

            if(TileInFront() == false) {//Nothing Ahead.
                if(TileBelow() == false) {//Nothing Below.
                    WalkAroundCorner();

                } else {//Tile Below.
                    transform.position = CommingPosition;

                }

            } else {
                //Turn Tile
                //DO STUFF
                //Climbing Tile
                ManualCollisionCheck();

            }

        }

    }

    void WalkAroundCorner() {
        if (RotateLeft == false && RotateRight == false) {
            if (MovingRightOrLeft == true) {
                RotatePoint = (VSaver1 + GravityDirection) + (Quaternion.Euler(0, 0, 180) * GravityDirection * 0.5f) + (Quaternion.Euler(0, 0, 90) * GravityDirection * 0.5f);
                if (Vector3.Cross((Quaternion.Euler(0, 0, 180) * GravityDirection), CommingPosition - RotatePoint).z < 0) {//Went To The Left If True, Right If False
                    Debug.Log("Rotating Right");
                    RotateRight = true;

                }

            } else {
                RotatePoint = (VSaver1 + GravityDirection) + (Quaternion.Euler(0, 0, 180) * GravityDirection * 0.5f) + (Quaternion.Euler(0, 0, -90) * GravityDirection * 0.5f);
                if (Vector3.Cross((Quaternion.Euler(0, 0, 180) * GravityDirection), CommingPosition - RotatePoint).z > 0) {//Went To The Left If True, Right If False
                    Debug.Log("Rotating Left");
                    RotateLeft = true;

                }

            }

            if (RotateLeft == false && RotateRight == false) {
                newpoint.x = transform.position.x + (MoveDirection.x * Time.deltaTime);
                newpoint.y = transform.position.y + (MoveDirection.y * Time.deltaTime);

                transform.position = newpoint;

            } else {
                VSaver1.x = Mathf.RoundToInt(transform.position.x);
                VSaver1.y = Mathf.RoundToInt(transform.position.y);

                if (RotateLeft == true) {
                    RotatePoint = (VSaver1 + GravityDirection) + (Quaternion.Euler(0, 0, 180) * GravityDirection * 0.5f) + (Quaternion.Euler(0, 0, -90) * GravityDirection * 0.5f);//Finding The CornerPoint Of The Tile Where I Want To Rotate.
                    transform.position = RotatePoint + (Quaternion.Euler(0, 0, 180) * GravityDirection * 0.25f);//Adding Additional Offset Of 25% Of Up Vector.
                    TargetPoint = RotatePoint + (Quaternion.Euler(0, 0, -90) * GravityDirection * 0.25f);//Finding Offset Point To Where The Rotation Should Go To.

                } else {
                    RotatePoint = (VSaver1 + GravityDirection) + (Quaternion.Euler(0, 0, 180) * GravityDirection * 0.5f) + (Quaternion.Euler(0, 0, 90) * GravityDirection * 0.5f);
                    transform.position = RotatePoint + (Quaternion.Euler(0, 0, 180) * GravityDirection * 0.25f);
                    TargetPoint = RotatePoint + (Quaternion.Euler(0, 0, 90) * GravityDirection * 0.25f);

                }

                startrotation = Sprite.transform.eulerAngles.z;
                StartVector = transform.position;
                CurrentVector = StartVector;

            }

        } else {
            RotateTime += Time.deltaTime * 2;
            if (RotateTime >= 1) {
                RotateTime = 1;

            }

            if (RotateLeft == true) {
                transform.position = RotatePoint + (Quaternion.Euler(0, 0, 90f * RotateTime) * (StartVector - RotatePoint));
                Sprite.transform.eulerAngles = Vector3.forward * startrotation + Vector3.back * -90 * RotateTime;

            } else if (RotateRight == true) {
                transform.position = RotatePoint + (Quaternion.Euler(0, 0, -90f * RotateTime) * (StartVector - RotatePoint));
                Sprite.transform.eulerAngles = Vector3.forward * startrotation + Vector3.back * 90 * RotateTime;
      
            }

            if (RotateTime >= 1) {
                RotateTime = 0;

                if (RotateRight == true) {
                    MoveDirectionSaver.x = GravityDirection.x;
                    MoveDirectionSaver.y = GravityDirection.y;

                    MoveDirectionSaver = (Quaternion.Euler(0, 0, -90) * MoveDirectionSaver);
                    GravityDirection.x = Mathf.RoundToInt(MoveDirectionSaver.x);
                    GravityDirection.y = Mathf.RoundToInt(MoveDirectionSaver.y);

                    MoveDirectionSaver.x = GravityDirection.x;
                    MoveDirectionSaver.y = GravityDirection.y;

                    MoveDirectionSaver = (Quaternion.Euler(0, 0, 90) * MoveDirectionSaver);
                    MoveDirection.x = Mathf.RoundToInt(MoveDirectionSaver.x);
                    MoveDirection.y = Mathf.RoundToInt(MoveDirectionSaver.y);
                }
                if (RotateLeft == true) {
                    MoveDirectionSaver.x = GravityDirection.x;
                    MoveDirectionSaver.y = GravityDirection.y;

                    MoveDirectionSaver = (Quaternion.Euler(0, 0, 90) * MoveDirectionSaver);
                    GravityDirection.x = Mathf.RoundToInt(MoveDirectionSaver.x);
                    GravityDirection.y = Mathf.RoundToInt(MoveDirectionSaver.y);

                    MoveDirectionSaver.x = GravityDirection.x;
                    MoveDirectionSaver.y = GravityDirection.y;

                    MoveDirectionSaver = (Quaternion.Euler(0, 0, -90) * MoveDirectionSaver);
                    MoveDirection.x = Mathf.RoundToInt(MoveDirectionSaver.x);
                    MoveDirection.y = Mathf.RoundToInt(MoveDirectionSaver.y);
                }

                RotateRight = false;
                RotateLeft = false;

            }

        }

    }


    bool ManualCollisionCheck() {
        a.x = Mathf.RoundToInt(CommingPosition.x + (MoveDirection.x * offset) );
        a.y = Mathf.RoundToInt(CommingPosition.y + (MoveDirection.y * offset) );

        if (CollisionTilemap.HasTile(a) == true) {//Colliding
            if(Falling == true) {
                Falling = false;
                gravity = 0;

            }

            GravityDirection = MoveDirection;

            if (MoveDirection.x == 0) {//Hugging The Ground :D, Important To Update Gravity To Get Correct Way To Snap To
                VSaver1.x = transform.position.x;
                VSaver1.y = Mathf.RoundToInt(transform.position.y);

                transform.position = (VSaver1 + GravityDirection) + ((Quaternion.Euler(0, 0, 180f) * GravityDirection) * 0.75f);
          
            } else {
                VSaver1.x = Mathf.RoundToInt(transform.position.x);
                VSaver1.y = transform.position.y;

                transform.position = (VSaver1 + GravityDirection) + ((Quaternion.Euler(0, 0, 180f) * GravityDirection) * 0.75f);

            }

            MoveDirectionSaver.x = GravityDirection.x;//Rotating New Gravity Vector To Find The New MoveDirection
            MoveDirectionSaver.y = GravityDirection.y;


            startrotation = Sprite.transform.eulerAngles.z;


            if (MovingRightOrLeft == true) {
                MoveDirectionSaver = (Quaternion.Euler(0, 0, 90) * MoveDirectionSaver);

                Sprite.transform.rotation = Quaternion.LookRotation(Vector3.forward, (Quaternion.Euler(0, 0, 180f) * GravityDirection));

                //  Sprite.transform.eulerAngles = Vector3.forward * startrotation + Vector3.back * 90;

            } else {
                MoveDirectionSaver = (Quaternion.Euler(0, 0, -90) * MoveDirectionSaver);
                Sprite.transform.eulerAngles = Vector3.forward * startrotation + Vector3.back * -90;

            }

            MoveDirection.x = Mathf.RoundToInt(MoveDirectionSaver.x);
            MoveDirection.y = Mathf.RoundToInt(MoveDirectionSaver.y);
            return true;

        } 
        return false;

    }

}
