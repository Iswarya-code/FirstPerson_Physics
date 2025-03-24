using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Shooting : MonoBehaviour
{
    public Camera playerCamera; // Assign your FPS camera
    public float shootForce = 500f; // Adjust force applied to objects
    public float maxDistance = 100f; // Max shooting range

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left-click to shoot
        {
            Shoot();
        }
    }

    void Shoot()
    {
        
         RaycastHit hit;
         if (Physics.Raycast(playerCamera.transform.position, playerCamera.transform.forward, out hit, maxDistance))
         {
             Debug.Log("Raycast hit: " + hit.collider.gameObject.name);

             if (hit.collider.gameObject.CompareTag("Destroy"))
             {
                 Debug.Log("Hit Destructible object: " + hit.collider.gameObject.name);

                 Rigidbody rb = hit.collider.attachedRigidbody;
                 if (rb != null)
                 {
                     Debug.Log("Applying ultra slow force to: " + hit.collider.gameObject.name);

                     rb.drag = 3f;  // Increased drag to slow down movement
                     rb.angularDrag = 3f; // Reduce rotation speed
                     rb.useGravity = true; // Ensure it falls naturally
                     // rb.AddForce(hit.normal * -shootForce * 0.02f, ForceMode.VelocityChange); // Apply very small force
         // Apply force in the direction of the raycast (where the player is aiming)
                     Vector3 shootDirection = (hit.collider.transform.position - playerCamera.transform.position).normalized;
                     rb.AddForce(shootDirection * shootForce * 0.05f, ForceMode.VelocityChange);
                 }

                 Debug.Log("Destroying object: " + hit.collider.gameObject.name);
                // Destroy(hit.collider.gameObject, 5f); // Destroy after delay
             }
             else
             {
                 Debug.Log("Object hit does NOT have the 'Destructible' tag.");
             }
         }
         else
         {
             Debug.Log("Raycast did NOT hit anything.");
         }
    }

    IEnumerator ApplySlowForce(Rigidbody rb, Vector3 direction)
    {
        float duration = 5f;  // Apply force for 5 seconds (increase for slower movement)
        float elapsedTime = 0f;
        float forceAmount = 3f; // Lower force for slower movement

        while (elapsedTime < duration)
        {
            rb.AddForce(-direction * forceAmount, ForceMode.Acceleration);
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for next frame
        }
    }
}

