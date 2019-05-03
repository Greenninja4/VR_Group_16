using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AchievementsMenu : MonoBehaviour {

    // Status variable & menu object
    public GameObject menu;

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
        laserLine.material.color = Color.black;
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
            Debug.Log(selectedItem.name);
            if(selectedItem != null){
                menu.GetComponent<Achievements>().clickHandle(selectedItem);
                clicked = true;
            }
        } 
        // Trigger released -> allow for another click
        if (clicked && OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) < triggerThresholdLower) {
            clicked = false;
        }
	}
}
