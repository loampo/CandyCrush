using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public int scoreForMatch = 10;
    private void Awake()
    {
        //Controllo in più
        if (instance == null)
            instance = this;
    }
    void Start()
    {
        UIManager.instance.scoreText.text = ScoreHolder.instance.score.ToString();
        UIManager.instance.bestScoreText.text = ScoreHolder.instance.bestScore.ToString();
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("Game");
    }
    public void LoadMainMenuScene()
    {
        SceneManager.LoadScene("Menù");
    }

    public void Pause()
    {
        Time.timeScale = 0;
        UIManager.instance.pauseMenu.SetActive(true);
    }
    public void ResumeGame()
    {
        if (Time.timeScale == 0)
        {
            UIManager.instance.pauseMenu.SetActive(false);
            Time.timeScale = 1;
        }
    }
    public void Quit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
