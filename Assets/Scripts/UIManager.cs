using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public int score;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI timeCountdown;
    public GameObject timeUp;
    public GameObject pauseMenu;
    public static UIManager instance;

    private void Awake()
    {
        //Controllo in più
        if (instance == null)
            instance = this;
    }
    public void ShowScore()
    {
        scoreText.text = score.ToString();
    }

    public void IncreaseScore(int amount)
    {
        score += amount;
        ShowScore();
    }
}
