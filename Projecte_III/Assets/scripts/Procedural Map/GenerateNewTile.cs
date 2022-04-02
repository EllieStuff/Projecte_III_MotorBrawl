using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class GenerateNewTile : MonoBehaviour
{
    [SerializeField] RoadData[] tiles;

    [SerializeField] RoadData lastTile = null;

    RoadData.SpawnRateSet maxSpawnRates = new RoadData.SpawnRateSet(0);

    List<RoadData> straightRoads = new List<RoadData>();
    List<RoadData> leftRoads = new List<RoadData>();
    List<RoadData> rightRoads = new List<RoadData>();


    // Start is called before the first frame update
    void Start()
    {
        GameObject[] tilesGO = Resources.LoadAll<GameObject>("Prefabs/ProceduralMap");
        tiles = new RoadData[tilesGO.Length];
        for (int i = 0; i < tilesGO.Length; i++)
        {
            tiles[i] = tilesGO[i].GetComponent<RoadData>();
            if (tiles[i].RoadType == RoadData.Type.STRAIGHT)
            {
                straightRoads.Add(tiles[i]);
                maxSpawnRates.straight += tiles[i].SpawnRate;
            }
            else if (tiles[i].RoadType == RoadData.Type.LEFT)
            {
                leftRoads.Add(tiles[i]);
                maxSpawnRates.left += tiles[i].SpawnRate;
            }
            else if (tiles[i].RoadType == RoadData.Type.RIGHT)
            {
                rightRoads.Add(tiles[i]);
                maxSpawnRates.right += tiles[i].SpawnRate;
            }
        }
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CalculateNewTile()
    {
        float random = Random.Range(0, 100);
        RoadData.Type roadType = lastTile.GetRoadType(random);
        RoadData newObject = null;
        if (roadType == RoadData.Type.STRAIGHT)
            newObject = GetNewRoad(ref straightRoads, maxSpawnRates.straight);
        else if (roadType == RoadData.Type.LEFT)
            newObject = GetNewRoad(ref leftRoads, maxSpawnRates.left);
        else if (roadType == RoadData.Type.RIGHT)
            newObject = GetNewRoad(ref rightRoads, maxSpawnRates.right);
        
        if (newObject == null) Debug.LogError("Upsie");


        Transform child = lastTile.transform.GetChild(0).Find("NewSpawn");

        newObject.transform.position = child.position;

        Vector3 _scale = new Vector3(0.5f, 0.5f, 0.5f);
        newObject.transform.localScale = _scale;
        newObject.transform.rotation = Quaternion.RotateTowards(newObject.transform.rotation, child.rotation, 360);

        if(transform.childCount > 2)
            Destroy(transform.GetChild(0).gameObject);

        lastTile = newObject;
    }


    RoadData GetNewRoad(ref List<RoadData> _roadList, float _maxSpawnRate)
    {
        float random = Random.Range(0, _maxSpawnRate);
        float currRndAmount = 0;
        foreach (RoadData road in _roadList)
        {
            if (random > currRndAmount && random < currRndAmount + road.SpawnRate)
            {
                GameObject newRoad = Instantiate(road.gameObject, transform);
                return newRoad.GetComponent<RoadData>();
            }

            currRndAmount += road.SpawnRate;
        }

        return null;
    }

}
