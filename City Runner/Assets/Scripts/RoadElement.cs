using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadElement : MonoBehaviour
{
    [SerializeField] Transform endPoint;
    int currentRoadVariant = 0;
  

    public Transform GetEndPoint()
    {
        return endPoint;
    }

    [SerializeField] GameObject [] RoadObstacleVariants;
    void OnEnable()
    {
        foreach(var item in RoadObstacleVariants)
            item.SetActive(false);
        
        int randomInt = Random.Range(0, RoadObstacleVariants.Length);
        currentRoadVariant = randomInt;

        for(int i=0; i<RoadObstacleVariants.Length; i++)
            {
                if(i == randomInt)
                    RoadObstacleVariants[i].SetActive(true);
                else RoadObstacleVariants[i].SetActive(false);
            }
    }

     



}
