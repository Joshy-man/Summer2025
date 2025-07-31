using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameController : MonoBehaviour
{
    public GameObject gameOverScreen;
    public TMP_Text survivedText;
    private int survivedLevelsCount;

    public static event Action OnReset;

    void Start()
    {
        PlayerHealth.OnPlayerDied += GameOverScreen;
        gameOverScreen.SetActive(false);
    }

    void GameOverScreen()
    {
        gameOverScreen.SetActive(true);
        survivedText.text = "LEVEL " + (survivedLevelsCount + 1) + " COMPLETED! " + survivedLevelsCount + " LEVELS";
        
        if (survivedLevelsCount != 1) 
            survivedText.text = "LEVEL " + (survivedLevelsCount + 1) + " COMPLETED! " + survivedLevelsCount + " LEVELS";
        Time.timeScale = 0;
       
    }

    public void ResetGame()
    {
        gameOverScreen.SetActive(false);
        survivedLevelsCount = 0;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        OnReset.Invoke();
        Time.timeScale = 1;
    }

    void LoadLevel(int levelIndex, bool wantSurvivedIncrease)
    {
        if (wantSurvivedIncrease) survivedLevelsCount++;
        
        // Load the actual scene by index
        SceneManager.LoadScene(levelIndex);
    }

    private void OnDestroy()
    {
        PlayerHealth.OnPlayerDied -= GameOverScreen;
    }
}