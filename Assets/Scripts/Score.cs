using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    private float score = 0.0f;

    private int difficultyLevel = 1;
    private int maxDifficultyLevel = 100;
    private int scoreToNextLevel = 10;
    private bool isDead = false;

    public Text scoreText;
    public Text questionText;
    public DeathMenu deathMenu;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
            return;
        if(score >= scoreToNextLevel)
            LevelUp();
        score += Time.deltaTime * difficultyLevel;
        scoreText.text = ((int)score).ToString();

        if(score > 3){
            questionText.text = "Is your score " + ((int)score).ToString() + "?";
        }
        if(score > 6){
            questionText.text = "What is 8 X 8?";
        }
        
    }

    void LevelUp()
    {
        if(difficultyLevel == maxDifficultyLevel)
            return;
        scoreToNextLevel *=2;
        difficultyLevel++;

        GetComponent<PlayerMotor>().SetSpeed(difficultyLevel);
    
        Debug.Log(difficultyLevel);
    }

    public void OnDeath()
    {
        isDead = true;
        if (score > PlayerPrefs.GetFloat("Highscore")){
            PlayerPrefs.SetFloat("Highscore", score);
        }
        deathMenu.ToggleEndMenu(score);
    }
}
