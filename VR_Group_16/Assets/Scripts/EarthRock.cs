using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRock : MonoBehaviour {

    public bool rock_active;
    public bool rock_visible;
    public int hand; // 0 = none, 1 = Left, 2 = Right
    public GameObject player;
    float timer;
    float thrust;
    Vector3 l_hand_vec;
    Vector3 l_hand_vec_norm;
    Vector3 vec_to_pos;
    Vector3 expected_pos;

	// Use this for initialization
	void Start () {
		// Disable
        rock_active = false;
        rock_visible = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = false;

        //Default hand to none
        hand = 0;
        timer = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        // If chosen and not enabled
        if (rock_active){
            l_hand_vec = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
            l_hand_vec_norm = l_hand_vec.normalized;

            // Enable
            if(!rock_visible){
                this.GetComponent<MeshRenderer>().enabled = true;
                this.GetComponent<Rigidbody>().isKinematic = true;
                rock_visible = true;
                timer = 5.0f;

                // Set start position at 5 m from active hand along ray
                transform.position = player.transform.position + 6 * l_hand_vec_norm;

            }
            
            // If trigger held
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.5){

                expected_pos = player.transform.position + 2*l_hand_vec_norm;

                // Come a fraction closer to expected position each update
                vec_to_pos = expected_pos - transform.position;
                transform.position += vec_to_pos/5;            
            }

            //Else, launch rock
            else{
                rock_active = false;
                // Force of launch in direction of hand
                // Force inversely proportional to distance to hand
                thrust = 10/Vector3.Distance(player.transform.position + l_hand_vec, transform.position);
                this.GetComponent<Rigidbody>().AddForce(l_hand_vec_norm*thrust);
            }
            
        }

        // Else rock is disabled
        else{
            
            if(timer > 0.0f){
                // Rock experiences gravity
                this.GetComponent<Rigidbody>().isKinematic = false;
                this.GetComponent<Rigidbody>().useGravity = true;

                timer -= Time.deltaTime;
            }
            else{
                // Rock fully disabled
                rock_active = false;
                this.GetComponent<MeshRenderer>().enabled = false;
                this.GetComponent<Rigidbody>().isKinematic = false;
                
            }
            

        }
            

            
        
            
                
            
        
        
	}

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.5){
                Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
            }
        }
    }
}
