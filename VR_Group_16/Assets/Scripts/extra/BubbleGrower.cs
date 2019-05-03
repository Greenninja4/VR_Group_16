using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleGrower : MonoBehaviour {

	private float growFactor = 0.3f;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		float time_diff = Time.deltaTime;
		transform.localScale = (1 + time_diff * growFactor) * transform.localScale;
	}
}
