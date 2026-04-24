using UnityEngine;
public interface IInteractable
{
    Transform GrabPoint { get; }
    bool isGrabbed {get;set;}
    void OnGrabbed();
    
    void OnReleased();
}