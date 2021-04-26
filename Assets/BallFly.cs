using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallFly : MonoBehaviour {

    public Vector3 ShootDirection;
    public Transform Target;
    public int Dmg;
    public int speed = 1;
    public LayerMask targets;
    public LayerMask obstacles;

    Vector3 previousPos = Vector3.zero;

    public void Setter(int dmg, Transform target, LayerMask targetlayer, LayerMask obstaleslayer) {
        Dmg = dmg; 
        Target = target;
        targets = targetlayer;
        obstacles = obstaleslayer;

    }


    // Update is called once per frame
    void Update() {

        if (Target == null) {

            transform.position += ShootDirection * Time.deltaTime * speed;
            if (Physics2D.Raycast(transform.position, ShootDirection, 0.1f, targets).collider != null) {
                if (Physics2D.Raycast(transform.position, ShootDirection, 0.1f, targets).collider.transform.GetComponent<Spawner>() == true) {
                    Physics2D.Raycast(transform.position, ShootDirection, 0.1f, targets).transform.GetComponent<Stats>().TakeDmg(Dmg);

                } else {
                Physics2D.Raycast(transform.position, ShootDirection, 0.1f, targets).transform.parent.GetComponent<Stats>().TakeDmg(Dmg);

                }
                GameObject.Destroy(this.gameObject);
                return;
            }
            if (Physics2D.Raycast(transform.position, ShootDirection, 0.1f, obstacles).collider != null) {
                GameObject.Destroy(this.gameObject);
                return;
            }
          


        } else {


            previousPos = transform.position;
            transform.position = Vector3.MoveTowards(transform.position, Target.transform.position, 1 * Time.deltaTime * speed);
            ShootDirection = (transform.position - previousPos).normalized;

            if (Physics2D.Raycast(transform.position, ShootDirection, 0.05f, obstacles).collider != null) {
                if(Physics2D.Raycast(transform.position, ShootDirection, 0.05f, obstacles).collider.tag == "Obscrutions") {
                GameObject.Destroy(this.gameObject);
                return;
                }
            }
            if (Vector3.Distance(transform.position, Target.transform.position) <= 0.05f) {
                if(Target.GetComponent<Spawner>() == true) {
                    Target.transform.GetComponent<Stats>().TakeDmg(Dmg);

                } else {
                Target.transform.parent.GetComponent<Stats>().TakeDmg(Dmg);

                }
                GameObject.Destroy(this.gameObject);
            }

        }

    }

}
