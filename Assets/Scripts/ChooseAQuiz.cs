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

public class ChooseAQuiz : MonoBehaviour
{
    private static Quiz[] quizlist;
    public static QuizLoader quizLoader = new QuizLoader();

    public static async void GetAllQuizzesAsync()
    {
        var listQuizzesResponse = await quizLoader.GetAllQuizzes();

        passAllQuizzes(listQuizzesResponse);
        Debug.Log(listQuizzesResponse[0].Name);
        // GraphQLResponse<ListQuizzesResponse> quizlist = await quizLoader.GetAllQuizzes();
        // Quiz[] quizlist = await quizLoader.GetAllQuizzes();
        // Debug.Log(quizlist[1].Name);       

        
    }
    private static void passAllQuizzes(Quiz[] listQuizzesResponse)
    {
        quizlist = listQuizzesResponse;
        Debug.Log(quizlist[0].Name);
    }

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
        // Task<Quiz[]> quizlist = GetDataQuizLoader();
        GetAllQuizzesAsync();
        
        // GameObject.FindWithTag("button1").GetComponentInChildren<Text>().text = quizlist[0].Name;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
