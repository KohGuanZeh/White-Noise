using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCollider : MonoBehaviour
{
    public Enemy enemy;
    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Player") {
            coll.GetComponent<PlayerController>().DepleteLamp(enemy.depleteAmt);
            if (enemy.despawnAfterCollide){
                enemy.gameObject.SetActive(false);
            }
        }
    }
}
