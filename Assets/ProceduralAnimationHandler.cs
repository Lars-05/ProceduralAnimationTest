using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.Events;

public class ProceduralAnimationHandler : MonoBehaviour
{
    [SerializeField] private TwoBoneIKConstraint ikConstraint;

    [SerializeField] private float grabSpeed = 2f; // higher = faster grab

    public UnityEvent onGrabInteractable; 
    public UnityEvent onReleaseInteractable; 
    private bool grabbedInteractable = false;
    private float currentWeight;
    public float totalDrag; 

    void Update()
    {
        
        
        Debug.Log(grabbedInteractable);
            
        bool mouseHeld = Input.GetMouseButton(0);
        

        if (mouseHeld)
        {
            totalDrag += Mathf.Abs(Input.mousePositionDelta.y);
            Debug.Log(totalDrag );
        }
        else
        {
            totalDrag = 0;
        }
        bool reachable = IsTargetReachable();

        float targetWeight = (mouseHeld && reachable) ? 1f : 0f;
        
        currentWeight = Mathf.MoveTowards(
            currentWeight,
            targetWeight,
            grabSpeed * Time.deltaTime
        );

        ikConstraint.weight = currentWeight;
        grabbedInteractable = ikConstraint.weight == 1;
        if (grabbedInteractable)
        {
            onGrabInteractable.Invoke();
        }
        else
        {
            onReleaseInteractable.Invoke();
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