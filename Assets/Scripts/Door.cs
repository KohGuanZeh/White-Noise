using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : MonoBehaviour
{
    [SerializeField] Animator anim;

    void OnTriggerEnter(Collider coll) {
        if (coll.tag == "Player") {
            anim.SetBool("Open", true);
        }
    }

    void OnTriggerExit(Collider coll) {
        if (coll.tag == "Player") {
            anim.SetBool("Open", false);
        }
    }
}
