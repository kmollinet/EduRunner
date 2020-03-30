using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.Events; // This is so that you can extend the pointer handlers
using UnityEngine.EventSystems; // This is so that you can extend the pointer handlers
using System;

public class Score : MonoBehaviour
{
    public AudioSource tickSource;
    private float score = 0.0f;

    private int difficultyLevel = 1;
    private int maxDifficultyLevel = 100;
    private int scoreToNextLevel = 10;
    private bool isDead = false;
    private Vector3 untouchableCoinPosition;
    Color invisible = new Color32(0,0,0,0);

    public Text scoreText;
    public Text negativeScore;
    public GameObject untouchableCoin;
    public GameObject untouchableTreasure;


    // Start is called before the first frame update
    void Start()
    {
        tickSource = GetComponent<AudioSource> ();
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
            return;
        // if(score >= scoreToNextLevel) // commenting this out because we probably don't want to change speeds with kids
        //     LevelUp();
        score += Time.deltaTime * difficultyLevel;
        scoreText.text = ((int)score).ToString();        
    }

    void LevelUp()
    {
        if(difficultyLevel == maxDifficultyLevel)
            return;
        scoreToNextLevel *=2;
        difficultyLevel++;

        GetComponent<PlayerMotor>().SetSpeed(difficultyLevel);
    }

    public void OnDeath()
    {
        isDead = true;
        if (score > PlayerPrefs.GetFloat("Highscore")){
            PlayerPrefs.SetFloat("Highscore", score);
        }

        PlayerPrefs.SetFloat("currentScore", score);

        SceneManager.LoadScene("EndingMenu");

    }

        //called when player hits something
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "jewel" || hit.gameObject.tag == "treasure")
        {
            tickSource.Play();
            GameObject go;
            if(hit.gameObject.tag == "jewel"){
                go = Instantiate(untouchableCoin) as GameObject; 
                score += 1;
            }
            else{
                go = Instantiate(untouchableTreasure) as GameObject;   
                score += 50;     
            }
            go.transform.SetParent(transform);
            untouchableCoinPosition.x = hit.gameObject.transform.position.x;
            untouchableCoinPosition.y = hit.gameObject.transform.position.y;
            untouchableCoinPosition.z = hit.gameObject.transform.position.z + 1.0f;
            go.transform.position = untouchableCoinPosition;

            Destroy(hit.gameObject);

        }
    }

    public void OnHitEnemy()
    {
        if(score > 50)
        {
            score -= 50;
        }
        else
        {
            score = 0;
        }
        StartCoroutine(InvokeMethod(setColorRed, 0.4f, 3));
    }

     private void setColorRed()
     {
        negativeScore.color = Color.red;
        Invoke("setColorInvisible",0.2f);

     }
 
    private void setColorInvisible()
    {
        negativeScore.color = invisible;

    }
 
    public IEnumerator InvokeMethod(Action method, float interval, int invokeCount)
    {
        for (int i = 0; i < invokeCount; i++)
        {
            method();
            yield return new WaitForSeconds(interval);
        }
    }



}
