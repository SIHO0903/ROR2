using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [Header("References")]
    public Transform orientation;
    public Transform player;
    public Transform playerObj;
    public Rigidbody rigid;

    public float rotationSpeed;
    float horizontalInput;
    [HideInInspector] public float verticalInput;
    private void Start()
    {
        CursorOnOff(true);
    }
    private void Update()
    {
        Vector3 viewDir = player.position - new Vector3(transform.position.x, player.position.y, transform.position.z);
        orientation.forward = viewDir.normalized;

        bool wKey = Input.GetKey(KeySoundSetManager.instance.keyValues[KeyAction.UP]);
        bool sKey = Input.GetKey(KeySoundSetManager.instance.keyValues[KeyAction.DOWN]);
        bool aKey = Input.GetKey(KeySoundSetManager.instance.keyValues[KeyAction.LEFT]);
        bool dKey = Input.GetKey(KeySoundSetManager.instance.keyValues[KeyAction.RIGHT]);

        horizontalInput = (aKey ? -1f : 0f) + (dKey ? 1f : 0f);
        verticalInput = (sKey ? -1f : 0f) + (wKey ? 1f : 0f);

        //horizontalInput = Input.GetAxis("Horizontal");
        //verticalInput = Input.GetAxis("Vertical");
        Vector3 inputDir = orientation.forward * verticalInput + orientation.right * horizontalInput;

        if(inputDir != Vector3.zero)
            playerObj.forward = Vector3.Slerp(playerObj.forward,inputDir.normalized,Time.deltaTime*rotationSpeed);
    }
    public static void CursorOnOff(bool tf)
    {
        if (tf)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
            Debug.Log("커서안보임");
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Debug.Log("커서보임");
        }

    }


}
