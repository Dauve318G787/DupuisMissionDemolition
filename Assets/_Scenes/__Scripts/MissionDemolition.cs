// MODULE PURPOSE:
// This is the primary script for the Mission Demolition project.
// It orchestrates everything that is seen on the main game screen.

// Boilerplate Unity includes with a few notable additions
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Import this for UI management
using UnityEngine.SceneManagement;  // Import this for scene management

public enum GameMode {
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour {
    static private MissionDemolition S; // another private Singleton

    [Header("Inscribed")]
    public Text uitLevel; // UIText_Level object
    public Text uitShots; // UIText_Shots object
    public Vector3 castlePos; // Where castles will be placed in game
    public GameObject[] castles; // Array to hold all castles

    [Header("Dynamic")]
    public int level; // The player's current level
    public int levelMax; // The total number of levels
    public int shotsTaken; // How many shots have been taken by the player
    public GameObject castle; // The current castle
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot"; // FollowCam mode


    void Start() {
        S = this; // Definition for Singleton

        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;

        StartLevel();
    }

    void StartLevel() {

        // Get rid of the old castle if one exists
        if (castle != null) {
            Destroy (castle);
        }

        // Destroy old projectiles if they exist
        Projectile.DESTROY_PROJECTILES();

        // Instantiate the new castle
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        // Reset goalMet flag
        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;

        FollowCam.SWITCH_VIEW( FollowCam.eView.both );
    }

    void UpdateGUI() {
        
        // Show the data in GUITexts
        uitLevel.text = "Level: " +(level+1)+" of "+levelMax;
        uitShots.text = "Shots Taken: "+shotsTaken;
    }

    void Update() {
        
        UpdateGUI();

        // GameOver scene switch logic
        if (shotsTaken > 30) {
            // Switch to Game Over scene when shotsTaken exceeds 10
            SceneManager.LoadScene("GameOver");
            return;  // Ensure the rest of the code doesn't run after the scene switch
        }

        // Check for level end
        if ((mode == GameMode.playing) && Goal.goalMet ) {
            mode = GameMode.levelEnd;

            FollowCam.SWITCH_VIEW( FollowCam.eView.both );
            
            Invoke("NextLevel", 2f);
        }
    }

    // Brings player to next level
    void NextLevel() {
    level++;
    if (level == levelMax) {
        level = 0;
        shotsTaken = 0;
    }

    // Check if the player completed the 4th level with fewer than 25 shots
    if (level == 4 && shotsTaken < 25) {
        SceneManager.LoadScene("GoodEnding");
    } else {
        StartLevel();
    }
}


    // Allows any code to increment shotsTaken, and ensures it does not go over 10
    static public void SHOT_FIRED() {
        S.shotsTaken++;

        if (S.shotsTaken > 30) {
            // Switch to Game Over scene when shotsTaken exceeds 10
            SceneManager.LoadScene("GameOver");
        }
    }

    // Allows any code to get a reference to S.castle
    static public GameObject GET_CASTLE() {
        return S.castle;
    }
}
