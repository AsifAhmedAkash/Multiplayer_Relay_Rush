using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallSpawnManager : MonoBehaviour
{
    [Header("Timing Settings")]
    [SerializeField] private float minThrowInterval = 20f;
    [SerializeField] private float maxThrowInterval = 25f;

    private List<BallThrower> throwers = new List<BallThrower>();
    private List<GameObject> activeBalls = new List<GameObject>();

    private void Start()
    {
        // Find all BallThrowers in the scene
        throwers.AddRange(FindObjectsOfType<BallThrower>());

        if (throwers.Count == 0)
        {
            Debug.LogWarning("No BallThrower objects found in the scene!");
            return;
        }

        // Start the throw loop
        StartCoroutine(ThrowManagerLoop());
    }

    private IEnumerator ThrowManagerLoop()
    {
        while (true)
        {
            // Pick random interval
            float waitTime = Random.Range(minThrowInterval, maxThrowInterval);

            // Wait until 2 seconds before throw
            float preThrowDuration = 0f;

            // Pick a random thrower
            BallThrower chosenThrower = throwers[Random.Range(0, throwers.Count)];
            if (chosenThrower != null)
                preThrowDuration = chosenThrower.PreThrowAnimDuration;

            yield return new WaitForSeconds(waitTime - preThrowDuration);

            // Let chosen thrower handle its animation + throw
            yield return StartCoroutine(chosenThrower.ThrowWithAnimation(OnBallSpawned));
        }
    }

    private void OnBallSpawned(GameObject ball)
    {
        if (ball != null)
        {
            activeBalls.Add(ball);
            // Remove automatically when destroyed
            BallLifeTracker tracker = ball.AddComponent<BallLifeTracker>();
            tracker.Init(this);
        }
    }

    public void RemoveBall(GameObject ball)
    {
        if (activeBalls.Contains(ball))
        {
            activeBalls.Remove(ball);
        }
    }

    public List<GameObject> GetActiveBalls()
    {
        return activeBalls;
    }
}
