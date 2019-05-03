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
            ControlsTut();
        }

        else if(stage == 2){
            healthStaminaTut();
        }

        else if(stage == 3){
            staminaTut();
        }

        else if(stage == 4){
            CycleTut();
        }

        else if(stage == 5){
            ThrowTut ();
        }

        else if(stage == 6){
            EnemiesTut ();
        }

        else if(stage == 7){
            RockwallTut ();
        }

        else if(stage == 8){
            FinalThoughts();
        }

        else{
            EndTut();
        }

    }


    public void IntroText () {
        guideWordsText.text = "Press the start button on your left controller to pause. Click continue for tutorial mode.";
    }

    public void ControlsTut(){
        guideWordsText.text = "Welcome to Elemental Experience: a game that allows you to control Earth, Water, Fire, and Air.";
    }

    public void healthStaminaTut(){
        guideWordsText.text = "To your right are health and stamina bars. Enemy projectiles that hit you will lower your health.";
    }

    public void staminaTut(){
        guideWordsText.text = "The stamina is used when you control an element. Unlike health, stamina replenishes.";
    }

    public void CycleTut(){
        guideWordsText.text = "Press A to cycle through the elements. To your left, point and pull the trigger to grab a projectile. Let go to release it.";

        // Display all 4 prefabs
        if(!objectsDisplayed){
            GameObject rock = Instantiate(projectiles[0], new Vector3(-6f,0.5f,-8f), Quaternion.identity);
            GameObject waterball = Instantiate(projectiles[1], new Vector3(-6f,0.5f,-10f), Quaternion.identity);
            GameObject airball = Instantiate(projectiles[2], new Vector3(-6f,0.5f,-12f), Quaternion.identity);
            GameObject fireball = Instantiate(projectiles[3], new Vector3(-6f,0.5f,-14f), Quaternion.identity);
            objectsDisplayed = true;
        }

    }

    public void ThrowTut (){
        guideWordsText.text = "You can grab rocks from the ground, waterballs from the pond, and fire / airballs from the air in the same way.";
    }

    public void EnemiesTut (){
        // Explain bending objects
        guideWordsText.text = "Enemy projectiles will attack you. Counter pairs are Earth vs. Air and Water vs. Fire. Try beating the incoming enemies.";

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
        guideWordsText.text = "You can defend yourself with rockwalls. With both controllers pointed down on Earth, hold the side triggers and raise your hands.";

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
        guideWordsText.text = "Now go master the elements in Battle Mode, Explore Mode, or restart this tutorial by pressing continue.";

    }

    public void EndTut (){
        // Take user to tutorial menu
        stage = -1;
    }
}
