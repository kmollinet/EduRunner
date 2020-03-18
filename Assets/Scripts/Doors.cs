using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    Animator animator;
    public Animator anim;
    bool doorOpenBlue;
    bool doorOpenBlack;
    bool doorOpenRed;

    void Start(){
        doorOpenBlue = false;
        doorOpenBlack = false;
        doorOpenRed = false;
        animator = GetComponent<Animator>();
    }

    void OnTriggerEnter(Collider col)
    {
        Debug.Log(col.gameObject.tag);
        Debug.Log(tag);

        if(col.gameObject.tag == "Player" && tag == "doorsBlack") {
            doorOpenBlack = true;
            DoorControl("OpenBlack");
        }
        if(col.gameObject.tag == "Player" && tag == "doorsBlue") {
            doorOpenBlue = true;
            DoorControl("OpenBlue");
        }
        if(col.gameObject.tag == "Player" && tag == "doorsRed") {
            doorOpenRed = true;
            DoorControl("OpenRed");
        }
    }

    void OnTriggerExit(Collider col)
    {
        if(doorOpenBlack) {
            doorOpenBlack = false;
            DoorControl("CloseBlack");
        }
        if(doorOpenBlue) {
            doorOpenBlue = false;
            DoorControl("CloseBlue");
        }
        if(doorOpenRed) {
            doorOpenRed = false;
            DoorControl("CloseRed");
        }
    }
    void DoorControl(string direction)
    {
        animator.SetTrigger(direction);
    }
}
