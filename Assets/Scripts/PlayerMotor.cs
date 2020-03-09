using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


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
    public Animator anim;
    Color green = new Color32(128, 255, 128, 255);
    Color invisible = new Color32(0,0,0,0);
    Color white = new Color32(255,255,255,255);

    private Vector3 scrollVector;
    private Text questionText;
    public GameObject questionImage;

    private Vector3 answer01Vector;
    public GameObject answer01;
    private Vector3 answer02Vector;
    public GameObject answer02;
    private Vector3 answer03Vector;
    public GameObject answer03;


    private QuestionSet qs;
    
    // Start is called before the first frame update
    void Start()
    {
        qs = QuestionSet.Init("sample_question_set.json");

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
    }    

    public void SetSpeed(float modifier)
    {
        speed = 5.0f + modifier;
    }

    //called when player hits something
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if(hit.gameObject.tag == "enemy")
        {
            hitEnemy(hit.gameObject);
        }
        // if(hit.gameObject.tag == "enemy" && hit.point.z > transform.position.z + controller.radius)
            


            // Death ();

        if(hit.gameObject.tag == "leftLaneQuestion" || hit.gameObject.tag == "middleLaneQuestion" || hit.gameObject.tag == "rightLaneQuestion")
        {
            // Debug.Log("test!!!");

            if(GameObject.FindWithTag ("scroll")){
                GameObject scroll = GameObject.FindWithTag ("scroll");
                scrollVector.x = 0;
                scrollVector.y = 2.0f;
                scrollVector.z = transform.position.z + 4.0f;
                scroll.transform.position = scrollVector;
                Invoke("SetQuestionAfterDelay",1);
            }
            else{
                GameObject go;
                go = Instantiate(questionImage) as GameObject;
                // go.transform.SetParent(transform);
            }
            initializeObject(answer01, "answer01", -1.0f, 2.0f, 2.0f);
            initializeObject(answer02, "answer02", 0.0f, 2.0f, 2.0f);
            initializeObject(answer03, "answer03", 1.0f, 2.0f, 2.0f);
        }
        if(hit.gameObject.tag == "leftlane" || hit.gameObject.tag == "middlelane" || hit.gameObject.tag == "rightlane")
        {
            if(GameObject.FindWithTag ("scroll")){
                Destroy(GameObject.FindWithTag ("scroll"));
                qs.Next();
            }
            if(GameObject.FindWithTag ("answer01")){
                Destroy(GameObject.FindWithTag ("answer01"));
            }
            if(GameObject.FindWithTag ("answer02")){
                Destroy(GameObject.FindWithTag ("answer02"));
            }
            if(GameObject.FindWithTag ("answer03")){
                Destroy(GameObject.FindWithTag ("answer03"));
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
        isDead = true;
        GetComponent<Score>().OnDeath();
        anim.Play("Die");
    }

    private void hitEnemy(GameObject enemy)
    {
        Destroy(enemy);
        anim.Play("GetHit");
        anim.Play("infantry_03_run");
        GetComponent<Score>().OnHitEnemy();

    }

    public void SetQuestionAfterDelay(){
        qs.SetQuestion(qs.CurrentQuestion.QuestionText, qs.CurrentQuestion.AnswerText, qs.CurrentQuestion.IncorrectAnswers[0], qs.CurrentQuestion.IncorrectAnswers[1]);
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

    private void initializeObject(GameObject gameObj, string tag, float x, float y, float z)
    {
        if(GameObject.FindWithTag (tag)){
                GameObject gameObject = GameObject.FindWithTag (tag);
                answer03Vector.x = x;
                answer03Vector.y = y;
                answer03Vector.z = transform.position.z + z;
                gameObject.transform.position = answer03Vector;
            }
            else{
                GameObject gameObject;
                gameObject = Instantiate(gameObj) as GameObject;
            }
    }
}
