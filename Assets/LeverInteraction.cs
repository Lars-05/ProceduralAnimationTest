using UnityEngine;
using UnityEngine.Events;

public class LeverInteraction : MonoBehaviour, IInteractable
{
    [Header("Lever Setup")]
    [SerializeField] private Transform leverTransform;
    [SerializeField] private Transform leverGrabPoint;
    [SerializeField] private float sensitivityMultiplier = 1f;

    [Header("Rotation Settings")]
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private Vector3 defaultRotation;

    public UnityEvent onLeverFlipped;      
    public UnityEvent onLeverUnflipped;  

    public bool isGrabbed { get; set; }
    public Transform grabPoint { get; set; }

    private float leverValue = 0f;
    private bool wasFullyFlipped = false;

    void Awake()
    {
        grabPoint = leverGrabPoint;
    }

    public void OnGrabbed() => isGrabbed = true;
    public void OnReleased() => isGrabbed = false;

    public void Interact(float delta)
    {
        leverValue += delta * sensitivityMultiplier;
        leverValue = Mathf.Clamp01(leverValue);

        Quaternion startRot = Quaternion.Euler(defaultRotation);
        Quaternion endRot = Quaternion.Euler(targetRotation);

        leverTransform.localRotation =
            Quaternion.Slerp(startRot, endRot, leverValue);

        bool isFullyFlipped = leverValue >= 0.99f;

        if (isFullyFlipped && !wasFullyFlipped)
            onLeverFlipped?.Invoke();

        if (!isFullyFlipped && wasFullyFlipped)
            onLeverUnflipped?.Invoke();

        wasFullyFlipped = isFullyFlipped;
    }
}