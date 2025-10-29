using UnityEngine;

public class DeadZone : MonoBehaviour
{
    public PlayerEnum player; // assign in Inspector

    private void OnTriggerEnter(Collider other)
    {
        Ball ball = other.GetComponent<Ball>();
        if (ball != null)
        {
            SoundManager.Instance.PlayOneShot("dead");
            // Notify GameManager
            GameManager.Instance.AddScore(player);

            // Fire RedLight event
            GameManager.Instance.FireRedLight(player);

            // Destroy the ball
            Destroy(ball.gameObject);
        }
    }
}
