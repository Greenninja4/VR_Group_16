using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    // Status variable & menu object
    private bool paused;
    public GameObject pauseMenu;
    // Ray cast controller
    private float triggerThreshold = 0.5f;
    public OVRInput.Controller controller;
    public Transform laserEnd, laserBegin;
    private LineRenderer laserLine;
    public Vector3 hitpoint;
    public GameObject selectedItem;
    private float range = 100f;
    

	// Use this for initialization
	void Start () {
        // Initialize pause flag and disable pause menu
        paused = false;
		// pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.SetActive(false);
        // Ray cast initialization
        laserLine = GetComponent<LineRenderer>();
	}

    public void PauseGame(){
        paused = true;
        Time.timeScale = 0.00001f;
        pauseMenu.SetActive(true);
    }

    public void ResumeGame(){
        paused = false;
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }

    // Handles the click from a raycast
    void clickHandle(GameObject selectedItem){
        print (selectedItem.name);
        // Resume button
        if(selectedItem.name == "Resume"){
            ResumeGame();
        }
        // Main Menu button
        if(selectedItem.name == "MainMenu"){
            ResumeGame();
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName:"Startmenu");
        }
        // Achievements button
        if(selectedItem.name == "Achievements"){
            ResumeGame();
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName:"Achievements");
        }
        // Quit button
        if(selectedItem.name == "Quit"){
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
	
	// Update is called once per frame
	void Update () {
        // Start button pauses game
        if (!paused){
            if (OVRInput.GetDown(OVRInput.Button.Start)){
                PauseGame();
            }
        }
        // If paused
        else {
            // Start button unpauses game
            if (OVRInput.GetDown(OVRInput.Button.Start)){
                ResumeGame();
            }
            // Ray cast finds ray casted object
            RaycastHit hit;
            Ray laserRay = new Ray(laserBegin.position, laserBegin.transform.forward);
            laserLine.SetPosition(0, laserBegin.position);
            Vector3 rayDirection = laserEnd.position - laserBegin.position;
            if (Physics.Raycast(laserRay, out hit, 100)){
                laserLine.SetPosition(1, hit.point);
                hitpoint = hit.point;
                selectedItem = hit.collider.gameObject;
            }
            else {
                laserLine.SetPosition(1, laserBegin.position + (rayDirection * range));
                selectedItem = null;
            }
            // Trigger clicked -> raycast to handle chosen object
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > triggerThreshold) {
                if(selectedItem != null){
                    clickHandle(selectedItem);
                }
            }
        }
	}
}
