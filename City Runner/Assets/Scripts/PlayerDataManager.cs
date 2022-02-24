using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerDataManager : MonoBehaviour
{
    // [SerializeField] CharacterMover characterMoveScript;
    [SerializeField] int playerHealth = 3;
    public Action<PlayerStates> PlayerStateAction;
    int totalCoinsTaken = 0;

void OnTriggerEnter(Collider colinfo)
{
    string tag = colinfo.gameObject.tag;
    switch(tag)
    {
        case "obstacle" : {
            // characterMoveScript.ChangePlayerState(PlayerStates.Idle); 
                           PlayerStateAction ?.Invoke(PlayerStates.Idle);
                            
                            playerHealth--;
                            if(playerHealth > 0)
                            GameMenuManager.instance.UpdateLifeText(playerHealth);
                            else
                                 PlayerStateAction ?.Invoke(PlayerStates.GameOver);
                            break;
                            }
        case "RoadSpawnTrigger" : {
                                    RoadSpawner.instance.SpawnNewRoad();
                                    break;}

        case "RoadDisableTrigger" : { colinfo.gameObject.transform.parent.gameObject.SetActive(false);
                                        break;
                                    }
           case "coin"             : {colinfo.gameObject.SetActive(false); 
                                        totalCoinsTaken++;
                                        GameMenuManager.instance.UpdateCoinTakenText(totalCoinsTaken);
                                    break;}
    }

}   





}

public enum PlayerStates{

    Idle,
    Run,
    GameOver
}

