using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Player Variables")]
    [SerializeField] private float playerSpeed = 12f;
    [SerializeField] private float playerJumpForce = 6f;
    [SerializeField] private float playerSensitivety;
    [Header("Player Attachments")]
    [SerializeField] private Rigidbody rb;
    [SerializeField] private Transform playerCameraObject;
    [SerializeField] private Transform playerGroundObject;
    [SerializeField] private LayerMask playerFloorMask;

    private Vector3 playerMovement;
    private Vector2 cameraPlayerMovement;
    private float xRotation = 0f;

    //later add the sounds just for fun

    private void Start() {
        Cursor.lockState = CursorLockMode.Locked;
        Application.targetFrameRate = 75;
    }
    private void Update() {
        playerMovement = new Vector3(Input.GetAxis("Horizontal"), 0f, Input.GetAxis("Vertical"));
        cameraPlayerMovement = new Vector2(Input.GetAxis("Mouse X"), Input.GetAxis("Mouse Y"));

        PlayerMove();
        PlayerCameraMove();
    }

    private void PlayerMove()
    {
        Vector3 movePlayer = transform.TransformDirection(playerMovement) * playerSpeed;
        rb.velocity = new Vector3(movePlayer.x, rb.velocity.y, movePlayer.z);
        if (Input.GetKeyDown(KeyCode.Space) && Physics.CheckSphere(playerGroundObject.position, 0.1f, playerFloorMask))
        {
            rb.AddForce(Vector3.up * playerJumpForce, ForceMode.Impulse);
        }
    }

    private void PlayerCameraMove() { 
        xRotation -= cameraPlayerMovement.y * playerSensitivety;

        transform.Rotate(0f, cameraPlayerMovement.x * playerSensitivety, 0f);
        playerCameraObject.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

}
