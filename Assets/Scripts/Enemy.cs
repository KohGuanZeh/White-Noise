using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] PlayerController player;
    [SerializeField] float depleteAmt = 25f;
    [SerializeField] float moveSpd = 8f;

    [Header("Simple Enemy")]
    [SerializeField] Collider objColl;
    public bool triggered = false;
    [SerializeField] bool simple = false;

    [Header("New Behaviour")]
    [SerializeField] float timeTillDespawn = 100f;
    [SerializeField] bool doNotDespawn = false;
    [SerializeField] Transform spawnPoint;

    void Start() {
        objColl = GetComponent<Collider>();
        player = FindObjectOfType<PlayerController>();
    }

    void Update() {
        if (simple && triggered) {
            transform.Translate(transform.right * -1 * moveSpd * Time.deltaTime);
            timeTillDespawn -= Time.deltaTime;
            if (timeTillDespawn <= 0) {
                gameObject.SetActive(false);
            }
        } else {
            if (!doNotDespawn) {
                timeTillDespawn -= Time.deltaTime;
                if (timeTillDespawn <= 0) {
                    gameObject.SetActive(false);
                }
            }

            Vector3 dir = player.transform.position - transform.position;
            dir.y = 0;
            // Will have to adjust direaction accordingly
            transform.Translate(dir.normalized * moveSpd * Time.deltaTime);
        }
    }

    public void SetEnemyAttributes(float time, bool despawn, Transform spawnLoc) {
        timeTillDespawn = time;
        doNotDespawn = despawn;
        spawnPoint = spawnLoc;
    }

    // Usually for do not despawn and upon encountering player
    public void Respawn() {
        transform.position = spawnPoint.position;
    }

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Player") {
            coll.GetComponent<PlayerController>().DepleteLamp(depleteAmt);
            if (simple) {
                objColl.enabled = false;
            } else {
                // May want to respawn after certain timer
                if (doNotDespawn) {
                    player.RespawnEnemy();
                }
                gameObject.SetActive(false);
            }
        }
    }
}
