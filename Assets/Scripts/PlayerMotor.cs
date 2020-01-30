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


    public Text questionText;
    public Image questionImage;
    
    // Start is called before the first frame update
    void Start()
    {
        questionImage.color = invisible;
        questionText.text = "";
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
        Debug.Log(hit.gameObject.tag);
        if(hit.point.z > transform.position.z + controller.radius && hit.gameObject.tag == "enemy")
            Death ();

        if(hit.gameObject.tag == "leftlane" || hit.gameObject.tag == "middlelane" || hit.gameObject.tag == "rightlane")
        {
            SetQuestion(white, "What is the capital of California?");
        }
        if (hit.gameObject.tag == "leftlane"){
            laneColorChange("middlelane", invisible);
            laneColorChange("leftlane", green);
            laneColorChange("rightlane", invisible);
        }
        else if(hit.gameObject.tag == "middlelane"){
            laneColorChange("middlelane", green);
            laneColorChange("leftlane", invisible);
            laneColorChange("rightlane", invisible);
        }     
        else if(hit.gameObject.tag == "rightlane"){
            laneColorChange("middlelane", invisible);
            laneColorChange("leftlane", invisible);
            laneColorChange("rightlane", green);
        }  
        else if(hit.gameObject.tag == "gameModeTile"){
            laneColorChange("middlelane", invisible);
            laneColorChange("leftlane", invisible);
            laneColorChange("rightlane", invisible);
            SetQuestion(invisible, "");

        }         
    }

    private void Death()
    {
        Debug.Log("Dead");
        isDead = true;
        GetComponent<Score>().OnDeath();
        anim.SetTrigger("isDead");
    }

    public void SetQuestion(Color newColor, string newText)
    {
        questionImage.color = newColor;
        questionText.text = newText;
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
