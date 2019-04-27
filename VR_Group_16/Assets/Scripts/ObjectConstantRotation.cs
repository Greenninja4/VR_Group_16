using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectConstantRotation : MonoBehaviour {

    public int rotationFactor;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(0, rotationFactor, 0);
	}
}
