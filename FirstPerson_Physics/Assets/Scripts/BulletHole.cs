using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EzySlice; // Import Ezy-Slice library


public class BulletHole : MonoBehaviour
{
    public Material cutMaterial; // Assign this in the Inspector

    public void CutObject(Vector3 position, Vector3 normal)
    {
        Debug.Log("Attempting to cut: " + gameObject.name);

        GameObject[] slices = gameObject.SliceInstantiate(position, normal, cutMaterial);

        if (slices != null && slices.Length > 0)
        {
            Debug.Log("Cutting successful!");

            foreach (GameObject slice in slices)
            {
                MeshCollider meshCollider = slice.AddComponent<MeshCollider>();
                meshCollider.convex = true;

                Rigidbody rb = slice.AddComponent<Rigidbody>();
                rb.useGravity = true;
                rb.mass = 1f;
                rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            }

            Destroy(gameObject); // Destroy original object
        }
        else
        {
            Debug.Log("Cutting failed! Ensure object has a MeshFilter & MeshCollider.");
        }
    }
}
