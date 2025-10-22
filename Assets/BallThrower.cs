using System.Collections;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
    [Header("Ball Settings")]
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private Transform target;
    [SerializeField] private float ballMoveSpeed = 5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string preThrowAnimationName = "PreThrow";
    [SerializeField] private float preThrowAnimDuration = 2f;

    public float PreThrowAnimDuration => preThrowAnimDuration;

    public IEnumerator ThrowWithAnimation(System.Action<GameObject> onBallSpawned)
    {
        // Play pre-throw animation if available
        if (animator != null)
            animator.Play(preThrowAnimationName);

        // Wait for animation to finish
        yield return new WaitForSeconds(preThrowAnimDuration);

        // Spawn and throw the ball
        GameObject ball = Instantiate(ballPrefab, throwPoint.position, Quaternion.identity);

        Vector3 direction = (target.position - throwPoint.position).normalized;
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.velocity = direction * ballMoveSpeed;
        }

        onBallSpawned?.Invoke(ball);
    }
}
