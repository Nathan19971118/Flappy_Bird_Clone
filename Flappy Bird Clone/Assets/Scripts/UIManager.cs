using System.Collections;
using System.Collections.Generic;
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
    public Text scoreText;             // Displays current score
    public Image gameOverImage;        // Shows game over status
    public GameObject titleScreen;     // Reference to the title screen
    public GameObject gameOverScreen;  // Reference to the game over screen
    public Button playButton;          // Add reference to play button

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
        if (scoreText == null || gameOverImage == null || titleScreen == null || playButton == null)
        {
            Debug.LogError("UI components not properly assigned to UIManager!");
            return;
        }

        // Setup initial state
        scoreText.text = "0";
        gameOverImage.gameObject.SetActive(false);
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
        gameOverImage.gameObject.SetActive(false);
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
        }
        else
        {
            Debug.LogError("GameManager not found in scene!");
        }
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
        gameOverImage.gameObject.SetActive(false);
        UpdateScore(0);

        // Reset score display
        UpdateScore(0);
    }

    public void OnGameOver()
    {
        gameOverImage.gameObject.SetActive(true);
        gameOverScreen.SetActive(true); // Show game over screen on game over
    }
}
