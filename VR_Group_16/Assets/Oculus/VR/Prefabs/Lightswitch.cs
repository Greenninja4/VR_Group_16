using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightswitch : MonoBehaviour {
	public Light lighting;
	private int currentColor;

	// Use this for initialization
	void Start () {
		lighting = GetComponent<Light>();
		currentColor = 0;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown("tab")){
			currentColor = (currentColor + 1) % 6;
			switch(currentColor){
				case 0: 
					lighting.color = Color.white;
					break;
				case 1: 
					lighting.color = Color.red;
					break;
				case 2: 
					lighting.color = Color.green;
					break;
				case 3: 
					lighting.color = Color.blue;
					break;
				case 4: 
					lighting.color = Color.cyan;
					break;
				case 5: 
					lighting.color = Color.magenta;
					break;
				case 6: 
					lighting.color = Color.yellow;
					break;
			}
		}
	}
}
