using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LavaBubbles : MonoBehaviour {

	public GameObject[] positions;
	public GameObject bubble;
	private float lastInstantiation = 0;
	private float bubblePeriod = 0.05f;
	private float bubbleKillTime = 3.0f;
	private float offsets = 15.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (lastInstantiation + bubblePeriod < Time.time){
			lastInstantiation = Time.time;
			LavaBubble();
		}
	}

	// Makes a lava bubble
	void LavaBubble(){
		float xOffset = Random.Range(0, offsets);
		float zOffset = Random.Range(0, offsets);
		Vector3 posOffset = new Vector3(xOffset, 0, zOffset);
		int index = (int)Random.Range(0, positions.GetLength(0)+1);
		Vector3 pos = positions[index].transform.position + posOffset;
		GameObject myBubble = Instantiate(bubble, pos, Quaternion.identity);
		Destroy(myBubble, bubbleKillTime);
		myBubble = null;
	}
}
