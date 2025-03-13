using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem; // Add this namespace

public class ColorMemoryGame : MonoBehaviour
{
    public Button[] colorButtons; // Assign the color buttons in the inspector
    public TextMeshProUGUI instructionText; // For TextMeshPro
    public TextMeshProUGUI scoreText; // For TextMeshPro

    private List<int> colorSequence = new List<int>();
    private int playerSequenceIndex = 0;
    private int score = 0;
    private bool isGameActive = false;
    private bool isGameStarted = false; // Track if the game has started

    private PlayerInput playerInput; // For the new Input System

    void Start()
    {
        // Initialize buttons
        for (int i = 0; i < colorButtons.Length; i++)
        {
            int index = i;
            colorButtons[i].onClick.AddListener(() => OnColorButtonClicked(index));
        }

        // Initialize the new Input System
        playerInput = new PlayerInput();
        playerInput.Enable();
        playerInput.Player.Tap.performed += OnTap; // Listen for tap input

        StartGame();
    }

    void OnDestroy()
    {
        // Clean up the input system
        playerInput.Player.Tap.performed -= OnTap;
        playerInput.Disable();
    }

    void OnTap(InputAction.CallbackContext context)
    {
        // Start the game if it hasn't started yet
        if (!isGameStarted)
        {
            isGameStarted = true;
            StartCoroutine(StartNewRound());
        }
    }

    void StartGame()
    {
        instructionText.text = "Press anywhere to start";
        scoreText.text = "Score: 0";
        isGameActive = false;
        isGameStarted = false;
    }

    IEnumerator StartNewRound()
    {
        isGameActive = true;
        instructionText.text = "Memorize the sequence!";
        playerSequenceIndex = 0;
        colorSequence.Add(Random.Range(0, colorButtons.Length));

        foreach (int colorIndex in colorSequence)
        {
            yield return new WaitForSeconds(0.5f);
            HighlightButton(colorIndex);
            yield return new WaitForSeconds(0.5f);
        }

        instructionText.text = "Repeat the colors!";
    }

    void HighlightButton(int index)
    {
        colorButtons[index].image.color = Color.white; // Highlight color
        StartCoroutine(ResetButtonColor(index));
    }

    IEnumerator ResetButtonColor(int index)
    {
        yield return new WaitForSeconds(0.5f);
        colorButtons[index].image.color = GetColorByIndex(index);
    }

    Color GetColorByIndex(int index)
    {
        switch (index)
        {
            case 0: return Color.red;
            case 1: return Color.blue;
            case 2: return Color.green;
            case 3: return Color.yellow;
            default: return Color.black;
        }
    }

    void OnColorButtonClicked(int index)
    {
        if (!isGameActive) return;

        if (index == colorSequence[playerSequenceIndex])
        {
            playerSequenceIndex++;
            if (playerSequenceIndex == colorSequence.Count)
            {
                score++;
                scoreText.text = "Score: " + score;
                StartCoroutine(StartNewRound());
            }
        }
        else
        {
            GameOver();
        }
    }

    void GameOver()
    {
        isGameActive = false;
        isGameStarted = false;
        instructionText.text = "Game Over! Final Score: " + score + "\nPress anywhere to restart.";
        colorSequence.Clear();
        score = 0;
    }
}