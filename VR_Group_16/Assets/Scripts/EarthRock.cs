using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRock : MonoBehaviour {

    public bool rock_active;
    public int hand; // 0 = none, 1 = Left, 2 = Right

	// Use this for initialization
	void Start () {
		// Disable
        rock_active = false;
        rock_visible = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = false;

        //Default hand to none
        hand = 0;
	}
	
	// Update is called once per frame
	void Update () {

        // If chosen and not enabled
        if (rock_active){

            // Enable
            if(!rock_visible){
                this.GetComponent<MeshRenderer>().enabled = true;
                this.GetComponent<Rigidbody>().isKinematic = true;
                rock_visible = true;
            }
            

        }

		
            
            // If active left
                // Set active hand to left
            // Else
                // Set active hand to right
            // Set start position at 5 m from active hand along ray
        
        // If enabled
            // If distance to active hand > 0.5 m
                // Bring rock 0.1 m closer 0.5 m distant point from hand
            // Else
                // Rock is stationary 0.5 m away
        
        
	}
}
