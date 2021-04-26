using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerMovement : MonoBehaviour {
    
    //Tilemap VecorInt(0,0,0) == World Coordinate (0,0,0)
    public Transform Sprite;
    public SpriteRenderer Spritee;
    public Animator SpriteAnimator;
    public Stats myStats;

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
    public bool StartedRotating = false;

    public GameObject Bullet;
    public float AttackRange = 0.35f;

    /// <summary>
    /// Zomebie == True
    /// </summary>
    public bool AmZombieOrMAge = true;


    /// <summary>
    /// True == Right
    /// </summary>
    public bool MovingRightOrLeft = true;

    void Start() {
        CollisionTilemap = GameObject.Find("CollidableObjects").GetComponent<Tilemap>();
        Spritee = transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>();
        myStats = GetComponent<Stats>();
        Sprite = transform.GetChild(0);
        RotateOnTile = Vector3Int.one;

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


    public bool DoMoving = false;
    public Vector3 playerJump = Vector3.zero;

    public Vector3Int GroundBeneath = Vector3Int.zero;
    void Update() {

        if (Input.GetKeyDown(KeyCode.Escape) == true){
            Application.Quit();
        }


        if (Input.GetKeyDown(KeyCode.A) == true && StartedRotating == false && Falling == false) {
            DoMoving = true;
            MovingRightOrLeft = false;
            Spritee.flipX = false;
            SpriteAnimator.SetBool("Running", true);
            MoveDirectionSaver.x = GravityDirection.x;
            MoveDirectionSaver.y = GravityDirection.y;

            MoveDirectionSaver = (Quaternion.Euler(0, 0, -90) * MoveDirectionSaver);
            MoveDirection.x = Mathf.RoundToInt(MoveDirectionSaver.x);
            MoveDirection.y = Mathf.RoundToInt(MoveDirectionSaver.y);
                       

        }
        if (Input.GetKeyUp(KeyCode.A) == true) {
            if (Input.GetKey(KeyCode.D) == false) {
                DoMoving = false;
                SpriteAnimator.SetBool("Running", false);
                

            }

        }

        if (Input.GetKeyDown(KeyCode.D) == true && StartedRotating == false && Falling == false) {
            DoMoving = true;
            MovingRightOrLeft = true;
            Spritee.flipX = true;
            SpriteAnimator.SetBool("Running", true);

            MoveDirectionSaver.x = GravityDirection.x;
            MoveDirectionSaver.y = GravityDirection.y;

            MoveDirectionSaver = (Quaternion.Euler(0, 0, 90) * MoveDirectionSaver);
            MoveDirection.x = Mathf.RoundToInt(MoveDirectionSaver.x);
            MoveDirection.y = Mathf.RoundToInt(MoveDirectionSaver.y);

        }
        if (Input.GetKeyUp(KeyCode.D) == true) {
            if (Input.GetKey(KeyCode.A) == false) {
                DoMoving = false;
                SpriteAnimator.SetBool("Running", false);

            }

        }

        if (Input.GetKeyDown(KeyCode.Space) == true && StartedRotating == false && Falling == false) {
            playerJump = Quaternion.Euler(0, 0, 180) * GravityDirection * 2;
            Falling = true;
            MoveDirection = GravityDirection;
            SpriteAnimator.SetBool("Running", false);

        }






        VSaver1.x = Mathf.RoundToInt(transform.position.x);
        VSaver1.y = Mathf.RoundToInt(transform.position.y);






        if (Falling == true) {
            gravity += Time.deltaTime * 0.2f;
            if (gravity >= 5) {
                gravity = 5;

            }

            playerJump += Quaternion.Euler(0, 0, 0) * GravityDirection * gravity;

            CommingPosition.x = transform.position.x + (playerJump.x * Time.deltaTime );
            CommingPosition.y = transform.position.y + (playerJump.y * Time.deltaTime);

            if (TileInFront() == false) {
                    transform.position = CommingPosition;

            } else {
                ManualCollisionCheck();

            }

        } else {

            if (CollisionTilemap.HasTile(GroundBeneath) == false) {//Colliding
                MoveDirection = GravityDirection;
                RotateLeft = false;
                RotateRight = false;
                Attacked();
                RotateTime = 0;
                StartedRotating = false;
                gravity = 0;

                Sprite.transform.rotation = Quaternion.LookRotation(Vector3.forward, (Quaternion.Euler(0, 0, 180f) * GravityDirection));

                playerJump = Vector3.zero;
                MoveDirection = GravityDirection;
                SpriteAnimator.SetBool("Running", false);
                Falling = true;

            } else {

                if (RotateOnTile != Vector3Int.one && CollisionTilemap.HasTile(RotateOnTile) == false) {
                    Sprite.transform.rotation = Quaternion.LookRotation(Vector3.forward, (Quaternion.Euler(0, 0, 180f) * GravityDirection));
                    RotateRight = false;
                    RotateLeft = false;
                    RotateOnTile = Vector3Int.one;
                    RotateTime = 0;
                    StartedRotating = false;

                } else {


                    CommingPosition.x = transform.position.x + MoveDirection.x * Time.deltaTime;
                    CommingPosition.y = transform.position.y + MoveDirection.y * Time.deltaTime;

                    if (EnemyAhead() == true) {
                        Attack();

                    } else {
                        if (TileInFront() == false) {//Nothing Ahead.
                            if (TileBelow() == false) {//Nothing Below.
                                WalkAroundCorner();

                            } else {//Tile Below.
                                if (DoMoving == true) {
                                    transform.position = CommingPosition;

                                }
                                GroundBeneath.x = Mathf.RoundToInt(transform.position.x) + (GravityDirection.x);
                                GroundBeneath.y = Mathf.RoundToInt(transform.position.y) + (GravityDirection.y);


                            }

                        } else {
                            //Turn Tile
                            //DO STUFF
                            //Climbing Tile
                            ManualCollisionCheck();

                        }

                    }

                }

            }

        }

    }
    Collider2D collided;
    Collider2D collided2;
    public LayerMask targetsLayer;
    public LayerMask Obstructions;

    bool EnemyAhead() {
        VSaver1.x = MoveDirection.x;
        VSaver1.y = MoveDirection.y;

        if (Bullet != null) {
            Vector2 ho = new Vector2();

            if (VSaver1.x == 0) {
                ho.y = 1;
                ho.x = 2;
            } else {
                ho.y = 2;
                ho.x = 1;
            }

            collided = Physics2D.BoxCast(transform.position + (VSaver1 * AttackRange * 0.5f), (ho) * AttackRange, 0, Vector2.zero, 0, targetsLayer).collider;

        } else {
            collided = Physics2D.Raycast(transform.position, VSaver1, AttackRange, targetsLayer).collider;

        }


        if (collided != null) {
            if (Bullet != null) {
                collided2 = Physics2D.Raycast(transform.position, collided.transform.position - transform.position, AttackRange, Obstructions).collider;

                if (collided2 != null) {
                    if (collided2.tag == "Obscrutions") {
                        return false;

                    }
                    if (collided2.tag == "Team1" || collided2.tag == "Team2") {
                        return true;

                    }
                } else {
                    return false;
                }

            } else {
                return true;

            }

        } else {
            return false;

        }
        return false;
    }

    bool AttackStarted = false;
    void Attack() {
        if (AttackStarted == false) {
            AttackStarted = true;
            SpriteAnimator.SetBool("Attack", true);

        }

    }
    public void Attacked() {
        if (AttackStarted == true) {
            AttackStarted = false;
            SpriteAnimator.SetBool("Attack", false);

        }

    }

    public void DoAttack() {
        if (Bullet != null) {
            if (collided != null)
                Instantiate(Bullet, transform.position, Quaternion.identity, transform.parent).GetComponent<BallFly>().Setter(myStats.Attack, collided.transform, targetsLayer, Obstructions);

        } else {
            if (myStats.Health > 0) {
                if (collided != null) {
                    collided.transform.parent.GetComponent<Stats>().TakeDmg(myStats.Attack);

                }

            } else {
                GameObject.Destroy(this.gameObject);

            }

        }

    }


    void WalkAroundCorner() {
        if (RotateLeft == false && RotateRight == false) {
            VSaver1.x = Mathf.RoundToInt(transform.position.x);
            VSaver1.y = Mathf.RoundToInt(transform.position.y);

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

                if (DoMoving == true) {
                    transform.position = newpoint;

                }

            } else {
                VSaver1.x = Mathf.RoundToInt(transform.position.x);
                VSaver1.y = Mathf.RoundToInt(transform.position.y);
                StartedRotating = true;

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
                StartedRotating = false;

            }


        }

    }


    bool ManualCollisionCheck() {
        a.x = Mathf.RoundToInt(CommingPosition.x + (MoveDirection.x * offset));
        a.y = Mathf.RoundToInt(CommingPosition.y + (MoveDirection.y * offset));
        a.z = 0;

        if (CollisionTilemap.HasTile(a) == true) {//Colliding
            if (GravityDirection != MoveDirection) {
                if (RotateSprite() == true) {
                    RotateMovement();
                    GroundBeneath = a;

                }

            } else {
                RotateMovement();
                GroundBeneath = a;

            }

        }
        return false;

    }

    public Vector3Int RotateOnTile = Vector3Int.zero;

    bool RotateSprite() {

      
            if (StartedRotating == false) {
                StartedRotating = true;


                if (MovingRightOrLeft == true) {
                    RotateLeft = true;

                } else {
                    RotateRight = true;

                }


                VSaver2.x = Mathf.RoundToInt(transform.position.x) - a.x;
                VSaver2.y = Mathf.RoundToInt(transform.position.y) - a.y;
                RotateTime = 0;
                startrotation = Sprite.transform.eulerAngles.z;
                RotateOnTile = a;

                transform.position = a + (((Quaternion.Euler(0, 0, 180f) * MoveDirection) * 0.75f)) - (((Quaternion.Euler(0, 0, 180f) * GravityDirection) * 0.25f));

            } else {
                RotateTime += Time.deltaTime * 2;
                if (RotateTime >= 1) {
                    RotateTime = 1;

                }

                if (RotateLeft == true) {
                    if (Vector3.Cross(VSaver2, GravityDirection).z < 0) {
                        Sprite.transform.eulerAngles = (Quaternion.Euler(0, 0, startrotation + ((-Vector3.Angle(VSaver2, GravityDirection)) * RotateTime))).eulerAngles;

                    } else {
                        Sprite.transform.eulerAngles = (Quaternion.Euler(0, 0, startrotation + ((Vector3.Angle(VSaver2, GravityDirection)) * RotateTime))).eulerAngles;

                    }

                } else if (RotateRight == true) {
                    if (Vector3.Cross(VSaver2, GravityDirection).z < 0) {
                        Sprite.transform.eulerAngles = (Quaternion.Euler(0, 0, startrotation + ((-Vector3.Angle(VSaver2, GravityDirection)) * RotateTime))).eulerAngles;

                    } else {
                        Sprite.transform.eulerAngles = (Quaternion.Euler(0, 0, startrotation + ((Vector3.Angle(VSaver2, GravityDirection)) * RotateTime))).eulerAngles;

                    }

                }

                if (RotateTime >= 1) {
                    RotateTime = 0;
                    RotateOnTile = Vector3Int.one;
                    StartedRotating = false;
                    RotateLeft = false;
                    RotateRight = false;
                    return true;
                }

        }
        return false;
    }

    void RotateMovement() {
        if (Falling == true) {
            Falling = false;
            gravity = 0;

            if(DoMoving == true) {
                SpriteAnimator.SetBool("Running", true);
            }

            if (GravityDirection.x == 0) {
                VSaver2.x = transform.position.x - a.x;
                VSaver2.y = 0;

                transform.position = a + (Quaternion.Euler(0, 0, 180f) * GravityDirection * 0.75f) + VSaver2;

            } else {
                VSaver2.x = 0;
                VSaver2.y = transform.position.y - a.y;

                transform.position = a + (Quaternion.Euler(0, 0, 180f) * GravityDirection * 0.75f) + VSaver2;
                //    transform.position = a + (Quaternion.Euler(0, 0, 180f) * GravityDirection * 0.75f) + (Quaternion.Euler(0, 0, 90f) * GravityDirection * (transform.position.y - a.y));

            }


        }

        VSaver2.x = a.x - Mathf.RoundToInt(transform.position.x);
        VSaver2.y = a.y - Mathf.RoundToInt(transform.position.y);
        GravityDirection.x = Mathf.RoundToInt(VSaver2.x);//Setting Rotation Based On What I Landed On
        GravityDirection.y = Mathf.RoundToInt(VSaver2.y);

        MoveDirectionSaver.x = GravityDirection.x;
        MoveDirectionSaver.y = GravityDirection.y;
        if (MovingRightOrLeft == true) {
            MoveDirectionSaver = (Quaternion.Euler(0, 0, 90) * MoveDirectionSaver);
            MoveDirection.x = Mathf.RoundToInt(MoveDirectionSaver.x);
            MoveDirection.y = Mathf.RoundToInt(MoveDirectionSaver.y);

        } else {
            MoveDirectionSaver = (Quaternion.Euler(0, 0, -90) * MoveDirectionSaver);
            MoveDirection.x = Mathf.RoundToInt(MoveDirectionSaver.x);
            MoveDirection.y = Mathf.RoundToInt(MoveDirectionSaver.y);

        }


        /*  if (MoveDirection.x == 0) {//Hugging The Ground :D, Important To Update Gravity To Get Correct Way To Snap To
              VSaver1.x = transform.position.x;
              VSaver1.y = Mathf.RoundToInt(transform.position.y);

              transform.position = (VSaver2 + GravityDirection) + ((Quaternion.Euler(0, 0, 180f) * GravityDirection) * 0.75f);

          } else {
              VSaver1.x = Mathf.RoundToInt(transform.position.x);
              VSaver1.y = transform.position.y;

              transform.position = (VSaver1 + GravityDirection) + ((Quaternion.Euler(0, 0, 180f) * GravityDirection) * 0.75f);

          }*/


        /*    if (MoveDirection.x == 0) {//Hugging The Ground :D, Important To Update Gravity To Get Correct Way To Snap To
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
            return true;*/

    }

}
