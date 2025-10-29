using TMPro;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Animator))]
public class Indicator : MonoBehaviour
{
    [SerializeField] private int score = 0;

    [Header("Player Settings")]
    public PlayerEnum player; // assign in Inspector

    [Header("UI Settings")]
    [SerializeField] private TMP_Text scoreText; // assign a Text UI element in the Canvas
    [SerializeField] private TMP_Text playerGlobalScoreTxt;
    [Header("Animation Settings")]
    [SerializeField] private string redLightAnimName = "RedLight"; // animation clip name

    private Animator animator;
    [SerializeField] private Animator blockerAnim;
    [SerializeField] private GameObject BotPlayerToDeactivate;
    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        // Initialize UI
        UpdateScoreUI();

        // Subscribe to event
        if (GameManager.Instance != null)
            GameManager.Instance.OnRedLight += HandleRedLight;
    }

    public void SetScore(int MaxScore)
    {
        score = MaxScore;
        UpdateScoreUI();
    }

    private void OnDisable()
    {
        if (GameManager.Instance != null)
            GameManager.Instance.OnRedLight -= HandleRedLight;
    }

    public void HandleRedLight(PlayerEnum triggeredPlayer)
    {
        //Debug.Log("red light from indicator " + player.ToString());

        // Only update this player's indicator
        if (triggeredPlayer == player)
        {
            score--;
            if(score <= 0)
            {
                score = 0;
                blockerAnim.Play("block");
                GameManager.Instance.AnyPlayerDied(player);
                BotPlayerToDeactivate.SetActive(false);
            }
                
            UpdateScoreUI();
            if(animator != null)
                animator.Play(redLightAnimName);
        }
    }

    private void UpdateScoreUI()
    {
        if (scoreText != null)
        {
            // Format: 000, 001, 023, 105, etc.
            scoreText.text = score.ToString("D3");
            playerGlobalScoreTxt.text = score.ToString("D3");
        }
    }
}
