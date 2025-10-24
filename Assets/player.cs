using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Player Settings")]
    public PlayerEnum playerType;
    public float moveSpeed = 5f;
    public float moveRange = 5f;

    [Header("Animation")]
    public Animator animator;
    public string hitAnimation = "Hit";

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        float input = 0f;

        // You can map player-specific controls later if you want
        // For now: A/D or Left/Right arrows
        if (playerType == PlayerEnum.Player1)
            input = Input.GetAxis("Horizontal");

        if (input != 0)
        {
            Vector3 newPos = transform.position + Vector3.right * input * moveSpeed * Time.deltaTime;
            newPos.x = Mathf.Clamp(newPos.x, startPosition.x - moveRange, startPosition.x + moveRange);
            transform.position = newPos;
        }
    }

}
