using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    void Start()
    {
        // Initialize buttons
        for (int i = 0; i < colorButtons.Length; i++)
        {
            int index = i;
            colorButtons[i].onClick.AddListener(() => OnColorButtonClicked(index));
        }

        StartGame();
    }

    void FixedUpdate()
    {
        // Check for mouse click or touch input
        if (!isGameStarted && (Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)))
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

        // Disable buttons at the start
        SetButtonsInteractable(false);
    }

    IEnumerator StartNewRound()
    {
        SetButtonsInteractable(true);

        isGameActive = true;
        instructionText.text = "Memorize the sequence!";
        playerSequenceIndex = 0;
        colorSequence.Add(Random.Range(0, colorButtons.Length));

        // Use a for loop instead of foreach to avoid modifying the collection while iterating
        for (int i = 0; i < colorSequence.Count; i++)
        {
            int colorIndex = colorSequence[i];
            yield return new WaitForSeconds(0.5f);
            yield return FlashButton(colorIndex); // Flash the button
        }

        instructionText.text = "Repeat the colors";
    }

    IEnumerator FlashButton(int index)
    {
        // Change the button color to its assigned color
        colorButtons[index].image.color = GetColorByIndex(index);
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds

        // Change the button color back to white
        colorButtons[index].image.color = Color.white;
    }

    void SetButtonsInteractable(bool interactable)
    {
        // Enable or disable all buttons
        foreach (Button button in colorButtons)
        {
            button.interactable = interactable;
        }
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
        instructionText.text = "Game Over! Final Score: " + score + "\nPress anywhere to restart";
        colorSequence.Clear();
        score = 0;

        // Disable buttons when the game is over
        SetButtonsInteractable(false);
    }
}