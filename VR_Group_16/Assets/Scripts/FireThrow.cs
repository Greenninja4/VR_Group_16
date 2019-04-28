using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireThrow : MonoBehaviour {

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
    private GameObject fireball;
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

        // Set fireball to null
        fireball = null;

        // Set initial nextfire time to 0 sec
        nextFire = 0.0f;

        // Set fireball instantiation direction
        forward = new Vector3(0.0f,0.0f,1.0f);
	}
	
	// Update is called once per frame
	void Update () {

        //Record current element index of hand
        elementIndex = this.GetComponent<BallShooting>().elementIndex;

        //If fire is current element in hand
        if(elementIndex == 3){

            //If fireball is held, check trigger
            if(fireball != null){

                if(OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > trigger_thresh){

                    //Calculate expected end position of fireball
                    controller_pos = OVRInput.GetLocalControllerPosition(controller);
                    controller_rot = OVRInput.GetLocalControllerRotation(controller);

                    //Record expected end position of fireball
                    end_pos = controller_pos + controller_rot*forward*float_dist + trackingSpace.transform.position;

                    // Bring fireball to expected position each update
                    fireball.transform.position = end_pos;            
                }

                // If trigger not held, throw fireball
                else{
                    // If enough stamina
                    if (statusBars.GetComponent<PlayerBars>().EnoughStamina()){
                        // Launch fireball
                        controller_rot = OVRInput.GetLocalControllerRotation(controller);
                        fireball.GetComponent<Rigidbody>().AddForce(controller_rot*forward*thrust_const);
                        Destroy(fireball, projectileLifetime);
                        fireball = null;
                        // Update stamina bars
                        statusBars.GetComponent<PlayerBars>().UseStamina(staminaRequired);
                    }
                }

            }

            //If fireball is not held, check trigger
            else{

                 // Instantiate/control fireball if index trigger is held and cooldown period has passed
                if((OVRInput.Get(OVRInput.Axis1D.PrimaryIndexTrigger, controller) > trigger_thresh)&&(Time.time > nextFire)){
            
                    //Record current selected item
                    selectedItem = this.GetComponent<BallShooting>().selectedItem;
                    
                    // If selected item is not null
                    if(selectedItem != null){

                        if(selectedItem.tag == "Fireball"){
                            fireball = selectedItem;
                            fireball.GetComponent<Rigidbody>().velocity = Vector3.zero;
                            fireball.GetComponent<Rigidbody>().angularVelocity = Vector3.zero; 

                            //Update next firing time
                            nextFire = Time.time + fireRate;
                        }

                        else{
                            //If controller not pointed at previous fireball, instantiate new object
                            controller_pos = OVRInput.GetLocalControllerPosition(controller);
                            controller_rot = OVRInput.GetLocalControllerRotation(controller);
                            end_pos = controller_pos + controller_rot*forward*float_dist + trackingSpace.transform.position;
                            fireball = Instantiate(projectiles[elementIndex], end_pos, Quaternion.identity);

                            //Update next firing time
                            nextFire = Time.time + fireRate;                
                        }
                    }

                    else{
                        //If controller not pointed at previous fireball, instantiate new object
                        controller_pos = OVRInput.GetLocalControllerPosition(controller);
                        controller_rot = OVRInput.GetLocalControllerRotation(controller);
                        end_pos = controller_pos + controller_rot*forward*float_dist + trackingSpace.transform.position;
                        fireball = Instantiate(projectiles[elementIndex], end_pos, Quaternion.identity);

                        //Update next firing time
                        nextFire = Time.time + fireRate;                

                    }
                }
            }
        }
    }
} 
        
        
        

    
