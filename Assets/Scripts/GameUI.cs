using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class GameUI : MonoBehaviour
{
    public GameObject pauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        Time.timeScale = 1;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Pause()
    {
        Time.timeScale = 0;
        pauseMenu.SetActive(true);
    }

    public void Resume()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);

    }

    public void ToMenu()
    {
        Time.timeScale = 1;
        pauseMenu.SetActive(false);
        SceneManager.LoadScene("SelectQuiz");
    }
}
