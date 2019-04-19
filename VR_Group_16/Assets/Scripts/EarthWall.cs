using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthWall : MonoBehaviour
{

    Vector3 vec_3m_height;
    Vector3 avg_hands;
    Vector3 avg_hands_norm;
    Vector3 wall_pos_min;
    Vector3 wall_cur_pos;
    Quaternion wall_cur_rot;
    public GameObject player;
    float hand_init_height;
    float disp;
    float disp_max;
    float y_rot;
    float timer;
    bool wall_active;


    // Use this for initialization
    void Start()
    {
        wall_active = false;
        this.GetComponent<MeshRenderer>().enabled = false;
        this.GetComponent<Rigidbody>().isKinematic = false;
        vec_3m_height = new Vector3(0, 3, 0);
        disp = 0.0f;
        disp_max = 0.8f;
        timer = 0.0f;

    }

    //Update is called once per frame
    void Update()
    {
        // If both controller triggers activate
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.5 && OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.5)
        {

            // Initial hand position
            avg_hands = (OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch)
                        + OVRInput.GetLocalControllerPosition(OVRInput.Controller.RTouch)) / 2;

            // If wall not previously active, enable
            if (!wall_active)
            {
                wall_active = true;
                // Enable wall
                this.GetComponent<MeshRenderer>().enabled = true;
                this.GetComponent<Rigidbody>().isKinematic = true;

                // Wall position goes to below ground, 3 m in front of average head-to-controller direction
                hand_init_height = avg_hands.y;
                avg_hands_norm = avg_hands;
                avg_hands_norm.y = 0.0f;
                avg_hands_norm = avg_hands_norm.normalized;
                wall_pos_min = player.transform.position + 4 * (avg_hands_norm);
                wall_pos_min.y = -1.5f;
                transform.position = wall_pos_min;

                // Wall rotation is equal to head_to_control atan(x/z)
                y_rot = Mathf.Atan2(avg_hands.x,avg_hands.z) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0,y_rot,0);
                //transform.rotation = Quaternion.identity;

                //Set timer
                timer = 5.0f;
            }

            // Calculate average vertical displacement from recorded position
            disp = avg_hands.y - hand_init_height;

            // If displacement is above max amount
            if (disp > disp_max)
            {
                // Wall reaches max height
                transform.position = wall_pos_min + vec_3m_height;
            }

            // Else if displacement is positive
            else if (disp > 0.0f)
            {
                // Wall moves up amount of displacement
                transform.position = wall_pos_min + (disp / disp_max) * vec_3m_height;
            }

            // Else
            else
            {
                // Wall stays at min position
                transform.position = wall_pos_min;
            }

            // Save pos and rot
            wall_cur_pos = transform.position;
            wall_cur_rot = transform.rotation;
        }

        // Else
        else
        {

            // Wall experiences gravity
            if(timer > 0.0f){
                timer -= Time.deltaTime;
                transform.position = wall_cur_pos;
                transform.rotation = wall_cur_rot;
            }
            else{
                this.GetComponent<Rigidbody>().isKinematic = false;
                this.GetComponent<Rigidbody>().useGravity = true;
            }

            // If wall position is wall_pos_min, set to null and disable
            if (transform.position.y <= wall_pos_min.y)
            {
                wall_active = false;
                transform.position = wall_pos_min;
                this.GetComponent<MeshRenderer>().enabled = false;
                this.GetComponent<Rigidbody>().isKinematic = false;
            }

        }

    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
        }
    }

}

