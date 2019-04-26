using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterThrow : MonoBehaviour {

    //Intialize external public elements
    public OVRInput.Controller controller;
    public GameObject[] projectiles;
    public GameObject player;

    //Initialize private elements
    private int elementIndex;
    private float thrust;
    private GameObject selectedItem;
    private GameObject trackingSpace;
    private GameObject waterball;
    private Vector3 controller_pos;
    private Vector3 vec_to_pos;
    private Vector3 end_pos;
    private Vector3 above_ground;
    private Vector3 forward;
    private Quaternion controller_rot;

    //Initialize private constants
    private float trigger_thresh = 0.5f; //0 to 1, threshold for trigger activation
    private float nextFire;
    private float fireRate = 0.5f; //In sec
    private float float_dist = 2.0f; //End distance (in m) away from hand
    private float attraction_const = 0.02f; //0 to 1, Larger -> Faster attraction
    private float thrust_const = 175.0f; //Constant of thrust
    private int projectileLifetime = 20;



	// Use this for initialization
	void Start () {
        // Get left and right hand game objects
        trackingSpace = player.transform.Find("OVRCameraRig").gameObject.transform.Find("TrackingSpace").gameObject;

        // Set waterball to null
        waterball = null;

        // Set initial nextfire time to 0 sec
        nextFire = 0.0f;

        // Set height of waterball instantiation
        above_ground = new Vector3(0.0f, 0.5f, 0.0f);
        forward = new Vector3(0.0f,0.0f,1.0f);
	}
	
	// Update is called once per frame
	void Update () {

        //Record current element index of hand
        elementIndex = this.GetComponent<BallShooting>().elementIndex;

        //If Water is current element in hand
        if(elementIndex == 1){

            //If waterball is held, check trigger
            if(waterball != null){

                if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > trigger_thresh){

                    //Calculate expected end position of waterball
                    controller_pos = OVRInput.GetLocalControllerPosition(controller);
                    controller_rot = OVRInput.GetLocalControllerRotation(controller);

                    //Record expected end position of waterball
                    end_pos = controller_pos + controller_rot*forward*float_dist + trackingSpace.transform.position;

                    // Bring waterball a fraction closer to expected position each update
                    vec_to_pos = end_pos - waterball.transform.position;
                    waterball.transform.position += vec_to_pos*attraction_const;            
                }

                // If trigger not held, throw waterball
                else{
                    // Launch waterball
                    controller_rot = OVRInput.GetLocalControllerRotation(controller);
                    waterball.GetComponent<Rigidbody>().AddForce(controller_rot*forward*thrust_const);
                    Destroy(waterball, projectileLifetime);
                    waterball = null;
                }

            }

            //If waterball is not held, check trigger
            else{

                 // Instantiate/control waterball if index trigger is held and cooldown period has passed
                if((OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > trigger_thresh)&&(Time.time > nextFire)){
            
                    //Record current selected item
                    selectedItem = this.GetComponent<BallShooting>().selectedItem;
                    
                    // If selected item is not null
                    if(selectedItem != null){

                        // If controller pointed at ground, produce new object
                        if(selectedItem.tag == "Water"){
                            waterball = Instantiate(projectiles[elementIndex], this.GetComponent<BallShooting>().hitpoint + above_ground, Quaternion.identity);

                            //Update next firing time
                            nextFire = Time.time + fireRate;                
                        }

                        else if(selectedItem.tag == "Waterball"){
                            waterball = selectedItem;
                            waterball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            waterball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; 

                            //Update next firing time
                            nextFire = Time.time + fireRate;
                        }
                    }
                }
            }
        }
    }
} 
        
        

    
