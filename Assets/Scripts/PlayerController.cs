using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Controller")]
    [SerializeField] CharacterController cc;
    [SerializeField] Camera cam;

    [Header("Audio Control")]
    [SerializeField] bool audioControl;
    [SerializeField] int sampleWin = 64; // Data to collect before clip pos
    [SerializeField] float loudnessSensibility = 50;
    [SerializeField] float interactThreshold = 0.5f;
    [SerializeField] float movementThreshold = 1.0f;
    [SerializeField] string micName;
    [SerializeField] AudioClip micClip;

    [Header("Movement")]
    [SerializeField] Vector3 velocity;
    [SerializeField] float moveSpd = 7.5f;

    [Header("Camera")]
    [SerializeField] float yaw;
    [SerializeField] float pitch;
    [SerializeField] float horLookSpeed = 1;
    [SerializeField] float vertLookSpeed = 1;

    void Start() {
        if (audioControl) {
            InitialiseMic();
        }
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
            } else if (micName != "") {
                Microphone.End(micName);
                micName = "";
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
            velocity = dir * moveSpd;
        } else {
            float inputVolume = AudioInputVolume(Microphone.GetPosition(micName), micClip) * loudnessSensibility;
            velocity = Vector3.zero;

            if (inputVolume >= movementThreshold) {
                velocity = transform.forward * moveSpd;
            } else if (inputVolume >= interactThreshold) {
                // Interact with Object
            }

            print (inputVolume);
        }

        // Set Gravity
        velocity.y = -9.81f;

        cc.Move(velocity * Time.deltaTime);
    }

    // Credits to https://www.youtube.com/watch?v=dzD0qP8viLw
    void InitialiseMic() {
        micName = Microphone.devices[0];
        micClip = Microphone.Start(micName, true, 1, AudioSettings.outputSampleRate);
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
        return volume / sampleWin;
    }
}
