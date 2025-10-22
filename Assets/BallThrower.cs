using System.Collections;
using UnityEngine;

public class BallThrower : MonoBehaviour
{
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
        if (animator != null)
            animator.Play(preThrowAnimationName);

        yield return new WaitForSeconds(preThrowAnimDuration);

        GameObject ball = Instantiate(ballPrefab, throwPoint.position, Quaternion.identity);

        // Each thrower gets its unique direction!
        Vector3 direction = (target.position - throwPoint.position).normalized;

        // Use Ball script
        Ball ballScript = ball.GetComponent<Ball>();
        if (ballScript != null)
        {
            ballScript.SetInitialDirection(direction, ballMoveSpeed);
        }

        onBallSpawned?.Invoke(ball);
    }
}
