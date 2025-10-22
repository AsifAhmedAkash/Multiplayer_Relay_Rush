using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector3 initialDirection; // Default: move backward
    private Vector3 moveDirection;
    private Rigidbody rb;
    private Vector3 direction;
    private float speed;

    
    void FixedUpdate()
    {
        rb.MovePosition(rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime);
    }

    public void SetInitialDirection(Vector3 dir, float moveSpeed)
    {
        initialDirection = dir;
        speed = moveSpeed;
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false; // optional
        moveDirection = initialDirection.normalized;
    }

    void OnCollisionEnter(Collision collision)
    {
        // Get contact point
        ContactPoint contact = collision.contacts[0];

        // The normal points *away* from the surface
        Vector3 surfaceNormal = contact.normal;

        // Move direction should point *away* from the surface toward the ball
        moveDirection = (transform.position - contact.point).normalized;

        // Optional: align the new direction to reflect off the surface
        // moveDirection = Vector3.Reflect(moveDirection, surfaceNormal);

        Debug.DrawRay(contact.point, surfaceNormal, Color.red, 1f);      // Surface normal
        Debug.DrawRay(transform.position, moveDirection * 2f, Color.green, 1f); // New direction
    }
}
