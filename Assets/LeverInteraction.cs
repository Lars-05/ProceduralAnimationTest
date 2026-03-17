using System;
using System.Collections;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class LeverInteraction : MonoBehaviour
{
    [SerializeField] private AnimationClip leverAnimation;
    [SerializeField] private AnimationClip interactionAnimation;
    [SerializeField] private GameObject grabPoint;
    [SerializeField] private Animator leverAnimator;
    [SerializeField] private GameObject lever;
    [SerializeField] private RigBuilder rigBuilder;
    [SerializeField] private float targetRotation;
    public InteractionAnimationHandler interactionAnimationHandler;
    public ProceduralAnimationHandler ProceduralAnimationHandler;
    public float sensitivity;
    private Vector3 currentLeverRotation;
    private bool isBusy;

    private void Awake()
    {
        currentLeverRotation = lever.transform.localEulerAngles;
       
        Debug.Log("THIS HAPPENED");
        
    }

    private void Update()
    {
        lever.transform.localEulerAngles = new Vector3(lever.transform.localEulerAngles.x, lever.transform.localEulerAngles.y, Mathf.Lerp(currentLeverRotation.z, targetRotation, ProceduralAnimationHandler.totalDrag * sensitivity));
    }
    

    private void OnDisable()
    {
        if (isBusy)
            return;
        
        isBusy = true;
        interactionAnimationHandler.leftHandTarget.SetActive(true);
        interactionAnimationHandler.ikConstraint.data.target = grabPoint.transform;
        rigBuilder.Build();
        StartCoroutine(PreformInteraction());
    }

    IEnumerator PreformInteraction()
    {
        interactionAnimationHandler.animator.Play(interactionAnimation.name);
        yield return new WaitForSeconds(interactionAnimation.length);
        leverAnimator.Play(leverAnimation.name);
        yield return new WaitForSeconds(leverAnimation.length);
    }
    

    void OnTriggerExit(Collider other)
    {
        interactionAnimationHandler = null;
    }
}
