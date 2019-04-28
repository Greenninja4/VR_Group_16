using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectCollisions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // On collision with certain element, destroy element
    void OnCollisionEnter(Collision collision)
    {
        if(this.tag == "Rock"){
            if (collision.gameObject.tag == "Airball")
                Destroy(collision.gameObject);
        }

        else if(this.tag == "Airball"){
            if (collision.gameObject.tag == "Waterball")
                Destroy(collision.gameObject);
        }

        else if(this.tag == "Waterball"){
            if (collision.gameObject.tag == "Fireball")
                Destroy(collision.gameObject);
        }

        else if(this.tag == "Fireball"){
            if (collision.gameObject.tag == "Rock")
                Destroy(collision.gameObject);
        }
    }
}
