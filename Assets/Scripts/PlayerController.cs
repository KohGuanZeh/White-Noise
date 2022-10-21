using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Player Controller")]
    [SerializeField] CharacterController cc;
    [SerializeField] Camera cam;

    [Header("Movement")]
    [SerializeField] Vector3 velocity;
    [SerializeField] float moveSpd = 7.5f;

    [Header("Camera")]
    [SerializeField] float yaw;
    [SerializeField] float pitch;
    [SerializeField] float horLookSpeed = 1;
    [SerializeField] float vertLookSpeed = 1;

    void Update() {
        if (cam == null) { // Guard Case
            return;
        }

        // Player Movement
        // Credits to Brackeys: https://www.youtube.com/watch?v=4HpC--2iowE
        // More control with GetAxisRaw than GetAxis
        float vert = Input.GetAxisRaw("Vertical");
        float hor = Input.GetAxisRaw("Horizontal");

        // Vector Arithmetic so direction is based Camera rotation
        Vector3 camForward = Vector3.Scale(transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(transform.right, new Vector3(1, 0, 1)).normalized;
        Vector3 dir = (vert * camForward + hor * camRight).normalized;

        yaw += horLookSpeed * Input.GetAxis("Mouse X");
		pitch -= vertLookSpeed * Input.GetAxis("Mouse Y"); //-Since 0-1 = 359 and 359 is rotation upwards;
		pitch = Mathf.Clamp(pitch, -90, 90); //Setting Angle Limits

        transform.eulerAngles = new Vector3(0, yaw, 0);
        cam.transform.localEulerAngles = new Vector3(pitch, 0, 0);

        velocity = dir * moveSpd;

        velocity.y = -9.81f;
        cc.Move(velocity * Time.deltaTime);
    }
}
