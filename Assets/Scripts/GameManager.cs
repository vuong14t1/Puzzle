using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ModeGame
{
    Normal,
    Countdown
}

public enum StateGame
{
    Playing,
    EndGame
}

public class GameManager : Singleton<GameManager>
{
    public int currentScore;

    public int highScore = 0;

    public ModeGame modeGame;

    public int threshold = 0;

    public StateGame stateGame;
    // Start is called before the first frame update
    void Start()
    {
        modeGame = ModeGame.Normal;
        stateGame = StateGame.Playing;
    }

    public void OnEnterGame()
    {
        highScore = currentScore;
        currentScore = 0;
    }

    public void OnEndGame()
    {
        stateGame = StateGame.EndGame;
    }

    public void CreaseScore(int score)
    {
        currentScore += score;
        if (currentScore > highScore)
        {
            highScore = currentScore;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
