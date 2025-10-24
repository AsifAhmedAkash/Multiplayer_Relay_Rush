using UnityEngine;

public class BotControllerZone : MonoBehaviour
{
    public BotPlayer bot;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ball"))
        {
            bot.OnBallEnteredZone(other.transform.position);
        }
    }
}
