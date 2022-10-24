using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTrap : MonoBehaviour
{
    [SerializeField] Collider objColl;
    [SerializeField] Enemy enemy;

    void Start() {
        objColl = GetComponent<Collider>();
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.tag == "Player") {
            enemy.triggered = true;
            objColl.enabled = false;
        }
    }
}
