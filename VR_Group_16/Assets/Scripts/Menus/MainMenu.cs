using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour {

    // Status variable & menu object
    public GameObject menu;
    public GameObject battleMenu;
    public GameObject exploreMenu;
    // Ray cast controller
    private float triggerThreshold = 0.5f;
    private float triggerThresholdLower = 0.1f;
    private bool clicked = false;
    public OVRInput.Controller controller;
    public Transform laserEnd, laserBegin;
    private LineRenderer laserLine;
    public Vector3 hitpoint;
    public GameObject selectedItem;
    private float range = 100f;
    

	// Use this for initialization
	void Start () {
        // Ray cast initialization
        laserLine = GetComponent<LineRenderer>();
        // Deactivate battle and explore menus
        battleMenu.SetActive(false);
        exploreMenu.SetActive(false);
	}

    // Handles the click from a raycast
    void clickHandle(GameObject selectedItem){
        // Main menu buttons (tutorial, battle, explore, quit)
        if(selectedItem.name == "Tutorial"){
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName:"Tutorial");
        }
        if(selectedItem.name == "Battle"){
            menu.SetActive(false);
            battleMenu.SetActive(true);
        }
        if(selectedItem.name == "Explore"){
            menu.SetActive(false);
            exploreMenu.SetActive(true);
        }
        if(selectedItem.name == "Quit"){
            #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
        // Battle -> arena selection buttons (earth, water)
        if(selectedItem.name == "BattleEarth"){
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName:"BattleEarth");
        }
        if(selectedItem.name == "BattleWater"){
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName:"BattleWater");
        }
        // Explore -> arena selection buttons (earth, water)
        if(selectedItem.name == "ExploreEarth"){
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName:"ExploreEarth");
        }
        if(selectedItem.name == "ExploreWater"){
            UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName:"ExploreWater");
        }
        // Battle & Explore back buttons
        if(selectedItem.name == "Back"){
            menu.SetActive(true);
            exploreMenu.SetActive(false);
            battleMenu.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {
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
        if (!clicked && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > triggerThreshold) {
            if(selectedItem != null){
                clickHandle(selectedItem);
                clicked = true;
            }
        } 
        // Trigger released -> allow for another click
        if (clicked && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) < triggerThresholdLower) {
            clicked = false;
        }
	}
}
