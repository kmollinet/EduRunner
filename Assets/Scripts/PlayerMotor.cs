using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMotor : MonoBehaviour
{
    private CharacterController controller;
    private Vector3 moveVector;
    private float speed = 7.0f;    
    private float verticalVelocity = 0.0f;
    private float animationDuration = 3.0f;
    private float gravity = 12.0f;
    private bool isDead = false;
    private float startTime = 0.0f;
    public Animator anim;
    Color green = new Color32(128, 255, 128, 255);
    Color invisible = new Color32(0,0,0,0);
    Color white = new Color32(255,255,255,255);

    // private Transform answer1Transform;
    // private Transform answer2Transform;
    // private Transform answer3Transform;

    private Vector3 moveVectorAnswer1;
    private Vector3 moveVectorAnswer2;
    private Vector3 moveVectorAnswer3;
    private Vector3 scrollVector;

    private Text questionText;
    public GameObject questionImage;
    public Text answer1Text;
    public Image answer1Image;
    public Text answer2Text;
    public Image answer2Image;
    public Text answer3Text;
    public Image answer3Image;

    public QuestionSet qs;
    
    // Start is called before the first frame update
    void Start()
    {
        qs = QuestionSet.Init("sample_question_set.json");
        qs.answer1Transform = GameObject.FindGameObjectWithTag("answer1").transform;
        qs.answer2Transform = GameObject.FindGameObjectWithTag("answer2").transform;
        qs.answer3Transform = GameObject.FindGameObjectWithTag("answer3").transform;
        // answer1Transform = GameObject.FindGameObjectWithTag("answer1").transform;
        // answer2Transform = GameObject.FindGameObjectWithTag("answer2").transform;
        // answer3Transform = GameObject.FindGameObjectWithTag("answer3").transform;

        answer1Image.color = invisible; 
        answer1Text.text = "";
        answer2Image.color = invisible;
        answer2Text.text = "";
        answer3Image.color = invisible;
        answer3Text.text = "";
        controller = GetComponent<CharacterController>();
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if(isDead)
            return;

        if(Time.time - startTime < animationDuration){
            controller.Move(Vector3.forward * speed * Time.deltaTime);
            return;
        }
        moveVector = Vector3.zero;

        if (controller.isGrounded){
            verticalVelocity = -0.5f;
        }
        else {
            verticalVelocity -= gravity * Time.deltaTime;
        }
        // X
        moveVector.x =  Input.GetAxisRaw("Horizontal") * speed;
        if(Input.GetMouseButton(0))
        {
            //right side of screen
            if(Input.mousePosition.x > Screen.width / 2)
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

        // AnswersMovement
        moveVectorAnswer1.x = -2;
        moveVectorAnswer1.y = 0;
        moveVectorAnswer1.z = transform.position.z + 2.0f;

        moveVectorAnswer2.x = 0;
        moveVectorAnswer2.y = 0;
        moveVectorAnswer2.z = transform.position.z + 2.0f;

        moveVectorAnswer3.x = 2;
        moveVectorAnswer3.y = 0;
        moveVectorAnswer3.z = transform.position.z + 2.0f;

        qs.answer1Transform.position = moveVectorAnswer1;
        qs.answer2Transform.position = moveVectorAnswer2;
        qs.answer3Transform.position = moveVectorAnswer3;

    }    

    public void SetSpeed(float modifier)
    {
        speed = 5.0f + modifier;
    }

    //called when player hits something
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.tag);
        // if(hit.point.z > transform.position.z + controller.radius && hit.gameObject.tag == "enemy")
        if(hit.gameObject.tag == "enemy")
            Death ();

        if(hit.gameObject.tag == "leftLaneQuestion" || hit.gameObject.tag == "middleLaneQuestion" || hit.gameObject.tag == "rightLaneQuestion")
        {
            Debug.Log("test!!!");

            if(GameObject.FindWithTag ("scroll")){
                GameObject scroll = GameObject.FindWithTag ("scroll");
                Debug.Log ("Exists");
                scrollVector.x = 0;
                scrollVector.y = 2.0f;
                scrollVector.z = transform.position.z + 2.0f;
                scroll.transform.position = scrollVector;
                Invoke("SetQuestionAfterDelay",1);
            }
            else{
                GameObject go;
                go = Instantiate(questionImage) as GameObject;
                // go.transform.SetParent(transform);
                
            }
        }
        if(hit.gameObject.tag == "leftlane" || hit.gameObject.tag == "middlelane" || hit.gameObject.tag == "rightlane")
        {
            Debug.Log("test!!!");
           // SetQuestion(white, qs.CurrentQuestion.QuestionText, qs.CurrentQuestion.AnswerText, qs.CurrentQuestion.IncorrectAnswers[0], qs.CurrentQuestion.IncorrectAnswers[1]);

            if(GameObject.FindWithTag ("scroll")){
                Destroy(GameObject.FindWithTag ("scroll"));
                SetQuestion(invisible, "", "", "", "");
            }
        }
        if (hit.gameObject.tag == "leftlane" || hit.gameObject.tag == "leftLaneQuestion"){
            laneColorChange("middlelane", invisible);
            laneColorChange("leftlane", green);
            laneColorChange("rightlane", invisible);
            laneColorChange("middleLaneQuestion", invisible);
            laneColorChange("leftLaneQuestion", green);
            laneColorChange("rightLaneQuestion", invisible);
        }
        else if(hit.gameObject.tag == "middlelane" || hit.gameObject.tag == "middleLaneQuestion"){
            laneColorChange("middlelane", green);
            laneColorChange("leftlane", invisible);
            laneColorChange("rightlane", invisible);
            laneColorChange("middleLaneQuestion", green);
            laneColorChange("leftLaneQuestion", invisible);
            laneColorChange("rightLaneQuestion", invisible);
        }     
        else if(hit.gameObject.tag == "rightlane" || hit.gameObject.tag == "rightLaneQuestion"){
            laneColorChange("middlelane", invisible);
            laneColorChange("leftlane", invisible);
            laneColorChange("rightlane", green);
            laneColorChange("middleLaneQuestion", invisible);
            laneColorChange("leftLaneQuestion", invisible);
            laneColorChange("rightLaneQuestion", green);
        }  
        else if(hit.gameObject.tag == "gameModeTile"){
            laneColorChange("middlelane", invisible);
            laneColorChange("leftlane", invisible);
            laneColorChange("rightlane", invisible);
        }      
    }

    private void Death()
    {
        Debug.Log("Dead");
        isDead = true;
        GetComponent<Score>().OnDeath();
        anim.Play("Die");
    }

    public void SetQuestionAfterDelay(){
        SetQuestion(white, qs.CurrentQuestion.QuestionText, qs.CurrentQuestion.AnswerText, qs.CurrentQuestion.IncorrectAnswers[0], qs.CurrentQuestion.IncorrectAnswers[1]);
        //SetQuestion(white, "What is the capital of Utah?", "Albany", "Salt Lake City", "Raleigh");
    }

    public void SetQuestion(Color newColor, string quesText, string ans1Text, string ans2Text, string ans3Text)
    {
        GameObject scroll = GameObject.FindWithTag ("scroll");
        Text scrollText = scroll.GetComponentInChildren<Text>();

        Debug.Log("test");
        Debug.Log(quesText);
        scrollText.text = quesText;
        answer1Image.color = newColor;
        answer1Text.text = ans1Text;
        answer2Image.color = newColor;
        answer2Text.text = ans2Text;
        answer3Image.color = newColor;
        answer3Text.text = ans3Text;
    }

    private void laneColorChange(string lane, Color32 color){
        GameObject[] rightLaneTiles = GameObject.FindGameObjectsWithTag(lane);
            foreach (GameObject go in rightLaneTiles) {
                MeshRenderer[] renderers = go.GetComponentsInChildren<MeshRenderer>();
                foreach (MeshRenderer r in renderers) {
                    foreach (Material m in r.materials) {
                        if (m.HasProperty("_Color"))
                            m.color = color;
                    }
                }
            }
    }
}
