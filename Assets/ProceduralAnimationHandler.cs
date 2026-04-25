using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

public class ProceduralAnimationHandler : MonoBehaviour
{
    [SerializeField] private TwoBoneIKConstraint ikConstraint;
    [SerializeField] private InteractableDetector interactableDetector;
    [SerializeField] private float grabSpeed = 2f;
    [SerializeField] private FirstPersonLook firstPersonLook;
    [SerializeField] private FirstPersonMovement firstPersonMovement;
    [SerializeField] private AnimationClip grabAnimation;
    [SerializeField] private Animator animator;
    

    private bool grabbedInteractable = false;
    private float currentWeight;
    public float totalDrag;

    float SpeedOverride() => 0;

    void Update()
    {
        IInteractable interactable = interactableDetector.GetClosestInteractable();

        if (interactable == null)
            return;

        ikConstraint.data.target.position = interactable.grabPoint.position;

        bool mouseHeld = Input.GetMouseButton(0);
        bool reachable = IsTargetReachable();

        float targetWeight = (mouseHeld && reachable) ? 1f : 0f;

       
        currentWeight = Mathf.MoveTowards(currentWeight, targetWeight, grabSpeed * Time.deltaTime);
        ikConstraint.weight = currentWeight;

        bool wasGrabbed = grabbedInteractable;
      
        grabbedInteractable = currentWeight >= 0.99f;
        
        animator.SetFloat("Blend",  currentWeight);

        if (grabbedInteractable && !wasGrabbed)
        {
            //animator.Play(grabAnimation.name);
            interactable.OnGrabbed();
            firstPersonLook.FocusOn(interactable.grabPoint);
            if(firstPersonMovement.speedOverrides.Contains(SpeedOverride))
            {
                firstPersonMovement.speedOverrides.Add(SpeedOverride);
            }
        }
        else if (!grabbedInteractable && wasGrabbed)
        {
            interactable.OnReleased();
            firstPersonLook.StopFocus();
            if(firstPersonMovement.speedOverrides.Contains(SpeedOverride))
            {
                firstPersonMovement.speedOverrides.Remove(SpeedOverride);
            }
        }
        
        if (grabbedInteractable)
        {
            float mouseDeltaY = Input.GetAxis("Mouse Y");
            interactable.Interact(-mouseDeltaY);
        }
    }

    bool IsTargetReachable()
    {
        var data = ikConstraint.data;

        float upper = Vector3.Distance(data.root.position, data.mid.position);
        float lower = Vector3.Distance(data.mid.position, data.tip.position);

        float maxReach = upper + lower;
        float targetDistance = Vector3.Distance(data.root.position, data.target.position);

        return targetDistance <= maxReach;
    }
}