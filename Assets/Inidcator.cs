using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Indicator : MonoBehaviour
{
    [Header("Player Settings")]
    public PlayerEnum player; // assign in Inspector

    [Header("Animation Settings")]
    [SerializeField] private string redLightAnimName = "RedLight"; // animation clip name

    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Subscribe to the event
        if (GameManager.Instance != null)
            GameManager.Instance.OnRedLight += HandleRedLight;
    }

    private void OnDisable()
    {
        // Unsubscribe to prevent memory leaks
        if (GameManager.Instance != null)
            GameManager.Instance.OnRedLight -= HandleRedLight;
    }

    public void HandleRedLight(PlayerEnum triggeredPlayer)
    {

        Debug.Log("red light from indicator" + player.ToString());
        // Only play animation if it's this indicator's player
        if (triggeredPlayer == player)
        {
            animator.Play(redLightAnimName);
        }
    }
}
