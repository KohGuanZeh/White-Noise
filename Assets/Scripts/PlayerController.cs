using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {
    [Header("Player Controller")]
    [SerializeField] CharacterController cc;
    [SerializeField] Camera cam;

    [Header("Audio Control")]
    [SerializeField] bool audioControl;
    [SerializeField] int sampleWin = 64; // Data to collect before clip pos
    [SerializeField] float loudnessSensibility = 50;
    [SerializeField] float stopThreshold = 0.5f;
    [SerializeField] float movementThreshold = 1.0f;
    [SerializeField] float maxMovementThreshold = 1.5f;
    [SerializeField] string micName;
    [SerializeField] AudioClip micClip;
    [SerializeField] bool isMoving = false;

    [Header("Movement")]
    [SerializeField] Vector3 velocity;
    [SerializeField] float moveSpd = 0;
    [SerializeField] float minMoveSpd = 7.5f;
    [SerializeField] float maxMoveSpd = 10f;

    [Header("Camera")]
    [SerializeField] float yaw;
    [SerializeField] float pitch;
    [SerializeField] float horLookSpeed = 1;
    [SerializeField] float vertLookSpeed = 1;
    public Renderer noiseRenderer;

    [Header("Lamp")]
    [SerializeField] Lamp lamp;

    [Header("Enemy")]
    [SerializeField] float respawnTimer;
    [SerializeField] bool respawnEnemy;
    [SerializeField] Enemy enemy;
    public LightEnemyRadius lightEnemyRadius;
    public float enemiesAroundLightMinDistance = 5;
    public float enemiesAroundLightMaxDistance = 50;
    public AudioSource audioSource;

    void Start() {
        yaw = transform.rotation.eulerAngles.y;
        if (audioControl) {
            InitialiseMic();
        }
        lightEnemyRadius.Setup(lamp.light);
    }

    void Update() {
        if (cam == null) { // Guard Case
            return;
        }

        // Switch Audio Control
        if (Input.GetKeyDown(KeyCode.P)) {
            audioControl = !audioControl;

            if (audioControl) {
                InitialiseMic();
            } else {
                DestroyMic();
            }
        }

        // Player Rotation
        yaw += horLookSpeed * Input.GetAxis("Mouse X");
        pitch -= vertLookSpeed * Input.GetAxis("Mouse Y"); //-Since 0-1 = 359 and 359 is rotation upwards;
        pitch = Mathf.Clamp(pitch, -90, 90); //Setting Angle Limits

        transform.eulerAngles = new Vector3(0, yaw, 0);
        cam.transform.localEulerAngles = new Vector3(pitch, 0, 0);

        // Player Movement
        Vector3 dir = Vector3.zero;
        if (!audioControl) {
            // Credits to Brackeys: https://www.youtube.com/watch?v=4HpC--2iowE
            // More control with GetAxisRaw than GetAxis
            float vert = Input.GetAxisRaw("Vertical");
            float hor = Input.GetAxisRaw("Horizontal");

            // Vector Arithmetic so direction is based Camera rotation
            Vector3 camForward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
            Vector3 camRight = Vector3.Scale(transform.right, new Vector3(1, 0, 1)).normalized;
            dir = (vert * camForward + hor * camRight).normalized;
            velocity = dir * minMoveSpd;
        } else if (Input.GetMouseButtonDown(1)) {
            isMoving = !isMoving;
            moveSpd = isMoving ? minMoveSpd : 0;
            velocity = transform.forward * moveSpd;
        } else {
            float inputVolume = AudioInputVolume(Microphone.GetPosition(micName), micClip) * loudnessSensibility;

            if (inputVolume >= movementThreshold) {
                isMoving = true;
                moveSpd = Mathf.Lerp(moveSpd, maxMoveSpd, (inputVolume - movementThreshold) / (maxMovementThreshold - movementThreshold));
            } else if (inputVolume >= stopThreshold) {
                if (isMoving) {
                    isMoving = false;
                    moveSpd = 0;
                }
            }
            print(inputVolume);
            velocity = transform.forward * moveSpd;
        }

        // Set Gravity
        velocity.y = -9.81f;

        cc.Move(velocity * Time.deltaTime);

        float enemiesAroundNormalized = lightEnemyRadius.AllEnemiesAroundDistanceNormalized(enemiesAroundLightMinDistance, enemiesAroundLightMaxDistance);
        float normalized = Mathf.Clamp01((1 - enemiesAroundNormalized) - 0.5f) / 0.5f;
        noiseRenderer.material.SetFloat("_Opacity", normalized);
        if (normalized > 0f) {
            audioSource.volume = normalized;
            if (!audioSource.isPlaying){
                audioSource.Play();
            }
        } else if (audioSource.isPlaying) {
            audioSource.Pause();
        }

        lightEnemyRadius.UpdateLight(enemiesAroundNormalized * lamp.light.intensity);

        if (respawnEnemy && respawnTimer > 0) {
            respawnTimer -= Time.deltaTime;
            if (respawnTimer <= 0) {
                respawnEnemy = false;
                enemy.gameObject.SetActive(true);
            }
        }
    }

    // Credits to https://www.youtube.com/watch?v=dzD0qP8viLw
    void InitialiseMic() {
        if (micName == ""){
            micName = Microphone.devices[0];
            micClip = Microphone.Start(micName, true, 1, AudioSettings.outputSampleRate);
        }
    }

    public void DestroyMic() {
        if (micName != "") {
            Microphone.End(micName);
            micName = "";
        }
    }

    float AudioInputVolume(int clipPos, AudioClip input) {
        int startPos = clipPos - sampleWin;

        if (startPos < 0) {
            return 0f;
        }

        float[] audioData = new float[sampleWin];
        input.GetData(audioData, 0);

        float volume = 0;
        for (int i = 0; i < sampleWin; i++) {
            volume += Mathf.Abs(audioData[i]);
        }
        //print(volume / sampleWin);
        return volume / sampleWin;
    }

    public float AudioInputVolumeWrapped(float loudnessSensibility = 50) {
        if (micName == "") {
            InitialiseMic();
        }
        return AudioInputVolume(Microphone.GetPosition(micName), micClip) * loudnessSensibility;
    }

    public void RefillLamp(float lifeSpanGain) {
        
        lamp.gainAmt += lifeSpanGain;
    }

    public void DepleteLamp(float depleteAmt) {
        lamp.DepleteLamp(depleteAmt);
    }

    public void RespawnEnemy() {
        respawnEnemy = true;
        respawnTimer = 10f;
    }
}
