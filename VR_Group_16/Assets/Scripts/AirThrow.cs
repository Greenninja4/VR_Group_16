using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirThrow : MonoBehaviour {

    //Intialize external public elements
    public OVRInput.Controller controller;
    public GameObject[] projectiles;
    public GameObject player;
    
    public GameObject statusBars;

    //Initialize private elements
    private int elementIndex;
    private float thrust;
    private GameObject selectedItem;
    private GameObject trackingSpace;
    private GameObject airball;
    private Vector3 controller_pos;
    private Vector3 end_pos;
    private Vector3 forward;
    private Quaternion controller_rot;

    //Initialize private constants
    private float trigger_thresh = 0.5f; //0 to 1, threshold for trigger activation
    private float nextFire;
    private float fireRate = 4.0f; //In sec
    private float float_dist = 2.0f; //End distance (in m) away from hand
    private float thrust_const = 175.0f; //Constant of thrust
    private int projectileLifetime = 20;
    public float staminaRequired = 20;



	// Use this for initialization
	void Start () {
        // Get left and right hand game objects
        trackingSpace = player.transform.Find("OVRCameraRig").gameObject.transform.Find("TrackingSpace").gameObject;

        // Set airball to null
        airball = null;

        // Set initial nextfire time to 0 sec
        nextFire = 0.0f;

        // Set airball instantiation direction
        forward = new Vector3(0.0f,0.0f,1.0f);
	}
	
	// Update is called once per frame
	void Update () {

        //Record current element index of hand
        elementIndex = this.GetComponent<BallShooting>().elementIndex;

        //If Air is current element in hand
        if(elementIndex == 2){

            //If airball is held, check trigger
            if(airball != null){

                if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > trigger_thresh){

                    //Calculate expected end position of airball
                    controller_pos = OVRInput.GetLocalControllerPosition(controller);
                    controller_rot = OVRInput.GetLocalControllerRotation(controller);

                    //Record expected end position of airball
                    end_pos = controller_pos + controller_rot*forward*float_dist + trackingSpace.transform.position;

                    // Bring airball to expected position each update
                    airball.transform.position = end_pos;            
                }

                // If trigger not held, throw airball
                else{
                    // If enough stamina
                    if (statusBars.GetComponent<PlayerBars>().EnoughStamina()){
                        // Launch airball
                        controller_rot = OVRInput.GetLocalControllerRotation(controller);
                        airball.GetComponent<Rigidbody>().AddForce(controller_rot*forward*thrust_const);
                        Destroy(airball, projectileLifetime);
                        airball = null;
                        // Update stamina bars
                        statusBars.GetComponent<PlayerBars>().UseStamina(staminaRequired);
                    }
                }

            }

            //If airball is not held, check trigger
            else{

                 // Instantiate/control airball if index trigger is held and cooldown period has passed
                if((OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > trigger_thresh)&&(Time.time > nextFire)){
            
                    //Record current selected item
                    selectedItem = this.GetComponent<BallShooting>().selectedItem;
                    
                    // If selected item is not null
                    if(selectedItem != null){

                        if(selectedItem.tag == "Airball"){
                            airball = selectedItem;
                            airball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            airball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; 

                            //Update next firing time
                            nextFire = Time.time + fireRate;
                        }

                        else{
                            //If controller not pointed at previous airball, instantiate new object
                            controller_pos = OVRInput.GetLocalControllerPosition(controller);
                            controller_rot = OVRInput.GetLocalControllerRotation(controller);
                            end_pos = controller_pos + controller_rot*forward*float_dist + trackingSpace.transform.position;
                            airball = Instantiate(projectiles[elementIndex], end_pos, Quaternion.identity);

                            //Update next firing time
                            nextFire = Time.time + fireRate;                
                        }
                    }

                    else{
                        //If controller not pointed at previous airball, instantiate new object
                        controller_pos = OVRInput.GetLocalControllerPosition(controller);
                        controller_rot = OVRInput.GetLocalControllerRotation(controller);
                        end_pos = controller_pos + controller_rot*forward*float_dist + trackingSpace.transform.position;
                        airball = Instantiate(projectiles[elementIndex], end_pos, Quaternion.identity);

                        //Update next firing time
                        nextFire = Time.time + fireRate;                

                    }
                }
            }
        }
    }
} 
        
        

    
