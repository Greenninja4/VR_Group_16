using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fish : MonoBehaviour {

 public float RotationSpeed;

	void Start () {
        RotationSpeed = -6.0f;
	}
	
	void Update () {
        transform.Rotate ( Vector3.up * ( RotationSpeed * Time.deltaTime ) );
		
	}
}