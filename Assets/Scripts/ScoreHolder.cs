using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreHolder : MonoBehaviour
{
    public static ScoreHolder instance;
    [HideInInspector]
    public int score;
    [HideInInspector]
    public int bestScore;

    private UIManager ui;

    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);
        DontDestroyOnLoad(gameObject);
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name == "Game")
        {
            UIManager.instance.ShowScore();
            score = UIManager.instance.score;

            if (score > bestScore)
            {
                bestScore = score;
            }
        }
    }
}
