using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartMenuGuy : MonoBehaviour {

    // Game objects
    public GameObject guy;
    public GameObject player;
    public float triggerDist;

    // Text & Pics
    public GameObject messageCanvas;
    public GameObject messageCanvas1;
    public GameObject messageCanvas2;

    public GameObject picture1;
    public GameObject picture2;
    public GameObject picture3;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float dist = Vector3.Magnitude(guy.transform.position - player.transform.position);
        if (dist < triggerDist){
            guy.transform.LookAt(player.transform);
            messageCanvas.SetActive(true);
            messageCanvas1.SetActive(true);
            messageCanvas2.SetActive(true);
            picture1.SetActive(true);
            picture2.SetActive(true);
            picture3.SetActive(true);

        }
	}
}
