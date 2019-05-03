using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour {

    // Status variable & menu object
    private bool paused;
    public GameObject player, centerEye;
    public GameObject pauseMenu;
    // Ray cast controller
    private float triggerThreshold = 0.5f;
    public OVRInput.Controller Rcontroller, Lcontroller;
    private GameObject laserEndR, laserEndL, laserBeginR, laserBeginL;
    private LineRenderer laserLineR, laserLineL;
    public Vector3 hitpointR,hitpointL;
    public GameObject selectedItemL, selectedItemR;
    private GameObject trackingSpace;
    private float range = 100f;
    

	// Use this for initialization
	void Start () {
        // Initialize pause flag and disable pause menu
        paused = false;
		// pauseMenu = GameObject.FindGameObjectWithTag("PauseMenu");
        pauseMenu.SetActive(false);
        // Ray cast initialization from player

        trackingSpace = player.transform.Find("OVRCameraRig").gameObject.transform.Find("TrackingSpace").gameObject;
        laserLineL = trackingSpace.transform.Find("LeftHandAnchor").gameObject.GetComponent<LineRenderer>();
        laserLineR = trackingSpace.transform.Find("RightHandAnchor").gameObject.GetComponent<LineRenderer>();
        laserEndL = trackingSpace.transform.Find("LeftHandAnchor").gameObject.transform.Find("LaserEnd").gameObject;
        laserEndR = trackingSpace.transform.Find("RightHandAnchor").gameObject.transform.Find("LaserEnd").gameObject;
        laserBeginL = trackingSpace.transform.Find("LeftHandAnchor").gameObject.transform.Find("LaserBegin").gameObject;
        laserBeginR = trackingSpace.transform.Find("RightHandAnchor").gameObject.transform.Find("LaserBegin").gameObject;
        centerEye = trackingSpace.transform.Find("CenterEyeAnchor").gameObject;

    }
        

    public void PauseGame(){
        paused = true;
        Time.timeScale = 0.00001f;
        pauseMenu.SetActive(true);

        // Place pause Menu in front of user
        pauseMenu.transform.position = centerEye.transform.position + centerEye.transform.rotation*(new Vector3(0,0,3f)
        );
        pauseMenu.transform.rotation = centerEye.transform.rotation;
    }

    public void ResumeGame(){
        paused = false;
        Time.timeScale = 1.0f;
        pauseMenu.SetActive(false);
    }

    // Handles the click from a raycast
    void clickHandle(GameObject selectedItem){
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
            // Ray cast finds ray casted object (Right hand)
            RaycastHit hit;
            Ray laserRayR = new Ray(laserBeginR.transform.position, laserBeginR.transform.forward);
            laserLineR.SetPosition(0, laserBeginR.transform.position);
            Vector3 rayDirectionR = laserEndR.transform.position - laserBeginR.transform.position;
            if (Physics.Raycast(laserRayR, out hit, 100)){
                laserLineR.SetPosition(1, hit.point);
                hitpointR = hit.point;
                selectedItemR = hit.collider.gameObject;
            }
            else {
                laserLineR.SetPosition(1, laserBeginR.transform.position + (rayDirectionR * range));
                selectedItemR = null;
            }

            //Repeat for Left Hand
            Ray laserRayL = new Ray(laserBeginL.transform.position, laserBeginL.transform.forward);
            laserLineL.SetPosition(0, laserBeginL.transform.position);
            Vector3 rayDirectionL = laserEndL.transform.position - laserBeginL.transform.position;
            if (Physics.Raycast(laserRayL, out hit, 100)){
                laserLineL.SetPosition(1, hit.point);
                hitpointL = hit.point;
                selectedItemL = hit.collider.gameObject;
            }
            else {
                laserLineL.SetPosition(1, laserBeginL.transform.position + (rayDirectionL * range));
                selectedItemL = null;
            }

            // Trigger clicked -> raycast to handle chosen object (right hand)
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Rcontroller) > triggerThreshold) {
                if(selectedItemR != null){
                    clickHandle(selectedItemR);
                }
            }

            // Repeat for Left Hand
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, Lcontroller) > triggerThreshold) {
                if(selectedItemL != null){
                    clickHandle(selectedItemL);
                }
            }
        }
	}
}
