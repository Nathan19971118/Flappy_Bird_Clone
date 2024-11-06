using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // Singleton pattern

    [Header("Game State")]
    public bool isGameStarted = false;
    public bool isGamePaused = true;

    [Header("Score")]
    public int currentScore = 0;
    public int highScore = 0;

    [Header("References")]
    public PlayerController playerController;
    public SpawnManager spawnManager;
    public RepeatBackground background;  // Hold background element

    [Header("Game Settings")]
    public float scoreIncreaseRate = 1f;  // Points per second
    private float scoreTimer = 0f;

    private bool isInitialized = false;

    void Awake()
    {
        // Singleton pattern implementation
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            InitializeGame();
        }
        else
        {
            Destroy(gameObject);
        }

        // Load high score from PlayerPrefs
        highScore = PlayerPrefs.GetInt("HighScore", 0);
    }

    void InitializeGame()
    {
        // Load high score
        highScore = PlayerPrefs.GetInt("HighScore", 0);

        // Find references if not set
        if (playerController == null)
            playerController = FindObjectOfType<PlayerController>();

        if (spawnManager == null)
            spawnManager = FindObjectOfType<SpawnManager>();

        if (background == null)
            background = FindObjectOfType<RepeatBackground>();

        isGameStarted = false;
        isGamePaused = false;
        currentScore = 0;
        isInitialized = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Find all required components if not set
        if (playerController == null)
        {
            playerController = FindObjectOfType<PlayerController>();
            if (playerController == null)
                Debug.LogError("PlayerController not found in scene!");
        }

        if (spawnManager == null)
        {
            spawnManager = FindObjectOfType<SpawnManager>();
            if (spawnManager == null)
                Debug.LogError("SpawnManager not found in scene!");
        }

        if (background == null)
        {
            background = FindObjectOfType<RepeatBackground>();
            if (background == null)
                Debug.LogError("RepeatBackground not found in scene!");
        }

        // Initialize game state
        isGameStarted = false;
        isGamePaused = false;
        currentScore = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // TogglePause();
        }

        // Update score while game is running
        if (isGameStarted && !isGamePaused && playerController.isAlive)
        {
            scoreTimer += Time.deltaTime;
            if (scoreTimer >= scoreIncreaseRate)
            {
                scoreTimer = 0f;
                IncreaseScore(1);
            }
        }
    }

    public void StartGame()
    {
        // Verify all required components are present
        if (!isInitialized)
        {
            Debug.LogError("Cannot start game: Missing required components!");
            return;
        }

        // Only start if not already started
        if (!isGameStarted)
        {
            isGameStarted = true;
            isGamePaused = false;
            currentScore = 0;

            if (playerController != null)
            {
                playerController.StartPlaying();
            }
            else
            {
                Debug.LogError("PlayerController reference missing!");
                return;
            }

            if (background != null)
            {
                background.isScrolling = true;
            }

            if (spawnManager != null)
            {
                spawnManager.SpawnObstacles();
            }
            else
            {
                Debug.LogError("SpawnManager reference missing!");
                return;
            }

            Time.timeScale = 1f;

            // Try to find UIManager if it exists
            var uiManager = FindObjectOfType<UIManager>();
            if (uiManager != null)
            {
                uiManager.OnGameStart();
            }
            else
            {
                Debug.LogError("UIManager not found in scene!");
            }
        }   
    }

    public void GameOver()
    {
        // Update high score if necessary
        if (currentScore > highScore)
        {
            highScore = currentScore;
            PlayerPrefs.SetInt("HighScore", highScore);
            PlayerPrefs.Save();
        }

        // Stop all game elements
        if (playerController != null)
        {
            playerController.isAlive = false;
            playerController.playerRb.simulated = false;
        }

        if (background != null)
        {
            background.isScrolling = false;
        }

        if (spawnManager != null)
        {
            spawnManager.StopSpawning();
        }

        isGameStarted = false;

        if (UIManager.Instance != null)
        {
            UIManager.Instance.OnGameOver();
        }
    }

    public void RestartGame()
    {
        // Reset player state, score, etc.
        currentScore = 0;
        isGameStarted = true;
        isGamePaused = false;

        if (playerController != null)
        {
            playerController.isAlive = true;
            playerController.playerAnim.SetBool("isFlap", true);
            playerController.ResetPosition(); 
        }

        if (background != null)
        {
            background.isScrolling = true;
        }

        // Clear pipes on screen and restart spawning
        spawnManager.DestroyObjects();

        UIManager.Instance.OnGameStart();
        spawnManager.SpawnObstacles();
    }

    public void IncreaseScore(int amount)
    {
        currentScore += amount;
        // Notify UI Manager
        UIManager.Instance.UpdateScore(currentScore);
    }

    // Method to be called when passing through pipes
    public void OnPipePass()
    {
        IncreaseScore(1);  // Award 1 point for passing through pipes
    }
}
