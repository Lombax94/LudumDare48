using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorInfo : MonoBehaviour {

    Movement player;

    private void Start() {
        player = transform.parent.parent.GetComponent<Movement>();    

    }

    public void SendInfo() {
        player.DoAttack();

    }
    public void AttackOver() {
        player.Attacked();

    }

}
