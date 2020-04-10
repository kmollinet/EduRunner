using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using AsyncQuizLoader;
using EduRunner;
using GraphQL;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;


public class ChooseAQuiz : MonoBehaviour
{
    public static QuizLoader quizLoader = new QuizLoader();
    public static Task<Quiz[]> quizlist;
    private int buttonAdder = 0;
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;
    public GameObject Button4;
    public GameObject Button5;



    public static void GetAllQuizzesAsync()
    {
        quizlist = quizLoader.GetAllQuizzes();      
    }
    // private static void passAllQuizzes(Quiz[] listQuizzesResponse)
    // {
    //     quizlist = listQuizzesResponse;
    //     Debug.Log(quizlist[0].Name);
    // }

    // Task<Quiz[]> GetAllQuizzes()
    // {
    //     // return await quizLoader.GetAllQuizzes();
    // }

    // public static async void GetQuizByIdAsync()
    // {
    //     var quizId = "b273fab4-6ee1-46f9-91ca-2251c7c4788a";
    //     var getQuizResponse = await quizLoader.GetQuizById(quizId);
    //     Quiz quiz = await quizLoader.GetQuizById(quizId);
    //     // Debug.Log(quiz);
        
    // }

    // Start is called before the first frame update
    void Start()
    {
        GetAllQuizzesAsync();     
    }

    // Update is called once per frame
    void Update()
    {
        if(quizlist.IsCompleted)
        {
            if(1 + buttonAdder <= quizlist.Result.Length){
                Button1.SetActive(true);
                Button1.GetComponentInChildren<Text>().text =  (1 + buttonAdder).ToString() + ". " + quizlist.Result[0 + buttonAdder].Name;
            }
            else{
                Button1.SetActive(false);
            }
            if(2 + buttonAdder <= quizlist.Result.Length){
                Button2.SetActive(true);
                Button2.GetComponentInChildren<Text>().text = (2 + buttonAdder).ToString() + ". " + quizlist.Result[1 + buttonAdder].Name;
            }
            else{
                Button2.SetActive(false);
            }
            if(3 + buttonAdder <= quizlist.Result.Length){
                Button3.SetActive(true);
                Button3.GetComponentInChildren<Text>().text = (3 + buttonAdder).ToString() + ". " + quizlist.Result[2 + buttonAdder].Name;
            }
            else{
                Button3.SetActive(false);
            }
            if(4 + buttonAdder <= quizlist.Result.Length){
                Button4.SetActive(true);
                Button4.GetComponentInChildren<Text>().text = (4 + buttonAdder).ToString() + ". " + quizlist.Result[3 + buttonAdder].Name;
            }
            else{
                Button4.SetActive(false);
            }
            if(5 + buttonAdder <= quizlist.Result.Length){
                Button5.SetActive(true);
                Button5.GetComponentInChildren<Text>().text = (5 + buttonAdder).ToString() + ". " + quizlist.Result[4 + buttonAdder].Name;;
            }
            else{
                Button5.SetActive(false);
            }

        }

    }

    public void onQuizChoice()
    {
        // PlayerPrefs.SetFloat("Highscore", score);
        if(EventSystem.current.currentSelectedGameObject.name == "Button1"){
            PlayerPrefs.SetInt("QuizIndex", 0);
        }
        else if(EventSystem.current.currentSelectedGameObject.name == "Button2"){
            PlayerPrefs.SetInt("QuizIndex", 1);
        }
        else if(EventSystem.current.currentSelectedGameObject.name == "Button3"){
            PlayerPrefs.SetInt("QuizIndex", 2);
        }
        else if(EventSystem.current.currentSelectedGameObject.name == "Button4"){
            PlayerPrefs.SetInt("QuizIndex", 3);
        }
        else if(EventSystem.current.currentSelectedGameObject.name == "Button5"){
            PlayerPrefs.SetInt("QuizIndex", 4);
        }
        Debug.Log(EventSystem.current.currentSelectedGameObject.name);
        SceneManager.LoadScene("Game");
    }

    public void buttonUp()
    {
        Debug.Log("Button up!");
        if(buttonAdder > 5){
            buttonAdder -= 5;
        }
        else{
            buttonAdder = 0;
        }
    }
    public void buttonDown()
    {
        if(quizlist.IsCompleted){
            if(buttonAdder < quizlist.Result.Length - 1){
                buttonAdder += 5;
            }
        }
    }
}
