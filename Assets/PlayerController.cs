using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
   [SerializeField] private Rigidbody rb;
   
   public float Right;
   public float Left;
   private void Update()
   {
      if (Input.GetKey(KeyCode.W))
         rb.linearVelocity = Vector3.forward * 10;
      if (Input.GetKey(KeyCode.Q))
         rb.linearVelocity = Vector3.left * 7;
      if (Input.GetKey(KeyCode.E))
         rb.linearVelocity = Vector3.right * 7;
      if (Input.GetKey(KeyCode.S))
         rb.linearVelocity = Vector3.back * 9;
      if (Input.GetKey(KeyCode.D))
         transform.Rotate(transform.up ,Input.GetAxis("Horizontal") * Right  );
      if (Input.GetKey(KeyCode.A))
         transform.Rotate(transform.up ,-Input.GetAxis("Horizontal") * Left  );
   }
}
