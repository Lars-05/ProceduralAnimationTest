using UnityEngine;
public interface IInteractable
{
    public Transform grabPoint { get; }
    bool isGrabbed {get;set;}
    public void OnGrabbed();
    public void Interact(float value);
    public void OnReleased();
}