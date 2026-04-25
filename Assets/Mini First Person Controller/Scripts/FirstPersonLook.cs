using UnityEngine;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] Transform character;

    [Header("Mouse Settings")]
    public float sensitivity = 2f;
    public float smoothing = 10f;

    [Header("Focus")]
    [SerializeField] private Transform lookTarget;
    [SerializeField] private float focusSpeed = 5f;
    private bool isFocusing = false;

    float yaw;
    float pitch;

    float targetYaw;
    float targetPitch;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;

        // Initialize from current rotation
        Vector3 forward = transform.forward;
        yaw = Mathf.Atan2(forward.x, forward.z) * Mathf.Rad2Deg;
        pitch = -Mathf.Asin(forward.y) * Mathf.Rad2Deg;

        targetYaw = yaw;
        targetPitch = pitch;
    }

    public void FocusOn(Transform target)
    {
        lookTarget = target;
        isFocusing = true;
    }

    public void StopFocus()
    {
        isFocusing = false;
    }

    void Update()
    {
        if (isFocusing && lookTarget != null)
        {
            Vector3 dir = (lookTarget.position - transform.position).normalized;

            targetYaw = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;
            targetPitch = -Mathf.Asin(dir.y) * Mathf.Rad2Deg;
        }
        else
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * sensitivity;
            float mouseY = Input.GetAxisRaw("Mouse Y") * sensitivity;

            targetYaw += mouseX;
            targetPitch -= mouseY;
            targetPitch = Mathf.Clamp(targetPitch, -90f, 90f);
        }

        float rotationSpeed = 720f; // degrees per second

        yaw = Mathf.MoveTowards(yaw, targetYaw, rotationSpeed * Time.deltaTime);
        pitch = Mathf.MoveTowards(pitch, targetPitch, rotationSpeed * Time.deltaTime);

        // Apply rotations
        character.localRotation = Quaternion.Euler(0f, yaw, 0f);
        transform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
    }
}