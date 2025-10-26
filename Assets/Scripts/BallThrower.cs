using System.Collections;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private Transform throwPoint;
    [SerializeField] private Transform[] targets; // Multiple targets
    [SerializeField] private float ballMoveSpeed = 5f;

    [Header("Animation")]
    [SerializeField] private Animator animator;
    [SerializeField] private string preThrowAnimationName = "PreThrow";
    [SerializeField] private float preThrowAnimDuration = 2f;

    public float PreThrowAnimDuration => preThrowAnimDuration;

    public IEnumerator ThrowWithAnimation(System.Action<GameObject> onBallSpawned)
    {
        // Play pre-throw animation
        if (animator != null)
            animator.Play(preThrowAnimationName);

        // Wait until animation finishes
        yield return new WaitForSeconds(preThrowAnimDuration);

        // Spawn the ball
        GameObject ball = Instantiate(ballPrefab, throwPoint.position, Quaternion.identity);

        // Pick a random target from the list
        if (targets != null && targets.Length > 0)
        {
            Transform randomTarget = targets[Random.Range(0, targets.Length)];
            Vector3 direction = (randomTarget.position - throwPoint.position).normalized;

            // Set direction & speed on the Ball script
            Ball ballScript = ball.GetComponent<Ball>();
            if (ballScript != null)
            {
                ballScript.SetInitialDirection(direction, ballMoveSpeed);
            }

            onBallSpawned?.Invoke(ball);
        }
        else
        {
            Debug.LogWarning($"{name} has no targets assigned!");
            Destroy(ball); // Clean up unused ball
        }
    }
}
