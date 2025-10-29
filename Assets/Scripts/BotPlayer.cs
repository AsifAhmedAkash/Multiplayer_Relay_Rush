using System.Collections;
using UnityEngine;

public class BotPlayer : MonoBehaviour
{
    public enum MovementAxis { X, Z }

    [Header("Movement Settings")]
    public MovementAxis moveAxis = MovementAxis.X;
    public float moveSpeed = 5f;
    public float idleMoveSpeed = 1f;
    public float idleChangeInterval = 2f; // time before choosing a new random idle target

    [Header("Bot Range Settings")]
    public float moveRange = 4f; // maximum distance from start position along moveAxis

    [Header("Bot Intelligence Settings")]
    [Range(0f, 1f)] public float accuracy = 0.7f; // 1 = perfect, 0 = always miss
    public float missOffset = 1.2f; // how far it misses when failing
    public float reactionDelay = 0.2f;

    private Vector3 startPos;
    private float targetAxisValue;
    private bool movingToBall = false;
    private float idleTimer;
    private float idleTargetAxisValue;

    public PlayerEnum player; // assign in Inspector

    private void Start()
    {
        startPos = transform.position;
        targetAxisValue = GetCurrentAxisValue();
        idleTargetAxisValue = targetAxisValue;
    }

    private void Update()
    {
        Vector3 pos = transform.position;

        if (movingToBall)
        {
            // Move smoothly toward target position on one axis (to hit ball)
            float newAxis = Mathf.MoveTowards(GetCurrentAxisValue(), targetAxisValue, moveSpeed * Time.deltaTime);
            pos = ApplyAxisValue(pos, newAxis);
        }
        else
        {
            // Random idle oscillation
            idleTimer += Time.deltaTime;
            if (idleTimer >= idleChangeInterval)
            {
                idleTimer = 0f;
                // Pick a new random target position within range
                float min = startPosAxisValue() - moveRange;
                float max = startPosAxisValue() + moveRange;
                idleTargetAxisValue = Random.Range(min, max);
            }

            float newAxis = Mathf.MoveTowards(GetCurrentAxisValue(), idleTargetAxisValue, idleMoveSpeed * Time.deltaTime);
            pos = ApplyAxisValue(pos, newAxis);
        }

        transform.position = pos;
    }

    private float GetCurrentAxisValue()
    {
        return moveAxis == MovementAxis.X ? transform.position.x : transform.position.z;
    }

    private float startPosAxisValue()
    {
        return moveAxis == MovementAxis.X ? startPos.x : startPos.z;
    }

    private Vector3 ApplyAxisValue(Vector3 pos, float value)
    {
        if (moveAxis == MovementAxis.X)
        {
            float clamped = Mathf.Clamp(value, startPos.x - moveRange, startPos.x + moveRange);
            pos.x = clamped;
        }
        else
        {
            float clamped = Mathf.Clamp(value, startPos.z - moveRange, startPos.z + moveRange);
            pos.z = clamped;
        }
        return pos;
    }

    /// <summary>
    /// Called by the ball trigger zone to tell the bot where the ball entered.
    /// </summary>
    public void OnBallEnteredZone(Vector3 ballPosition)
    {
        StopAllCoroutines();
        StartCoroutine(RespondToBall(ballPosition));
    }

    private IEnumerator RespondToBall(Vector3 ballPos)
    {
        yield return new WaitForSeconds(reactionDelay);

        bool willHit = Random.value <= accuracy;

        moveSpeed = Mathf.Lerp(1f, 12f, accuracy); // adjust speed based on accuracy

        float desiredAxis = moveAxis == MovementAxis.X ? ballPos.x : ballPos.z;

        if (!willHit)
        {
            // Miss slightly by adding an offset
            desiredAxis += Random.Range(-missOffset, missOffset);
        }

        // Clamp to movement range before applying
        float minRange = startPosAxisValue() - moveRange;
        float maxRange = startPosAxisValue() + moveRange;
        desiredAxis = Mathf.Clamp(desiredAxis, minRange, maxRange);

        targetAxisValue = desiredAxis;
        movingToBall = true;

        // Wait until it reaches that area (simulate reacting to the hit)
        yield return new WaitForSeconds(1.5f);

        // Stop actively moving, but STAY at that new spot (don’t reset to middle)
        movingToBall = false;
        idleTargetAxisValue = targetAxisValue;
        idleTimer = 0f;
    }
}
