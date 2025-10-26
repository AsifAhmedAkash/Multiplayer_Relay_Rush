using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Store scores for each player
    [SerializeField] private readonly int[] playerScores = new int[Enum.GetNames(typeof(PlayerEnum)).Length];

    // Event for Red Light
    public event Action<PlayerEnum> OnRedLight;

    public void AddScore(PlayerEnum player)
    {
        playerScores[(int)player]++;
        //Debug.Log($"{player} scored! Total: {playerScores[(int)player]}");
    }

    public void FireRedLight(PlayerEnum player)
    {
        OnRedLight?.Invoke(player);
        //Debug.Log($"Red Light fired for {player}!");
    }

    public int GetScore(PlayerEnum player)
    {
        return playerScores[(int)player];
    }
}


public enum PlayerEnum
{
    None,
    Player1,
    Player2,
    Player3,
    Player4
}

