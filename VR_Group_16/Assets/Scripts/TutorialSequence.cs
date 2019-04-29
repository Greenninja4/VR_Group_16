using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TutorialSequence : MonoBehaviour {

    // Initialize public elements
    public GameObject player;
    public OVRInput.Controller Lcontroller;
    public OVRInput.Controller Rcontroller;
    public GameObject statusBars;
    public GameObject[] enemies;
    public Transform[] spawnPoints;
    public GameObject selectedItemL;
    public GameObject selectedItemR;
    public Text guideWordsText;
    public GameObject[] projectiles;
    public float maxHealth;
    public float currentHealth;
    public GameObject leftHandAnchor;
    public GameObject rightHandAnchor;
    

    // Initialize private elements
    private bool paused;
    private bool tutorialStarted;
    private bool continueSelectedL;
    private bool objectsDisplayed;
    private bool continueSelectedR = false;
    private float trigger_thresh = 0.5f;
    private float nextFire;
    private float fireRate = 5.0f;

    // Initialize public variables
    public int stage = -1;

	// Use this for initialization
	void Start () {

        // Initialize bools
        continueSelectedL = false;
        continueSelectedR = false;
        objectsDisplayed = false;

        // Initialize variables
        maxHealth = 1000000000f;
        currentHealth = maxHealth;
        nextFire = 0.0f;
		
	}
	
	// Update is called once per frame
	void Update () {
        selectedItemL = leftHandAnchor.GetComponent<BallShooting>().selectedItem;
        selectedItemR = rightHandAnchor.GetComponent<BallShooting>().selectedItem;

        if(selectedItemL != null){
            // If left selects continue, move to next stage
            if((selectedItemL.name == "ContinueCube")&&(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Lcontroller) > trigger_thresh)){
                if(!continueSelectedL){
                    continueSelectedL = true;
                    stage++;
                }
            }

            // If left trigger inactivated, change bool to false
            else if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Lcontroller) <= trigger_thresh){
            continueSelectedL = false;
            }
        }
        

        

        if(selectedItemR != null){
            // If right selects continue, move to next stage
            if((selectedItemR.name == "ContinueCube")&&(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Rcontroller) > trigger_thresh)){
                if(!continueSelectedR){
                    continueSelectedR = true;
                    stage++;
                }
            }
            // If right trigger inactivated, change bool to false
            else if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Rcontroller) <= trigger_thresh){
                continueSelectedR = false;
            }
        }


        // Cycle through stages
        if (stage == -1){
            guideWordsText.text = "Welcome traveler! Use raycasting with either controller and select continue.";
        }

        if (stage == 0){
            IntroText();
        }

        else if (stage == 1){
            healthStaminaTut();
        }

        else if(stage == 2){
            ThrowTut();
        }

        else if(stage == 3){
            EnemiesTut();
        }

        else if(stage == 4){
            RockwallTut();
        }

        else if(stage == 5){
            FinalThoughts();
        }

        else{
            EndTut();
        }

    }


    public void IntroText () {
        // Display text that introduces the game
        guideWordsText.text = "Welcome to Elemental Experience, a 1-player game that allows you to control the elements of earth, water, fire, and air. In this tutorial, press the start button on your left controller at any time to pause. Use raycast to select continue.";
    }

    public void healthStaminaTut(){
        guideWordsText.text = "To the right of me are your health and stamina bars. Enemy projectiles that hit you will lower your health, which does not replenish. The stamina is used when you bend an element, but replenishes slowly over time.";
    }

    public void ThrowTut (){

        // Explain bending objects
        guideWordsText.text = "To my left are your basic elemental projectiles. The raycast color on each controller dictates which elements you can bend with each hand (yellow = Earth, blue = Water, green = Air, red = Fire). Press A to cycle through elements on each hand, and try using raycasting and index trigger select to pull these projectiles to you (let go to launch). In addition, you can instantiate rocks from the ground, waterballs from the pond, and fire and airballs from the air in the same way.";

        // Display all 4 prefabs
        if(!objectsDisplayed){
            GameObject rock = Instantiate(projectiles[0], new Vector3(-6f,0.5f,-8f), Quaternion.identity);
            GameObject waterball = Instantiate(projectiles[1], new Vector3(-6f,0.5f,-10f), Quaternion.identity);
            GameObject airball = Instantiate(projectiles[2], new Vector3(-6f,0.5f,-12f), Quaternion.identity);
            GameObject fireball = Instantiate(projectiles[3], new Vector3(-6f,0.5f,-14f), Quaternion.identity);
            objectsDisplayed = true;
        }

    }

    public void EnemiesTut (){
        // Explain bending objects
        guideWordsText.text = "Enemy projectiles will randomly appear and hurtle towards you. The only way to stop them is to hit their element with a superior element. In this game, the paper-rock-scissors order is Earth -> Air -> Water -> Fire -> Earth, so Earth beats Air, etc. Try beating these enemies!";

        // Show user example of enemy hitting you
        if(Time.time > nextFire){
            nextFire = Time.time + fireRate;

            // Spawn new enemy
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            int enemyIndex = Random.Range(0, enemies.Length);
            Instantiate(enemies[enemyIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        }
    }

    public void RockwallTut (){
        // Tell user how to build rock wall
        guideWordsText.text = "There is a special earthbending move called rock wall, which lets you protect yourself from enemies. With both controllers on the earth element, hold both side hand triggers and raise your hands to raise a wall. Release the triggers to set the wall in place.";

        // Show user example of enemy hitting you
        if(Time.time > nextFire){
            nextFire = Time.time + fireRate;

            // Spawn new enemy
            int spawnPointIndex = Random.Range(0, spawnPoints.Length);
            int enemyIndex = Random.Range(0, enemies.Length);
            Instantiate(enemies[enemyIndex], spawnPoints[spawnPointIndex].position, spawnPoints[spawnPointIndex].rotation);
        }

    }

    public void FinalThoughts(){
        // Finishing thoughts for user
        guideWordsText.text = "That's it! In the main start menu, you can choose to enter battle mode if you think you are ready, or go to the world explorer to check out the different arenas. Thanks for playing, and have fun! Press continue to restart tutorial.";

    }

    public void EndTut (){
        // Take user to tutorial menu
        stage = -1;
    }
}
