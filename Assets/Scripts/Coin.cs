using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    // public GameObject untouchableCoin;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0,1,0);
    }

    private void CoinFlyAway(int prefabIndex = -1){
        // GameObject go;
        // go = Instantiate(untouchableCoin) as GameObject;        
        // go.transform.SetParent(transform);
        // go.transform.position = Vector3.forward * spawnZ;
    }
}
