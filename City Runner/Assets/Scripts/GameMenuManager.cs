using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class GameMenuManager : MonoBehaviour
{

    public static GameMenuManager instance; 

    // PlayerMover script reference to start player movement when Start button is clicked
    [SerializeField] CharacterMover playerMover;

    // Start Game Panel to disable it when start button is clicked
    [SerializeField] GameObject StartGamePanel;

    // GameOver panel to enable when game over happened
    [SerializeField] GameObject GameOverPanel;

    // Continue Panel. If player has life remaining then this panel will be displayed
    [SerializeField] GameObject ContinuePanel;

    // Total remaining life on header text
    [SerializeField] TextMeshProUGUI lifeTextOnHeader;
    
    // RestartGameText to show how much life remaining and restart game text
    [SerializeField] TextMeshProUGUI RestartGameText;
    
    [Header("update distance and time text")]
    [SerializeField] TextMeshProUGUI[] distanceTravelled;
    [SerializeField] TextMeshProUGUI[] timetakenText;
    [Header("Coin Taken Text")]
    [SerializeField] TextMeshProUGUI coinTakenText;
    bool IsGameOver = false;

    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        StartGamePanel.SetActive(true);
        FindObjectOfType<PlayerDataManager>().PlayerStateAction   +=  PlayeGameState;
    }

    // When player life decreases by one when player hits any obstacle.  then update the life text
    public void UpdateLifeText(int remainingLife)
    {   
        lifeTextOnHeader.text= "Life :" +remainingLife.ToString();
        
        RestartGameText.text = remainingLife +" life remaining " + ", \n Restart Game";
        ContinuePanel.SetActive(true);
    }

    // This function is called from UI button Start
    public void StartGame()
    {
        playerMover.ChangePlayerState(PlayerStates.Run);
        StartGamePanel.SetActive(false);
    }

    // This function is called from Continue button when player loses one life
    public void ContinueGame()
    {
        if(IsGameOver)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else
         {
             playerMover.ChangePlayerState(PlayerStates.Run);
             ContinuePanel.SetActive(false);
         }
    }

    // This function is called from player health manager Game state actoin. 
    // used when player has game over
    void PlayeGameState(PlayerStates state)
    {
        if(state == PlayerStates.GameOver)
            {
                 IsGameOver = true;
                GameOverPanel.SetActive(true);
                RestartGameText.text = "0 life remaining";
                lifeTextOnHeader.text= "Life : 0";
            }
    }


    // This function is called from CharacterMover script to update current distance and time taken text
    public void UpdateScores(float timeTaken, float distance)
    {
        // Updating distance travelled text on ui
        foreach(var item in distanceTravelled)
            if(item !=null)
                item.text = "Distance Travelled :" + distance.ToString("0") + " Meters.";;
        

        // updating time taken text on ui
        foreach(var item in timetakenText)
            if(item != null)
                item.text = "Time Taken :"+ timeTaken.ToString("0.0") +" seconds.";

    }

    // This function is called from PlayerHealthManager.cs class
    public void UpdateCoinTakenText(int coins)
    {
        coinTakenText.text ="Coins : " + coins.ToString();
    }
}
