using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRockHandler : MonoBehaviour {

    public EarthRock EarthRock1;
    //public EarthRock EarthRock2;
    Vector3 l_hand_vec;

	// Use this for initialization
	void Start () {
		// Set 2 rocks to disabled
        EarthRock1.rock_active = false;
        //EarthRock2.rock_active = false;
	}
	
	// Update is called once per frame
	void Update () {
		// If left controller trigger active
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.5){
            // If left controller pointing to existing rock
                // That rock is enabled for left
            // Else if left controller pointing to ground
            l_hand_vec = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            if (l_hand_vec.y < 0.0f){
                // Non-existent rock is enabled for left
                EarthRock1.rock_active = true;

                // Hand to 1 = left (0 for none, 2 for right)
                EarthRock1.hand = 1;
            }
                
        }
            

        // If right controller trigger active
            // If right controller pointing to existing rock
                // That rock is enabled for right
            // Else if right controller pointing to ground
                // Non-existent rock is enabled for right
            

            
	}
}
