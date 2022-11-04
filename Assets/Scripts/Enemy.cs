using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour {
    [SerializeField] PlayerController player;
    [SerializeField] public float depleteAmt = 25f;
    [SerializeField] float moveSpd = 8f;
    [SerializeField] float sprintThreshhold = 10f;
    [SerializeField] float sprintMultiplier = 5f;

    [SerializeField] Collider objColl;
    [SerializeField] EnemyPoints movementPoints;
    public int currentPoint;
    public bool triggered = false;
    public AudioSource audioSource;

    void Start() {
        objColl = GetComponent<Collider>();
        audioSource = GetComponent<AudioSource>();
        player = FindObjectOfType<PlayerController>();
    }

    void OnDisable() {
        player.DestroyMic();
    }

    int GetNextPoint(int point) {
        if (Vector3.Distance(transform.position, movementPoints.GetPoint(point).position) <= 0.1f) {
            point += 1;
        }
        return point > movementPoints.points.Length ? 0 : point;
    }

    public void Trigger()
    {
        audioSource.Play();
        triggered = true;
    }

    void Update() {
        Quaternion q = Quaternion.FromToRotation(Vector3.forward, player.transform.position - transform.position);
        q.eulerAngles = new Vector3(0, q.eulerAngles.y, 0);
        transform.rotation = q;
        if (!triggered || !gameObject.activeSelf) {
            return;
        }

        currentPoint = GetNextPoint(currentPoint);
        if (currentPoint == 0) {
            gameObject.SetActive(false);
        }

        float micVolume = player.AudioInputVolumeWrapped();
        float moveSpdMultiplier = micVolume > sprintThreshhold ? sprintMultiplier : 1;
        transform.position += ((movementPoints.GetPoint(currentPoint).position - transform.position).normalized * moveSpd * moveSpdMultiplier) * Time.deltaTime;
    }
}
