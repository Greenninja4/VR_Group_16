using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Eagle : MonoBehaviour {

 public float RotationSpeed;

	void Start () {
        RotationSpeed = -6.0f;
	}
	
	void Update () {
        // Rotate bird over time
        transform.Rotate ( Vector3.up * ( RotationSpeed * Time.deltaTime ) );
		
	}
}