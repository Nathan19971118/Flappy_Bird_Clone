using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    static UIManager _instance;
    public static UIManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<UIManager>();
            }
            return _instance;
        }
    }

    [Header("UI Elements")]
    public Text scoreText;               // Displays current score
    public Text currentScoreText;        // Displays current score when the game is over
    public Text highScoreText;           // Displays high score
    public GameObject titleScreen;       // Reference to the title screen
    public GameObject gameOverScreen;    // Reference to the game over screen
    public GameObject inGameScreen;      // Reference to the in game screen
    public Button playButton;            // Add reference to play button

    public Image medalImage;  // Reference to the Image for the medal
    public Sprite bronzeMedal;            // Bronze metal sprite
    public Sprite silverMedal;            // Silver metal sprite
    public Sprite goldMedal;              // Gold metal sprite
    public Sprite platinumMedal;          // Platinum metal sprite

    [Header("Reference")]
    public SpawnManager spawnManager;
    public PlayerController playerController;

    bool isInitialized = false;

    void Awake()
    {
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeUI();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void InitializeUI()
    {
        // Verify components
        if (scoreText == null || gameOverScreen == null || titleScreen == null || inGameScreen == null || playButton == null)
        {
            Debug.LogError("UI components not properly assigned to UIManager!");
            return;
        }

        // Setup initial state
        scoreText.text = "0";
        gameOverScreen.gameObject.SetActive(false);
        titleScreen.SetActive(true);

        // Setup button
        playButton.onClick.RemoveAllListeners();
        playButton.onClick.AddListener(OnPlayButtonClicked);

        isInitialized = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Initialize the UI at the beginning of the game
        UpdateScore(0);
        gameOverScreen.gameObject.SetActive(false);
        titleScreen.SetActive(true); // Ensure title screen is visible at start
    }

    // Method to be called when Play button is clicked
    public void OnPlayButtonClicked()
    {
        if (!isInitialized)
        {
            Debug.LogError("UIManager not properly initialized!");
            return;
        }

        var gameManager = FindObjectOfType<GameManager>();
        if (gameManager != null)
        {
            gameManager.StartGame();
            titleScreen.SetActive(false);
            gameOverScreen.SetActive(false);
        }
        else
        {
            Debug.LogError("GameManager not found in scene!");
        }
    }

    public void OnReplayButtonClicked()
    {
        // Hide the Game Over UI
        gameOverScreen.SetActive(false);

        if (playerController != null)
        {
            playerController.ResetPosition();
            playerController.StartPlaying();
        }

        // Reset game state
        GameManager.Instance.RestartGame();
    }

    // Update is called once per frame
    public void UpdateScore(int score)
    {
        if (scoreText != null)
        {
            scoreText.text = score.ToString();
        }
    }

    public void OnGameStart()
    {
        gameOverScreen.gameObject.SetActive(false);
        inGameScreen.gameObject.SetActive(true);

        UpdateScore(0);

        // Reset score display
        UpdateScore(0);
    }

    public void OnGameOver()
    {
        gameOverScreen.gameObject.SetActive(true);
        inGameScreen.gameObject.SetActive(false);
        gameOverScreen.SetActive(true); // Show game over screen on game over
    }

    // Method to update scores on the game over screen
    public void DisplayGameOverUI(int currentScore, int highScore)
    {
        currentScoreText.text = currentScore.ToString();
        highScoreText.text = highScore.ToString();

        // Enable the medal GameObject
        medalImage.gameObject.SetActive(true);

        // Determine which medal to display
        if (currentScore >= 40)
        {
            medalImage.sprite = platinumMedal;
        }
        else if (currentScore >= 30) // Set your thresholds as desired
        {
            medalImage.sprite = goldMedal;
        }
        else if (currentScore >= 20)
        {
            medalImage.sprite = silverMedal;
        }
        else if (currentScore >= 10)
        {
            medalImage.sprite = bronzeMedal;
        }
        else
        {
            medalImage.gameObject.SetActive(false); // No medal for lower scores
        }
    }

    public void HideGameOverUI()
    {
        medalImage.gameObject.SetActive(false);
    }
}
