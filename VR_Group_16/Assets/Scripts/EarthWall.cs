using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : MonoBehaviour {

    Vector3 vec_3m_height;
    Vector3 avg_hands;
    Vector3 wall_pos_min;
    Vector3 head_to_control;
    public GameObject player;
    float hand_init_height;
    float disp;
    float disp_max;
    float y_rot;
    bool wall_active;
    public Transform groundPrefab;

	// Use this for initialization
	void Start () {
        wall_active = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        vec_3m_height = new Vector3(0,4,0);
        disp = 0.0f;
        disp_max = 0.8f;

        Transform ground = Instantiate(groundPrefab) as Transform;
        Physics.IgnoreCollision(ground.GetComponent<Collider>(), GetComponent<Collider>());
	}
	
	//Update is called once per frame
	void Update () {
        // If both controller triggers activate
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.5 && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.5){

            // Initial hand position
            avg_hands = (OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch) 
                        + OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch))/2;
            
            // If wall not previously active, enable
            if (!wall_active){
                wall_active = true;
                // Enable wall
                this.GetComponent<MeshRenderer>().enabled = true;

                // Wall position goes to below ground, 3 m in front of average head-to-controller direction
                hand_init_height = avg_hands.y;
                head_to_control = avg_hands - player.transform.position;
                head_to_control.y = 0.0f;
                head_to_control = head_to_control.normalized;
                wall_pos_min = player.transform.position + 3*(head_to_control);
                wall_pos_min.y = -2.0f;
                transform.position = wall_pos_min;

                // Wall rotation is equal to head_to_control atan(x/z)
                //y_rot = Mathf.Atan(head_to_control.x/head_to_control.z);
                //transform.rotation = Quaternion.Euler(0,y_rot,0);
                transform.rotation = Quaternion.identity;
            }
            transform.rotation = Quaternion.identity;

            // Calculate average vertical displacement from recorded position
            disp = avg_hands.y - hand_init_height;

            // If displacement is above max amount
            if (disp > disp_max){
                // Wall reaches max height
                transform.position = wall_pos_min + vec_3m_height;
            }
                
            // Else if displacement is positive
            else if (disp > 0.0f){
                // Wall moves up amount of displacement
                transform.position = wall_pos_min + (disp/disp_max)*vec_3m_height;
            }

            // Else
            else{
                // Wall stays at min position
                transform.position = wall_pos_min;
            }
        }

        // Else
        else{
            // Wall experiences gravity

            // If wall position is wall_pos_min, set to null and disable
            if (transform.position.y <= wall_pos_min.y){
                wall_active = false;
                transform.position = wall_pos_min;
                this.GetComponent<MeshRenderer>().enabled = false;
            }

        }
                
        
            
    }
}

