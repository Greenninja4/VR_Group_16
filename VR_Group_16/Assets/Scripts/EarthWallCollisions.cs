using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWallCollisions : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    // On collision w/ ground, ignore
    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
        else if(collision.gameObject.tag == "Rock"){
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag == "Fireball"){
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag == "Waterball"){
            Destroy(collision.gameObject);
        }
        else if(collision.gameObject.tag == "Airball"){
            Destroy(collision.gameObject);
        }
    }

}
