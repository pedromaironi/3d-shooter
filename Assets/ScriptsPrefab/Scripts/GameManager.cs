using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    private int totalEnemies = 3;     // Total number of enemies
    private int remainingEnemies;

    private bool gameWon = false;     // Flag to check if the game is won

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        remainingEnemies = totalEnemies;
    }

    public void EnemyDestroyed()
    {
        remainingEnemies--;

        if (remainingEnemies <= 0)
        {
            gameWon = true;
        }
    }

    void OnGUI()
    {
        // Display the remaining enemies count
        GUI.Label(new Rect(Screen.width - 110, Screen.height - 30, 100, 20), "Enemies: " + remainingEnemies);

        // Display the "Ganaste" message when the game is won
        if (gameWon)
        {
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Ganaste");
            SceneManager.LoadScene("Game over");
        }
    }
}