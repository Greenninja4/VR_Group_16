using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class roomswitch : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		// Move to MP1.1.2
		if(Input.GetKeyDown("2")){
			transform.position = new Vector3(50,1,0);
		}
		// Move to MP1.1.1
		if(Input.GetKeyDown("1")){
			transform.position = new Vector3(0,0,0);
		}
		// Quit the game
		if(Input.GetKeyDown(KeyCode.Escape)){
			# if UNITY_EDITOR
				UnityEditor.EditorApplication.isPlaying = false;
			# else
				Application.Quit();
			# endif
		}
	}
}
