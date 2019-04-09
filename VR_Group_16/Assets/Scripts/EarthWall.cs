using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : MonoBehaviour {

	// Use this for initialization
	void Start () {
        transform.position = new Vector3(0,-1,2);
        this.GetComponent<MeshRenderer>().enabled = false;
	}
	
	//Update is called once per frame
	void Update () {
        // If both controller triggers activate
            // Wall position goes to below ground, 3 m in front of average controller direction
            // Record average current position of controllers
            // Calculate average vertical displacement from recorded position
            // If displacement is positive
                // If displacement is above max amount
                    // Wall reaches max height
                // Else
                    // Wall moves up the amount of displacement
            // Else
                // Wall stays at min position
        // Else
            // Wall experiences gravity
    }
}

