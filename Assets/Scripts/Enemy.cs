using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] Collider objColl;
    [SerializeField] float depleteAmt = 25f;
    [SerializeField] float moveSpd = 8f;
    public bool triggered = false;
    [SerializeField] float timeTillDespawn = 100f;

    void Start() {
        objColl = GetComponent<Collider>();
    }

    void Update() {
        if (triggered) {
            transform.Translate(transform.right * -1 * moveSpd * Time.deltaTime);
            timeTillDespawn -= Time.deltaTime;
            if (timeTillDespawn <= 0) {
                gameObject.SetActive(false);
            }
        }
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Player") {
            coll.GetComponent<PlayerController>().DepleteLamp(depleteAmt);
            objColl.enabled = false;
        }
    }
}
