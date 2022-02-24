using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] roadPrefabs;
    [SerializeField] Transform startPoint;
    // Start is called before the first frame update

    public static RoadSpawner instance;
    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        SpawnNewRoad();
        SpawnNewRoad();
    }


    public void SpawnNewRoad()
    {
            GameObject randomPrefab = roadPrefabs[Random.Range(0, roadPrefabs.Length)];
            GameObject roadObject =  Instantiate(randomPrefab, startPoint.position, startPoint.rotation);

            startPoint = roadObject.GetComponent<RoadElement>().GetEndPoint();
    }

}
