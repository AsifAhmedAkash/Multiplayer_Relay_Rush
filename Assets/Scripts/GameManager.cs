using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public GameObject Tutorial;
    public GameObject pauseGameCanv_notDied;
    public GameObject pauseGameCanv_Died;
    public GameObject winCanv;
    public TMP_Text winCanv_txt;
    private bool[] isPlayerAlive;


    [SerializeField] private TMP_Text maxPointtxt;
    [SerializeField] private TMP_Text botAccTxt;

    [Header("Sliders")]
    [SerializeField] private Slider botAccuracySlider; 
    [SerializeField] private Slider maxPointSlider;

    [SerializeField] private int numberOfPlayers = 4;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    [SerializeField] public BotPlayer[] Bots;
    [SerializeField] public Indicator[] indicators;

    private void Start()
    {
        Tutorial.SetActive(true);
        winCanv.SetActive(false);
        PauseGame();

        // Initialize all players as alive
        int totalPlayers = Enum.GetNames(typeof(PlayerEnum)).Length - 1; // skip None
        isPlayerAlive = new bool[totalPlayers + 1]; // +1 because PlayerEnum starts at 1
        for (int i = 1; i <= totalPlayers; i++)
            isPlayerAlive[i] = true;

        if (botAccuracySlider != null)
            botAccuracySlider.onValueChanged.AddListener(BotAccuractyChange);

        if (maxPointSlider != null)
            maxPointSlider.onValueChanged.AddListener(MaxPointChange);

        botAccTxt.text = "Bot Accuracy: " + 30 + "%";
        maxPointtxt.text = "Max Points: " + 3;
    }

    public void YouDied()
    {
        pauseGameCanv_Died.SetActive(true);
    }

    public void AnyPlayerDied(PlayerEnum playerdied)
    {
        if (playerdied == PlayerEnum.None) return;

        if(playerdied == PlayerEnum.Player1)
        {
            YouDied();
        }

        isPlayerAlive[(int)playerdied] = false; // mark dead
        numberOfPlayers--;

        if (numberOfPlayers == 1)
        {
            // Find the last player alive
            PlayerEnum lastPlayer = PlayerEnum.None;
            for (int i = 1; i < isPlayerAlive.Length; i++)
            {
                if (isPlayerAlive[i])
                {
                    lastPlayer = (PlayerEnum)i;
                    break;
                }
            }

            if (lastPlayer == PlayerEnum.Player1)
            {
                SoundManager.Instance.PlayOneShot("win");
                pauseGameCanv_Died.SetActive(true);
            }
            winCanv.SetActive(true);
            winCanv_txt.text = "Winner: " + lastPlayer.ToString();
            //Debug.Log("Last player standing: " + lastPlayer);
            StartCoroutine(PauseAfterDelay(2f));
        }
    }

    private IEnumerator PauseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        PauseGame();
    }

    public void BotAccuractyChange(float value)
    {
        // Map slider range [0,1]  accuracy range [0.3,1.0]
        float mappedAccuracy = Mathf.Lerp(0.3f, 1f, value);

        foreach (var bot in Bots)
        {
            bot.accuracy = mappedAccuracy;
        }

        botAccTxt.text = "Bot Accuracy: " + (int)(mappedAccuracy * 100) + "%";
    }


    [SerializeField] private int MaxPoints = 20;
    [SerializeField] private int minPoints = 3;

    public void MaxPointChange(float value)
    {
        int intValue = (int)((20-3)* value) + minPoints;
        maxPointtxt.text = "Max Points: " + intValue.ToString();
        
        foreach(var indicator in indicators)
        {
            indicator.SetScore(intValue);
        }
    }


    public void RestartGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }
    public void UnPauseGame()
    {
        Time.timeScale = 1f;
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

