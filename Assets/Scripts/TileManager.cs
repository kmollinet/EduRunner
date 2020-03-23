using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public GameObject[] questionTiles;
    public GameObject bridgeEnd;
    private Transform playerTransform;
    private float spawnZ = -16.0f;
    private float tileLength = 8.0f;
    private float safeZone = 15.0f;
    private int amnTilesOnScreen = 7;
    private int lastPrefabIndex = 0;
    private float tileTimer = 0.0f;
    private float questionTimer = 5.0f; //must be 5.0f or greater to make the game work correctly with the ending castle tile.
                                        //I actually force it to be 3.0f or higher in the start() method below to prevent errors here:)
    private Color green = new Color32(128, 255, 128, 255);
    private Color invisible = new Color32(0,0,0,0);
    private Color white = new Color32(255,255,255,255);
    private bool gameComplete = false;
    private bool  noMoreTiles = false;
    private bool thisIsTheLastQuestion = false;
    private bool noMoreQuestionModeTiles = false;

    private List<GameObject> activeTiles;
    // Start is called before the first frame update
    void Start()
    {
        if(questionTimer < 5.0f){
            questionTimer = 5.0f;
        }
        tileTimer = Time.time; //Start at 0 seconds
        activeTiles = new List<GameObject>();
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        
        for(int i = 0; i < amnTilesOnScreen; i++){
            if (i < 6)
                SpawnTile(0);
            else
                SpawnTile();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if(playerTransform.position.z - safeZone > (spawnZ - amnTilesOnScreen * tileLength)){
            SpawnTile();
            DeleteTile();
        }
    }
    
    private void SpawnTile(int prefabIndex = -1){
        GameObject go;
        if(noMoreTiles == false){ //don't make any more tiles after the ending tile
            if(prefabIndex == -1)
                if(Time.time - tileTimer > questionTimer && noMoreQuestionModeTiles == false) //if we mark that we don't want anymore question mode tiles, only generate the monster tiles until the game ends.
                {
                    if(Time.time - tileTimer > questionTimer * 2)
                    {
                        if(thisIsTheLastQuestion == true){
                            go = Instantiate(questionTiles[1]) as GameObject; //generate only 1 answers tile
                            tileTimer = Time.time; //Reset the timer so that we switch back to generating game mode tiles
                            noMoreQuestionModeTiles = true;
                        }
                        else{
                            go = Instantiate(questionTiles[1]) as GameObject; //generate only 1 answers tile
                            tileTimer = Time.time; //Reset the timer so that we switch back to generating game mode tiles
                        }
                    }
                    else
                    {
                        go = Instantiate(questionTiles[0]) as GameObject; //generate question mode tiles
                    }
                }
                else
                {
                    if(gameComplete == true){
                        go = Instantiate(bridgeEnd) as GameObject; 
                        noMoreTiles = true;
                    }
                    else{
                        go = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
                    }
                }
            
            else
                go = Instantiate(tilePrefabs[prefabIndex]) as GameObject;
            
        
            go.transform.SetParent(transform);
            go.transform.position = Vector3.forward * spawnZ;
            spawnZ += tileLength;
            activeTiles.Add(go);
        }
    }

    private void DeleteTile(int prefabIndex = -1)
    {
        if(gameComplete == false){
            Destroy (activeTiles[0]);
            activeTiles.RemoveAt(0);

        }
    }

    private int RandomPrefabIndex()
    {
        if(tilePrefabs.Length <= 1)
            return 0;

        int randomIndex = lastPrefabIndex;
        while (randomIndex == lastPrefabIndex)
        {
            randomIndex = Random.Range(0, tilePrefabs.Length);
        }

        lastPrefabIndex = randomIndex;
        return randomIndex;  
    }

    public void EndGame(){
        gameComplete = true;
    }

    public void MarkLastQuestion(){
        thisIsTheLastQuestion = true;
    }


}
