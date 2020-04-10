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
    // public static QuizLoader quizLoader = new QuizLoader();
    // public static Task<Quiz[]> quizlist;
    private int buttonAdder = 0;
    public GameObject Button1;
    public GameObject Button2;
    public GameObject Button3;
    public GameObject Button4;
    public GameObject Button5;



    // public static void GetAllQuizzesAsync()
    // {
    //     quizlist = quizLoader.GetAllQuizzes();      
    // }
    // private static void passAllQuizzes(Quiz[] listQuizzesResponse)
    // {
    //     quizlist = listQuizzesResponse;
    //     Debug.Log(quizlist[0].name);
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
        // GetAllQuizzesAsync();   
        Debug.Log("Calling ensure"); 
        QuestionSet.EnsureData(); 
    }

    // Update is called once per frame
    void Update()
    {
        if(QuestionSet.initialized)
        {
            // Debug.Log("inited");
            var quizlist = QuestionSet.GetAvailableQuizzes();
            if(1 + buttonAdder <= quizlist.Length){
                Button1.SetActive(true);
                Button1.GetComponentInChildren<Text>().text =  (1 + buttonAdder).ToString() + ". " + quizlist[0 + buttonAdder].name;
            }
            else{
                Button1.SetActive(false);
            }
            if(2 + buttonAdder <= quizlist.Length){
                Button2.SetActive(true);
                Button2.GetComponentInChildren<Text>().text = (2 + buttonAdder).ToString() + ". " + quizlist[1 + buttonAdder].name;
            }
            else{
                Button2.SetActive(false);
            }
            if(3 + buttonAdder <= quizlist.Length){
                Button3.SetActive(true);
                Button3.GetComponentInChildren<Text>().text = (3 + buttonAdder).ToString() + ". " + quizlist[2 + buttonAdder].name;
            }
            else{
                Button3.SetActive(false);
            }
            if(4 + buttonAdder <= quizlist.Length){
                Button4.SetActive(true);
                Button4.GetComponentInChildren<Text>().text = (4 + buttonAdder).ToString() + ". " + quizlist[3 + buttonAdder].name;
            }
            else{
                Button4.SetActive(false);
            }
            if(5 + buttonAdder <= quizlist.Length){
                Button5.SetActive(true);
                Button5.GetComponentInChildren<Text>().text = (5 + buttonAdder).ToString() + ". " + quizlist[4 + buttonAdder].name;
            }
            else{
                Button5.SetActive(false);
            }

        }

    }

    public void onQuizChoice()
    {
        if(QuestionSet.initialized){
            var quizlist2 = QuestionSet.GetAvailableQuizzes();
            // PlayerPrefs.SetFloat("Highscore", score);
            if(EventSystem.current.currentSelectedGameObject.name == "Button1"){
                QuestionSet.SetByIndex(0);
            }
            else if(EventSystem.current.currentSelectedGameObject.name == "Button2"){
                QuestionSet.SetByIndex(1);
            }
            else if(EventSystem.current.currentSelectedGameObject.name == "Button3"){
                QuestionSet.SetByIndex(2);
            }
            else if(EventSystem.current.currentSelectedGameObject.name == "Button4"){
                QuestionSet.SetByIndex(3);
            }
            else if(EventSystem.current.currentSelectedGameObject.name == "Button5"){
                QuestionSet.SetByIndex(4);
            }
            Debug.Log(EventSystem.current.currentSelectedGameObject.name);
            SceneManager.LoadScene("Game");
        }
    }

    public void buttonUp()
    {
        if(buttonAdder > 5){
            buttonAdder -= 5;
        }
        else{
            buttonAdder = 0;
        }
    }
    public void buttonDown()
    {
        if(QuestionSet.initialized){
            if(buttonAdder < QuestionSet.GetAvailableQuizzes().Length - 1){
                buttonAdder += 5;
            }
        }
    }
}
