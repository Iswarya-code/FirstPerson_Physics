using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickAndPlace : MonoBehaviour
{
    public float pickupDistance = 20f;
    public Transform holdPosition;
    public LayerMask pickableLayer;  // Assign "Pickable" layer in Inspector
    public float throwForce = 20f;

    private GameObject heldObject;
    private Rigidbody heldObjectRb;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Left Click to Pick/Drop
        {
            if (heldObject == null)
                TryPickup();
            else
                PlaceObject();
        }

        if (Input.GetMouseButtonDown(1)) // Right Click to Throw
        {
            if (heldObject != null)
                ThrowObject();
        }
    }

    void TryPickup()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit, pickupDistance, pickableLayer)) // Now checks only "Pickable" objects
        {
            Rigidbody rb = hit.collider.GetComponent<Rigidbody>();
            if (rb != null)
            {
                heldObject = hit.collider.gameObject;
                heldObjectRb = rb;
                heldObjectRb.isKinematic = true;  // Disable physics while holding
                heldObject.transform.position = holdPosition.position;
                heldObject.transform.rotation = holdPosition.rotation;  // Maintain correct rotation
                heldObject.transform.SetParent(holdPosition);
            }
        }
    }

    void PlaceObject()
    {
        heldObject.transform.SetParent(null);
        heldObjectRb.isKinematic = false; // Re-enable physics

        // Raycast downwards to place object on the surface
        if (Physics.Raycast(heldObject.transform.position, Vector3.down, out RaycastHit hit, 2f))
        {
            heldObject.transform.position = hit.point;
        }

        heldObject = null;
        heldObjectRb = null;
    }

    void ThrowObject()
    {
        heldObject.transform.SetParent(null);
        heldObjectRb.isKinematic = false;
        heldObjectRb.AddForce(Camera.main.transform.forward * throwForce, ForceMode.Impulse);
        heldObject = null;
        heldObjectRb = null;
    }
}
