using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class View : MonoBehaviour
{
    public Text lbTitleModeGame;
    public Text lbValueModeGame;

    public Text lbHighScore;
    public Text lbTitleHighScore;
    
    public Text lbCurrentScore;
    public Text lbTitleCurrentScore;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnEnterGame()
    {
        lbHighScore.text = "" + GameManager.Instance.highScore;
        lbCurrentScore.text = "" + GameManager.Instance.currentScore;
        if (GameManager.Instance.modeGame == ModeGame.Countdown)
        {
            lbTitleModeGame.text = "Timer";
            lbValueModeGame.text = "" + GameManager.Instance.threshold;
        }
        else
        {
            lbTitleModeGame.text = "Moving";
            lbValueModeGame.text = "";
        }

        lbTitleCurrentScore.text = "Score";
        lbTitleHighScore.text = "High Score";
    }

    public void UpdateScore()
    {
        lbCurrentScore.text = "" + GameManager.Instance.currentScore;
        lbHighScore.text = "" + GameManager.Instance.highScore;
    }
}
