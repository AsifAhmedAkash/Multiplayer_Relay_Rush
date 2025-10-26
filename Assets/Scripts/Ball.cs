using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private Vector3 moveDirection;
    private Rigidbody rb;

    [Header("Lifetime Settings")]
    public float lifeTime = 15f;
    private BallSpawnManager spawnManager;

    public PlayerEnum LastTouchedBy = PlayerEnum.None;
    private float yStartPos;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        if (rb != null)
            rb.useGravity = false;

        yStartPos = transform.position.y;
        // Start lifetime countdown
        StartCoroutine(LifeTimer());
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            // Keep current Y position fixed
            Vector3 newPos = rb.position + moveDirection * moveSpeed * Time.fixedDeltaTime;
            newPos.y = yStartPos; // Don't change Y

            rb.MovePosition(newPos);
        }
    }


    public void SetInitialDirection(Vector3 dir, float moveSpeed)
    {
        this.moveSpeed = moveSpeed;
        moveDirection = dir.normalized;
    }

    public void SetSpawnManager(BallSpawnManager manager)
    {
        spawnManager = manager;
    }

    private IEnumerator LifeTimer()
    {
        yield return new WaitForSeconds(lifeTime - 2f);
        // Optional: play pop animation here
        yield return new WaitForSeconds(2f);
        Destroy(gameObject);
    }

    void OnCollisionEnter(Collision collision)
    {
        
        ContactPoint contact = collision.contacts[0];
        moveDirection = (transform.position - contact.point).normalized;
        moveSpeed += 3f; // Increase speed on bounce
        if(moveSpeed > 12f)
            moveSpeed = 12f; // Cap max speed
        Debug.DrawRay(contact.point, contact.normal, Color.red, 1f);
        Debug.DrawRay(transform.position, moveDirection * 2f, Color.green, 1f);
    }

    
    private void OnDestroy()
    {
        if (spawnManager != null)
            spawnManager.RemoveBall(gameObject);
    }
}
