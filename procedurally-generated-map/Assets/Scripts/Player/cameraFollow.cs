using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraFollow : MonoBehaviour
{
    public Transform target;                 // The target (car) to follow
    public Vector3 offset = new Vector3(0, 5, -10);  // Position offset from the target
    public float followSpeed = 10f;          // Speed at which the camera follows the target
    public float rotationSpeed = 5f;         // Speed of rotation adjustment
    public float positionDamping = 0.3f;     // Extra damping for position movement

    private Vector3 velocity = Vector3.zero; // For SmoothDamp

    void FixedUpdate()
    {
        if (target == null) return;

        // Smoothly move the camera to the target position with SmoothDamp
        Vector3 desiredPosition = target.position + target.TransformDirection(offset);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, positionDamping);

        // Smoothly rotate the camera to face the same direction as the target
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSpeed * Time.deltaTime);
    }
}
