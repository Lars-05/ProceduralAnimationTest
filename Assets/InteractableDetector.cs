using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class InteractableDetector : MonoBehaviour
{
    public float radius = 2f;

    private IInteractable closestInteractable;
    private IInteractable currentInteractable;
    private IInteractable grabbedInteractable;
    [SerializeField] private GameObject origin;
    
    void Update()
    {
        currentInteractable = GetClosestInteractable();
    }

    public IInteractable GetClosestInteractable()
    {
        Collider[] hits = Physics.OverlapSphere(origin.transform.position, radius);

        float closestDistance = Mathf.Infinity;
        IInteractable bestMatch = null;

        foreach (Collider hit in hits)
        {
            IInteractable interactable = hit.GetComponent<IInteractable>();

            if (interactable == null)
                continue;

            float distance = Vector3.Distance(origin.transform.position, hit.transform.position);

            if (distance < closestDistance)
            {
                closestDistance = distance;
                bestMatch = interactable;
            }
        }

        closestInteractable = bestMatch;

        return bestMatch;
    }

    void OnDrawGizmosSelected()
    {
        if (origin == null) return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(origin.transform.position, radius);

        if (closestInteractable != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(
                origin.transform.position,
                ((MonoBehaviour)closestInteractable).transform.position
            );
        }
    }
}