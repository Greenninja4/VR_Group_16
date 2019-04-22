using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EarthRock : MonoBehaviour {

    //Intialize external public elements
    public OVRInput.Controller Lcontroller;
    public OVRInput.Controller Rcontroller;
    public GameObject[] projectiles;
    public GameObject player;

    //Initialize private elements
    private int elementIndex_l;
    private int elementIndex_r;
    private float thrust;
    private GameObject selectedItem_l;
    private GameObject selectedItem_r;
    private GameObject leftHandAnchor;
    private GameObject rightHandAnchor;
    private GameObject trackingSpace;
    private GameObject rock_l;
    private GameObject rock_r;
    private Vector3 hand_vec_l;
    private Vector3 hand_norm_l;
    private Vector3 hand_norm_r;
    private Vector3 vec_to_pos;
    private Vector3 end_pos;

    //Initialize private constants
    private float trigger_thresh = 0.5f; //0 to 1, threshold for trigger activation
    private float nextFire;
    private float fireRate = 0.5f; //In sec
    private float float_dist = 2.0f; //End distance (in m) away from hand
    private float attraction_const = 0.2f; //0 to 1, Larger -> Faster attraction
    private float thrust_const = 10.0f; //Constant of thrust



	// Use this for initialization
	void Start () {
        // Get left and right hand game objects
        trackingSpace = player.transform.Find("OVRCameraRig").gameObject.transform.Find("TrackingSpace").gameObject;
        leftHandAnchor = trackingSpace.transform.Find("LeftHandAnchor").gameObject;
        rightHandAnchor = trackingSpace.transform.Find("RightHandAnchor").gameObject;

        // Set left and right rocks to null
        rock_l = null;
        rock_r = null;

        // Set initial nextfire time to 0 sec
        nextFire = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {

        //Record current element index of each hand
        elementIndex_l = leftHandAnchor.GetComponent<BallShooting>().elementIndex;
        elementIndex_r = rightHandAnchor.GetComponent<BallShooting>().elementIndex;

        //If Earth is current element in left
        if(elementIndex_l == 0){

            //If rock is held, check trigger
            if(rock_l != null){

                if(OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Lcontroller) > trigger_thresh){

                    //Calculate normalized hand vector
                    hand_vec_l = OVRInput.GetLocalControllerPosition(OVRInput.Controller.LTouch);
                    hand_norm_l = hand_vec_l.normalized;

                    //Record expected end position of rock
                    end_pos = player.transform.position + float_dist*hand_norm_l;

                    // Bring rock a fraction closer to expected position each update
                    vec_to_pos = end_pos - rock_l.transform.position;
                    transform.position += vec_to_pos*attraction_const;            
                }

                // If trigger not held, throw rock
                else{
                    // Launch rock
                    thrust = thrust_const/(Vector3.Distance(end_pos, rock_l.transform.position)+1.0f);
                    rock_l.GetComponent<Rigidbody>().AddForce(hand_norm_l*thrust);
                }

            }

            //If rock is not held, check trigger
            else{

                 // Instantiate/control rock if index trigger is held and cooldown period has passed
                if((OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger, Lcontroller) > trigger_thresh)&&(Time.time > nextFire)){
            
                    //Record current selected item
                    selectedItem_l = leftHandAnchor.GetComponent<BallShooting>().selectedItem;

                    // If controller pointed at ground, produce new object
                    if(selectedItem_l.tag == "Ground"){
                        rock_l = Instantiate(projectiles[elementIndex_l], leftHandAnchor.GetComponent<BallShooting>().hitpoint, Quaternion.identity);

                        //Update next firing time
                        nextFire = Time.time + fireRate;                
                    }

                    else if(selectedItem_l.tag == "Rock"){
                        rock_l = selectedItem_l;

                        //Update next firing time
                        nextFire = Time.time + fireRate;
                    }
                }
            }
        }
    }
} 
        
        

    // void OnCollisionEnter(Collision collision)
    // {
    //     if (collision.gameObject.tag == "Ground")
    //     {
    //         if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.5){
    //             Physics.IgnoreCollision(collision.gameObject.GetComponent<Collider>(), GetComponent<Collider>());
    //         }
    //     }
    // }
