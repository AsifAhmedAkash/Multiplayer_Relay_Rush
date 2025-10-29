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

    [Header("Joystick (Optional)")]
    public FloatingJoystick floatingJoystick; // assign in Inspector

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

        // Keyboard input (PC)
        if (playerType == PlayerEnum.Player1)
            input = Input.GetAxis("Horizontal");

        // Joystick input (Mobile)
        if (floatingJoystick != null && Mathf.Abs(floatingJoystick.Horizontal) > 0.1f)
            input = floatingJoystick.Horizontal;

        // Move player
        if (Mathf.Abs(input) > 0.01f)
        {
            Vector3 newPos = transform.position + Vector3.right * input * moveSpeed * Time.deltaTime;
            newPos.x = Mathf.Clamp(newPos.x, startPosition.x - moveRange, startPosition.x + moveRange);
            transform.position = newPos;

            // Optional: trigger animation if moving
            if (animator != null)
                animator.SetBool("isMoving", true);
        }
        else
        {
            if (animator != null)
                animator.SetBool("isMoving", false);
        }
    }

}
