using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using AsyncQuizLoader;
using EduRunner;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;



public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveVector;
    private float speed = 5.0f;
    private float verticalVelocity = 0.0f;
    private float animationDuration = 3.0f;
    private float gravity = 12.0f;
    private bool isDead = false;
    private float startTime = 0.0f;
    private int totalQuestions;
    private int questionNum = 0;
    public Text totalQuestionsText;
    public Text numberCorrect;
    private int numCorrect = 0;
    private int correctAnsPosition;




    public Animator anim;
    // Color blue = new Color32(128, 255, 128, 140);
    Color blue = new Color32(115, 164, 229, 255);
    Color red = new Color32(191, 105, 105, 255);

    Color black = new Color32(86, 89, 94, 255);
    Color rightAnswerColor = new Color32(0, 255, 0, 255);

    Color wrongAnswerColor = new Color32(234, 74, 70, 255);



    Color invisible = new Color32(0, 0, 0, 0);

    private Vector3 scrollVector;
    public GameObject questionImage;
    public GameObject answer01;
    public GameObject answer02;
    private Vector3 answer03Vector;
    public GameObject answer03;

    public GameObject tileManager;
    public GameObject treasureChest;
    private Vector3 treasureVector;
    private bool placeTreasureChest = false;



    private QuestionSet qs;


    //  This "GetData()" Method does nothing - it's just an example of how to hit an API using standard REST API HTTP calls
     public static async void GetData()
    {
        string baseUrl = "https://jsonplaceholder.typicode.com/todos/2";
        using (HttpClient client = new HttpClient())
        {
            using (HttpResponseMessage res = await client.GetAsync(baseUrl))
            {
                using (HttpContent content = res.Content)
                {
                    var data = await content.ReadAsStringAsync();
                    if (data != null)
                    {
                        Debug.Log(data);
                    }
                    else 
                    {
                        Debug.Log("NO Data----------");
                    }
                }
            }
        }
    }

    public static QuizLoader quizLoader = new QuizLoader();
    public static Task<Quiz[]> quizlist = quizLoader.GetAllQuizzes();


    public Quiz quiz;

    // Start is called before the first frame update
    void Start()
    {
        GetData();
        // GetDataQuizLoader();
        
        controller = GetComponent<CharacterController>();
        startTime = Time.time;
        questionNum = 1;
        totalQuestionsText.text = "Question " + questionNum.ToString() + "/" + totalQuestions.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(quizlist.IsCompleted)
        {
            var quizIndex = PlayerPrefs.GetInt("QuizIndex");
            qs = QuestionSet.FromJson(JsonConvert.SerializeObject(quizlist.Result[quizIndex]));
            totalQuestions = (int)quizlist.Result[quizIndex].QuestionsPerQuiz;
            if(totalQuestions > quizlist.Result[quizIndex].Questions.Count){
                totalQuestions = quizlist.Result[quizIndex].Questions.Count;
            }
            tileManager.GetComponent<TileManager>().DetermineTotalQuestions(totalQuestions); 
            Debug.Log(totalQuestions);   
            if (qs.Questions.Count < 3)
            { // We can't handle less than 3 questions in a set because we need to display 3 answers.
                Death();
            }
            if (qs.Questions.Count < totalQuestions)
            {
                totalQuestions = qs.Questions.Count;
            }
        }
        totalQuestionsText.text = "Question " + questionNum.ToString() + "/" + totalQuestions.ToString();

        numberCorrect.text = numCorrect.ToString() + "/" + (questionNum).ToString() + " Correct";
        if (isDead)
            return;

        if (Time.time - startTime < animationDuration)
        {
            controller.Move(Vector3.forward * speed * Time.deltaTime);
            return;
        }
        moveVector = Vector3.zero;

        if (controller.isGrounded)
        {
            verticalVelocity = -0.5f;
        }
        else
        {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        // X
        moveVector.x = Input.GetAxisRaw("Horizontal") * speed;
        if (Input.GetMouseButton(0))
        {
            //right side of screen
            if (Input.mousePosition.x > Screen.width / 2)
            {
                moveVector.x = speed;
            }
            else
            {
                moveVector.x = -speed;
            }
        }
        // y
        moveVector.y = verticalVelocity;
        // z
        moveVector.z = speed;


        controller.Move(moveVector * Time.deltaTime);
    }

    public void SetSpeed(float modifier)
    {
        speed = 5.0f + modifier;
    }

    //called when player hits something
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "endGame")
            Death();
        if (hit.gameObject.tag == "enemy")
            hitEnemy(hit.gameObject);
        // if(hit.gameObject.tag == "enemy" && hit.point.z > transform.position.z + controller.radius)
        // Death ();

        if (hit.gameObject.tag == "leftLaneQuestion" || hit.gameObject.tag == "middleLaneQuestion" || hit.gameObject.tag == "rightLaneQuestion")
        {
            if (GameObject.FindWithTag("scroll"))
            {
                GameObject scroll = GameObject.FindWithTag("scroll");
                scrollVector.x = 0;
                scrollVector.y = 2.0f;
                scrollVector.z = transform.position.z + 4.0f;
                scroll.transform.position = scrollVector;
            }
            else
            {
                GameObject go;
                go = Instantiate(questionImage) as GameObject;
                Invoke("SetQuestionAfterDelay", 1);
            }
            initializeObject(answer01, "answer01", -1.0f, 2.0f, 2.0f);
            initializeObject(answer02, "answer02", 0.0f, 2.0f, 2.0f);
            initializeObject(answer03, "answer03", 1.0f, 2.0f, 2.0f);
        }
        if (hit.gameObject.tag == "leftlane" || hit.gameObject.tag == "middlelane" || hit.gameObject.tag == "rightlane")
        {
            if (GameObject.FindWithTag("scroll"))
            {
                Destroy(GameObject.FindWithTag("scroll"));
                // if(questionNum == totalQuestions - 1 || questionNum == qs.Questions.Count - 1){
                //     tileManager.GetComponent<TileManager>().MarkLastQuestion(); 
                // }
                // if(questionNum >= totalQuestions || questionNum >= qs.Questions.Count){
                //     tileManager.GetComponent<TileManager>().EndGame(); 
                // }
                // else{
                qs.Next();
                questionNum += 1;
                // }
            }
            if (GameObject.FindWithTag("answer01"))
            {
                Destroy(GameObject.FindWithTag("answer01"));
            }
            if (GameObject.FindWithTag("answer02"))
            {
                Destroy(GameObject.FindWithTag("answer02"));
            }
            if (GameObject.FindWithTag("answer03"))
            {
                Destroy(GameObject.FindWithTag("answer03"));
            }
        }
        if (hit.gameObject.tag == "leftlane" || hit.gameObject.tag == "leftLaneQuestion")
        {
            laneColorChange("middlelane", invisible);
            laneColorChange("leftlane", blue);
            laneColorChange("rightlane", invisible);
            laneColorChange("middleLaneQuestion", invisible);
            laneColorChange("leftLaneQuestion", blue);
            laneColorChange("rightLaneQuestion", invisible);
        }
        else if (hit.gameObject.tag == "middlelane" || hit.gameObject.tag == "middleLaneQuestion")
        {
            laneColorChange("middlelane", blue);
            laneColorChange("leftlane", invisible);
            laneColorChange("rightlane", invisible);
            laneColorChange("middleLaneQuestion", blue);
            laneColorChange("leftLaneQuestion", invisible);
            laneColorChange("rightLaneQuestion", invisible);
        }
        else if (hit.gameObject.tag == "rightlane" || hit.gameObject.tag == "rightLaneQuestion")
        {
            laneColorChange("middlelane", invisible);
            laneColorChange("leftlane", invisible);
            laneColorChange("rightlane", blue);
            laneColorChange("middleLaneQuestion", invisible);
            laneColorChange("leftLaneQuestion", invisible);
            laneColorChange("rightLaneQuestion", blue);
        }
        else if (hit.gameObject.tag == "gameModeTile")
        {
            laneColorChange("middlelane", invisible);
            laneColorChange("leftlane", invisible);
            laneColorChange("rightlane", invisible);
        }
        GameObject scroll2 = GameObject.FindWithTag("scroll");

        if (hit.gameObject.tag == "LeftDoorChoice" && correctAnsPosition == 1)
        {
            if (scroll2)
            {
                Text scroll2Text = scroll2.GetComponentInChildren<Text>();
                scroll2Text.text = "Correct!";
                scroll2Text.color = rightAnswerColor;
                scroll2Text.fontSize = 36;
            }
            numCorrect += 1;
            placeTreasureChest = true;
            Destroy(hit.gameObject);
        }
        if (hit.gameObject.tag == "MiddleDoorChoice" && correctAnsPosition == 2)
        {
            if (scroll2)
            {
                Text scroll2Text = scroll2.GetComponentInChildren<Text>();
                scroll2Text.text = "Correct!";
                scroll2Text.color = rightAnswerColor;
                scroll2Text.fontSize = 36;
            }
            numCorrect += 1;
            placeTreasureChest = true;
            Destroy(hit.gameObject);
        }
        if (hit.gameObject.tag == "RightDoorChoice" && correctAnsPosition == 3)
        {
            if (scroll2)
            {
                Text scroll2Text = scroll2.GetComponentInChildren<Text>();
                scroll2Text.text = "Correct!";
                scroll2Text.color = rightAnswerColor;
                scroll2Text.fontSize = 36;
            }
            numCorrect += 1;
            placeTreasureChest = true;
            Destroy(hit.gameObject);
        }
        if (placeTreasureChest)
        {
            GameObject treasure = Instantiate(treasureChest) as GameObject;
            treasureVector.x = 0.0f;
            treasureVector.y = 0.0f;
            treasureVector.z = transform.position.z + 14.0f;
            treasure.transform.position = treasureVector;
            placeTreasureChest = false;
        }

        if (hit.gameObject.tag == "LeftDoorChoice" && correctAnsPosition != 1)
        {
            if (scroll2)
            {
                Text scroll2Text = scroll2.GetComponentInChildren<Text>();
                scroll2Text.text = "Wrong Answer";
                scroll2Text.color = wrongAnswerColor;
                scroll2Text.fontSize = 36;
            }
            Destroy(hit.gameObject);
        }
        if (hit.gameObject.tag == "MiddleDoorChoice" && correctAnsPosition != 2)
        {
            if (scroll2)
            {
                Text scroll2Text = scroll2.GetComponentInChildren<Text>();
                scroll2Text.text = "Wrong Answer";
                scroll2Text.color = wrongAnswerColor;
                scroll2Text.fontSize = 36;
            }
            Destroy(hit.gameObject);
        }
        if (hit.gameObject.tag == "RightDoorChoice" && correctAnsPosition != 3)
        {
            if (scroll2)
            {
                Text scroll2Text = scroll2.GetComponentInChildren<Text>();
                scroll2Text.text = "Wrong Answer";
                scroll2Text.color = wrongAnswerColor;
                scroll2Text.fontSize = 36;
            }
            Destroy(hit.gameObject);
        }
    }

    private void Death()
    {
        isDead = true;
        GetComponent<Score>().OnDeath();
        // anim.Play("Die");
    }

    private void hitEnemy(GameObject enemy)
    {
        Destroy(enemy);
        anim.Play("GetHit");
        anim.Play("infantry_03_run");
        GetComponent<Score>().OnHitEnemy();
    }

    public void SetQuestionAfterDelay()
    {
        correctAnsPosition = qs.SetQuestion();
    }



    private void laneColorChange(string lane, Color32 color)
    {
        GameObject[] rightLaneTiles = GameObject.FindGameObjectsWithTag(lane);
        foreach (GameObject go in rightLaneTiles)
        {
            MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer r in renderers)
            {
                foreach (Material m in r.materials)
                {
                    if (m.HasProperty("_Color"))
                        m.color = color;
                }
            }
        }
    }

    private void initializeObject(GameObject gameObj, string tag, float x, float y, float z)
    {
        if (GameObject.FindWithTag(tag))
        {
            GameObject gameObject = GameObject.FindWithTag(tag);
            answer03Vector.x = x;
            answer03Vector.y = y;
            answer03Vector.z = transform.position.z + z;
            gameObject.transform.position = answer03Vector;
        }
        else
        {
            Debug.Log("initializing");
            Debug.Log(tag);
            GameObject gameObject;
            gameObject = Instantiate(gameObj) as GameObject;
        }
    }
}
