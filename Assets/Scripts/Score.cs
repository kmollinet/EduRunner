using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    public AudioSource tickSource;
    private float score = 0.0f;

    private int difficultyLevel = 1;
    private int maxDifficultyLevel = 100;
    private int scoreToNextLevel = 10;
    private bool isDead = false;
    private Vector3 untouchableCoinPosition;
    

    public Text scoreText;
    public DeathMenu deathMenu;
    public GameObject untouchableCoin;
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
        deathMenu.ToggleEndMenu(score);
    }

        //called when player hits something
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        // Debug.Log(hit.gameObject.tag);
        if(hit.gameObject.tag == "jewel")
        {
            tickSource.Play();
            GameObject go;
            go = Instantiate(untouchableCoin) as GameObject;        
            go.transform.SetParent(transform);
            untouchableCoinPosition.x = hit.gameObject.transform.position.x;
            untouchableCoinPosition.y = hit.gameObject.transform.position.y;
            untouchableCoinPosition.z = hit.gameObject.transform.position.z + 1.0f;
            go.transform.position = untouchableCoinPosition;

            Destroy(hit.gameObject);
            score += 10;
        }
        if(hit.gameObject.tag == "jewel5")
        {
            Destroy(hit.gameObject);
            score += 50;
        }
    }


}
