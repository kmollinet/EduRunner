using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class EndingMenu : MonoBehaviour
{
    public Text scoreText;
    public GameObject endMenu;

    private float startTime = 0.0f;
    private float animationDuration = 2.0f;

    // Start is called before the first frame update
    void Start()
    {
        /*Do not show in the beginning*/
        endMenu.SetActive(false);

        startTime = Time.time;

        scoreText.text = ((int)PlayerPrefs.GetFloat("currentScore")).ToString();

    }

    // Update is called once per frame
    void Update()
    {
        /*Show UI after camera zoomed in*/

        //If camera finished its animation to zoom in, show UI
        if (Time.time - startTime > animationDuration)
        {
            endMenu.SetActive(true);
        }

    }

    public void Restart()
    {
        SceneManager.LoadScene("Game");
    }

    public void ToMenu()
    {
        SceneManager.LoadScene("Menu");

    }

    public void ToScoreboard()
    {
        SceneManager.LoadScene("Scoreboard");
    }
}
