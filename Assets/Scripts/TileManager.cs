using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TileManager : MonoBehaviour
{
    public GameObject[] tilePrefabs;
    public GameObject[] questionTiles;
    private Transform playerTransform;
    private float spawnZ = -16.0f;
    private float tileLength = 12.0f;
    private float safeZone = 15.0f;
    private int amnTilesOnScreen = 7;
    private int lastPrefabIndex = 0;
    private float tileTimer = 0.0f;
    private float questionTimer = 5.0f;
    private Color green = new Color32(128, 255, 128, 255);
    private Color invisible = new Color32(0,0,0,0);
    private Color white = new Color32(255,255,255,255);

    private List<GameObject> activeTiles;
    // Start is called before the first frame update
    void Start()
    {
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
        if(prefabIndex == -1)
            if(Time.time - tileTimer > questionTimer)
            {
                if(Time.time - tileTimer > questionTimer * 2)
                {
                    go = Instantiate(questionTiles[1]) as GameObject; //generate only 1 answers tile
                    tileTimer = Time.time; //Reset the timer so that we switch back to generating game mode tiles
                }
                else
                {
                    go = Instantiate(questionTiles[0]) as GameObject; //generate question mode tiles
                }
            }
            else
            {
                go = Instantiate(tilePrefabs[RandomPrefabIndex()]) as GameObject;
            }
        
        else
            go = Instantiate(tilePrefabs[prefabIndex]) as GameObject;
        go.transform.SetParent(transform);
        go.transform.position = Vector3.forward * spawnZ;
        spawnZ += tileLength;
        activeTiles.Add(go);
    }

    private void DeleteTile(int prefabIndex = -1)
    {
        Destroy (activeTiles[0]);
        activeTiles.RemoveAt(0);
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


}
