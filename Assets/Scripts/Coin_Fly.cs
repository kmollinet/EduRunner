using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin_Fly : MonoBehaviour
{
    private float startTime = 0.0f;
    private Vector3 diagonalVector;

    public GameObject coin_fly;

    // Start is called before the first frame update
    void Start()
    {
        startTime = Time.time;
        // diagonalVector = Vector3.up;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 destinationVector = new Vector3(0.1f, 0.1f, 0.1f);
        transform.Rotate(0,1,0);
        transform.Translate(Vector3.back * Time.deltaTime * 16.0f, Space.World);
        transform.Translate(Vector3.right * Time.deltaTime * 8.0f, Space.World);  
        transform.localScale = Vector3.Lerp(transform.localScale, destinationVector, Time.time - startTime);
        // diagonalVector.x = 
        // transform.position = Vector3.Lerp(transform, moveVector, transition);
        // transition += Time.deltaTime * 1 / animationDuration;
        if(Time.time - startTime > 0.2f){
            Destroy(coin_fly);
        } 
    }
}
