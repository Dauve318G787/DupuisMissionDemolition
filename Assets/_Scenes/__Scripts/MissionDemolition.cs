using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;  // Import this for scene management

public enum GameMode {
    idle,
    playing,
    levelEnd
}

public class MissionDemolition : MonoBehaviour {
    static private MissionDemolition S;

    [Header("Inscribed")]
    public Text uitLevel;
    public Text uitShots;
    public Vector3 castlePos;
    public GameObject[] castles;

    [Header("Dynamic")]
    public int level;
    public int levelMax;
    public int shotsTaken;
    public GameObject castle;
    public GameMode mode = GameMode.idle;
    public string showing = "Show Slingshot";


    void Start() {
        S = this;

        level = 0;
        shotsTaken = 0;
        levelMax = castles.Length;

        StartLevel();
    }

    void StartLevel() {
        if (castle != null) {
            Destroy (castle);
        }

        Projectile.DESTROY_PROJECTILES();

        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;

        Goal.goalMet = false;

        UpdateGUI();

        mode = GameMode.playing;

        FollowCam.SWITCH_VIEW( FollowCam.eView.both );
    }

    void UpdateGUI() {
        uitLevel.text = "Level: " +(level+1)+" of "+levelMax;
        uitShots.text = "Shots Taken: "+shotsTaken;
    }

    void Update() {
        UpdateGUI();

        if (shotsTaken > 10) {
            // Switch to Game Over scene when shotsTaken exceeds 10
            SceneManager.LoadScene("GameOver");
            return;  // Ensure the rest of the code doesn't run after the scene switch
        }

        if ((mode == GameMode.playing) && Goal.goalMet ) {
            mode = GameMode.levelEnd;

            FollowCam.SWITCH_VIEW( FollowCam.eView.both );
            
            Invoke("NextLevel", 2f);
        }
    }

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
        SceneManager.LoadScene("Scene_0");
    }
}


    static public void SHOT_FIRED() {
        S.shotsTaken++;

        if (S.shotsTaken > 10) {
            // Switch to Game Over scene when shotsTaken exceeds 10
            SceneManager.LoadScene("GameOver");
        }
    }

    static public GameObject GET_CASTLE() {
        return S.castle;
    }
}
