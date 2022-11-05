using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LampRefill : MonoBehaviour
{
    [SerializeField] float gainAmt = 50f;
    public AudioSource audioSource;

    void OnTriggerEnter(Collider coll) {
        if (coll.gameObject.tag == "Player") {
            AudioSource.PlayClipAtPoint(audioSource.clip, transform.position);
            coll.gameObject.GetComponent<PlayerController>().RefillLamp(gainAmt);
            gameObject.SetActive(false);
        }
    }
}
