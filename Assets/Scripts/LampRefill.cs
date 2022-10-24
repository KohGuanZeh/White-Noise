using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampRefill : MonoBehaviour
{
    [SerializeField] float gainAmt = 50f;

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Player") {
            coll.gameObject.GetComponent<PlayerController>().RefillLamp(gainAmt);
            gameObject.SetActive(false);
        }
    }
}
