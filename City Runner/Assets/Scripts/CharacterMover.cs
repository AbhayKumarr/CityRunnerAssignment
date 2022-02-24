using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMover : MonoBehaviour
{

    [Header("Assign 3 lane Transforms")]
    [SerializeField] Transform [] LanePositions; // Three lane positions 
    // We only need X positoin from our Lane Positions
    private float NewXPos;
    // our character controller reference
    CharacterController controller;
    // To check if we are swiping left, right or up
    bool SwipeLeft, SwipeRight, SwipeUp, SwipeDown;

    // variable to track current lane of player
    Lanes currentLane = Lanes.Mid;
    // Player Animator component to change the animation
    Animator playerAnimator;

    // variable to keep track if player current state is idle or running
    bool IsPlayerIdle = true;
    
    // variable to check if player is jumping ( in air)
    bool IsJumping = false;

    // bool IsRolling = false;
    [SerializeField] float forwardSpeed = 1;

    // 
    float LaneXValue;
    [Header("move sensitivity on switching lanes")]
    [SerializeField] float changeLaneSensitivity =1;

    [Header("Jump power of player")]
    [SerializeField] float jumpPower = 7;

    // y direction velocity update when player jumps
    private float yVelocity;

    // time taken by player to travel the distance
    float timeTaken = 0;

    // starting position of player to track the overall distance travelled by player
    float startingPosOfPlayer;
    void Start()
    {
        //  Making sure by default player is in the mid position when game starts
        NewXPos = LanePositions[1].position.x;              
        
        // Getting reference of our character controller reference
        controller = GetComponent<CharacterController>();

        // Getting reference of our Animator component
        playerAnimator = GetComponent<Animator>();

        // By default player state should be idle
        ChangePlayerState(PlayerStates.Idle);

        // Subscribing player state actoin change 
        FindObjectOfType<PlayerHealthManager>().PlayerStateAction += ChangePlayerState;
    
        // Getting Starting positoin of player to track it's overall distance
        startingPosOfPlayer = transform.position.z;

        // Subscribing to Swipe Controlls
        SwipeEvents.OnSwipeUp += SwipeUpEvent;
        SwipeEvents.OnSwipeLeft += SwipeLeftEvent;
        SwipeEvents.OnSwipeRight += SwipeRightEvent;
    }

    void SwipeUpEvent()
    {
        SwipeUp = true;
    }
    void SwipeRightEvent()
    {
        SwipeRight = true;
    }
    void SwipeLeftEvent()
    {
        SwipeLeft = true;
    }

    // Update is called once per frame
    void Update()
    {
        // when player is in idle state,  do not move the player forward or left, right direction
        if(IsPlayerIdle)
            return;
      
        JumpPlayer();

        UpdateCurrentScore();
        // // Detect input
        // SwipeLeft = Input.GetKeyDown(KeyCode.A);
        // SwipeRight = Input.GetKeyDown(KeyCode.D);
        // SwipeUp = Input.GetKeyDown(KeyCode.W);

        // If player Did swipe Left
        if(SwipeLeft)
        {
                if(currentLane == Lanes.Mid)
                        {
                            // modifying the new x position as per the lane
                            NewXPos = LanePositions[0].position.x;
                            
                            // modifying the current lane status
                            currentLane = Lanes.Left;
                            // playing the animation to move
                            playerAnimator.Play("Left Move");   
                        }
                else if(currentLane == Lanes.Right)
                    {
                        // modifying the new x position as per the lane
                        NewXPos = LanePositions[1].position.x;
                       
                        // modifying the current lane status
                        currentLane = Lanes.Mid;

                         // playing the animation to move
                        playerAnimator.Play("Left Move");
                    }
        }
        else
        // If player did Swipe Right
        if(SwipeRight)
        {
            if(currentLane == Lanes.Mid)
                {
                    NewXPos = LanePositions[2].position.x;
                     
                     // modifying the current lane status
                    currentLane = Lanes.Right;

                     // playing the animation to move
                    playerAnimator.Play("Right Move");
                }
            else if(currentLane == Lanes.Left)
                    {
                         // modifying the new x position as per the lane
                        NewXPos = LanePositions[1].position.x;

                         // modifying the current lane status
                        currentLane = Lanes.Mid;

                         // playing the animation to move
                        playerAnimator.Play("Right Move");
                    }
        }

         // Calculating moveVector to feed in character controller

        Vector3 moveVector = new Vector3(LaneXValue - transform.position.x, yVelocity*Time.deltaTime, forwardSpeed * Time.deltaTime);
        
        // Smoothly moving player to the new Lane Position
        LaneXValue = Mathf.Lerp(LaneXValue, NewXPos, Time.deltaTime * changeLaneSensitivity);
        controller.Move(moveVector);

        // disable all swipe boolean
        SwipeRight = false;
        SwipeLeft = false;
        SwipeUp = false;
    }   


    public void JumpPlayer()
    {
       // jump only if player is grounded and swipes up
        if(controller.isGrounded && SwipeUp)
        {       
            // Modify vertical yVelocity with jumpPower
                yVelocity = jumpPower;
                playerAnimator.Play("Jump");
                IsJumping = true;
        }
        else 
        {   
            // Constantlyy apply some downward forece to the player, to make player fall again after he jumps
            yVelocity -= jumpPower * 2 * Time.deltaTime;
        }
    }



// This function is called from PlayerHealthManager to modify player animation state
public void ChangePlayerState(PlayerStates state)
{
    if(state == PlayerStates.Idle)
    {
        IsPlayerIdle = true;
        playerAnimator.Play("Idle");
    }
    else if(state == PlayerStates.Run)
    {
        DetectAndRemoveObstacles();
            IsPlayerIdle = false;
           playerAnimator.Play("Run Animation");
    }
    else if(state == PlayerStates.GameOver)
    {
        float distanceTravelledOverAll = transform.position.z - startingPosOfPlayer;
       
    }
}

// This function is called from update function. It calculates the time and distance travelled when player is moving
void UpdateCurrentScore()
{
    timeTaken += Time.deltaTime;
    float distanceTravelled = transform.position.z - startingPosOfPlayer;

    // CalculateDistanceTravelled();
    GameMenuManager.instance.UpdateScores(timeTaken, distanceTravelled);
}


// When player hits obstacle,  then he start the game again, at that time remove some front obstacles
void DetectAndRemoveObstacles()
{
    var dectectedobjects=Physics.SphereCastAll(transform.position-transform.forward * 13f, 20f,transform.forward, 35);
    foreach(var item in dectectedobjects)
    {
        if(item.transform.tag == "obstacle")
            item.transform.gameObject.SetActive(false);
    }
}

}

// public enum to keep track of the lanes whether player is on left, right or mid lane
public enum Lanes
{
    Left,
    Mid,
    Right
}

